using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public interface IEvaluatorNodeFactory
{
    IEvaluatorNode ToEvaluator();
}

// TODO figure out better naming alignment w/ IExpressionVisitorNode
public interface IEvaluatorBuilder : IEvaluatorNodeFactory
{
    void AddCondition(ICondition condition);
    void AddChildren(Func<Expression, IEnumerable<IEvaluatorNodeFactory>> getChildren);
}

public interface IEvaluatorBuilder<TNode> : IEvaluatorBuilder
{
    // TODO
    //  can this be merged into IEvaluatorBuilder above,
    //  replacing usages of IEvaluatorBuilder w/ IEvaluatorBuilder<Expression>?
    //  perhaps these two could/should just be concrete implementations now?
}

public interface IVisitorNodeFactory
{
    IEvaluatorBuilder OfType(ExpressionType nodeType);
    IEvaluatorBuilder<TExpression> OfType<TExpression>(ExpressionType? nodeType = null) where TExpression : Expression;
    IEvaluatorBuilder OneOf(params IEvaluatorBuilder[] children);
}

public class ExpressionTreeEvaluator
{
    private readonly RootNode _rootNode;

    private ExpressionTreeEvaluator(RootNode rootNode)
    {
        _rootNode = rootNode;
    }

    public static ExpressionTreeEvaluator Create(Func<IVisitorNodeFactory, IEvaluatorBuilder> doIt)
    {
        var factory = new VisitorNodeFactory();

        var root = doIt(factory);

        if (root == null)
        {
            throw new NotSupportedException();
        }

        return new ExpressionTreeEvaluator(new RootNode(root.ToEvaluator()));
    }

    // TODO find a way to return the expression tree here
    public void Evaluate(Expression expressionTree)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(expressionTree).ToArray();
        var head = new TapeHead(tape);

        var visitation = new VisitationContext(head);

        try
        {
            _rootNode.Evaluate(visitation);
        }
        catch (Exception)
        {
            throw new TreeRejectedException();
        }

        if (visitation.HasRejection)
        {
            throw new TreeRejectedException();
        }
    }

    private class RootNode : IEvaluatorNode
    {
        private readonly IEvaluatorNode _tree;

        public RootNode(IEvaluatorNode tree)
        {
            _tree = tree;
        }

        public Expression? Evaluate(IVisitationContext context)
        {
            var result = _tree.Evaluate(context);

            if (context.CanMoveForward())
            {
                // TODO reevaluated whether this should be tree rejected and not some other ex type
                throw new TreeRejectedException();
            }

            return result;
        }
    }

    private class EvaluatorNode : IEvaluatorNode
    {
        private readonly ICondition[] _conditions;
        private readonly IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> _childLookups;

        public EvaluatorNode(ICondition[] conditions,
            IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
        {
            _conditions = conditions;
            _childLookups = childLookups;
        }

        public Expression? Evaluate(IVisitationContext context)
        {
            var current = context.MoveForward();

            try
            {
                var failedConditions = _conditions.Where(c => !c.Evaluate(current)).ToArray();

                if (failedConditions.Any())
                {
                    // TODO pass in failedConditions
                    context.Reject(this);

                    // TODO figure out method of returning an Expression
                    return null;
                }
            }
            catch (TreeRejectedException)
            {
                context.Reject(this);
                return null;
            }

            foreach (var child in _childLookups.SelectMany(lookup => lookup(current!)))
            {
                var visitor = child.ToEvaluator();
                visitor.Evaluate(context);

                if (context.HasRejection)
                {
                    // TODO figure out method of returning an Expression
                    context.Reject(this);
                    return null;
                }
            }

            context.Accept(this);

            // TODO figure out method of returning an Expression
            return null;
        }
    }

    public abstract class EvaluatorBuilderBase : IEvaluatorBuilder
    {
        private readonly List<ICondition> _conditions = new(1);
        private readonly List<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> _childLookups = new(1);

        protected EvaluatorBuilderBase()
        {
            AddCondition(new NotNullCondition());
        }

        public virtual IEvaluatorNode ToEvaluator()
            => new EvaluatorNode(_conditions.ToArray(), _childLookups);

        public void AddCondition(ICondition condition)
            => _conditions.Add(condition);

        public void AddChildren(Func<Expression, IEnumerable<IEvaluatorNodeFactory>> getChildren)
            => _childLookups.Add(getChildren);
    }

    private class EvaluatorBuilder : EvaluatorBuilderBase
    {
        public ExpressionType NodeType { get; }

        public EvaluatorBuilder(ExpressionType nodeType)
        {
            NodeType = nodeType;
            AddCondition(new NodeTypeCondition(nodeType));
        }
    }

    private class EvaluatorBuilder<TNode> : EvaluatorBuilderBase, IEvaluatorBuilder<TNode>
        where TNode : Expression
    {
        public ExpressionType? NodeType { get; }

        public EvaluatorBuilder(ExpressionType? nodeType)
        {
            NodeType = nodeType;
            AddCondition(new NodeTypeCondition(typeof(TNode), nodeType)
            {
                Throw = new TreeRejectedException()
            });
        }
    }

    private class OneOfNode : EvaluatorBuilderBase
    {
        private readonly IEvaluatorBuilder[] _options;

        public OneOfNode(IEvaluatorBuilder[] options)
        {
            _options = options;
        }

        // TODO
        //  this doesn't handle conditions
        //  it probably shouldn't be able to have children
        public override IEvaluatorNode ToEvaluator()
            => new OneOfVisitor(_options);

        private class OneOfVisitor : IEvaluatorNode
        {
            private readonly IEvaluatorBuilder[] _options;

            public OneOfVisitor(IEvaluatorBuilder[] options)
            {
                _options = options;
            }

            public Expression? Evaluate(IVisitationContext context)
            {
                foreach (var option in _options)
                {
                    var tracker = context.Try(copy =>
                    {
                        var visitor = option.ToEvaluator();
                        visitor.Evaluate(copy);
                    });

                    if (!tracker.HasRejection)
                    {
                        context.Accept(this);
                        // TODO figure out method of returning an Expression
                        return null;
                    }
                }

                context.Reject(this);

                // TODO figure out method of returning an Expression
                throw new NotSupportedException("No OneOf matched expression");
            }
        }
    }

    private class VisitorNodeFactory : IVisitorNodeFactory
    {
        public IEvaluatorBuilder OfType(ExpressionType nodeType)
            => new EvaluatorBuilder(nodeType);

        public IEvaluatorBuilder<TExpression> OfType<TExpression>(ExpressionType? nodeType = null)
            where TExpression : Expression
            => new EvaluatorBuilder<TExpression>(nodeType);

        public IEvaluatorBuilder OneOf(IEvaluatorBuilder[] children)
            => new OneOfNode(children);

        public IEvaluatorBuilder OneOf(Func<IEvaluatorBuilder[]> buildChildren)
            => OneOf(buildChildren());
    }
}
