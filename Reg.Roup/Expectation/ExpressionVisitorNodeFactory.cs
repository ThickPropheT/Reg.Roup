using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Reg.Roup.Expectation._RecycleBin;

namespace Reg.Roup.Expectation;

public interface IVisitorFactory
{
    // TODO improve naming
    IExpressionVisitorNode ToVisitor();
}

// TODO figure out better naming alignment w/ IExpressionVisitorNode
public interface IVisitorNode : IVisitorFactory
{
    // TODO merge this w/ IVisitorFactory
}

public interface IVisitorNode<out TNode> : IVisitorNode
{
    IVisitorNode<TNode> Where(Func<TNode, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = "");

    new IVisitorNode<TNode> HavingChild(IVisitorNode child);
    new IVisitorNode<TNode> HavingChildren(IVisitorNode[] children);
    new IVisitorNode<TNode> HavingChildren(Func<TNode, IVisitorNode[]> buildChildren);

    IVisitorNode<TNode> WithEachChildBeing<TChild>(Func<TNode, IEnumerable<TChild>> findChildren,
        Func<TChild, IVisitorNode> buildChildren);
}

public interface IVisitorNodeFactory
{
    IVisitorNode OfType(ExpressionType nodeType);
    IVisitorNode<TExpression> OfType<TExpression>(ExpressionType? nodeType = null) where TExpression : Expression;
    IVisitorNode OneOf(params IVisitorNode[] children);
}

public class ExpressionVisitorNodeFactory
{
    private readonly RootNode _rootNode;

    private ExpressionVisitorNodeFactory(RootNode rootNode)
    {
        _rootNode = rootNode;
    }
    
    public static ExpressionVisitorNodeFactory Create(Func<IVisitorNodeFactory, IVisitorNode> doIt)
    {
        var factory = new VisitorNodeFactory();

        var root = doIt(factory);

        if (root == null)
        {
            throw new NotSupportedException();
        }

        return new ExpressionVisitorNodeFactory(new RootNode(root.ToVisitor()));
    }

    // TODO find a way to return the expression tree here
    public void Evaluate(Expression expressionTree)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(expressionTree).ToArray();
        var head = new TapeHead(tape);
        
        var visitation = new VisitationContext(head);

        _rootNode.Visit(visitation);
    }

    private class RootNode : IExpressionVisitorNode
    {
        private readonly IExpressionVisitorNode _tree;

        public RootNode(IExpressionVisitorNode tree)
        {
            _tree = tree;
        }

        public Expression? Visit(IVisitationContext context)
        {
            var result = _tree.Visit(context);

            if (context.CanMoveForward())
            {
                throw new NotSupportedException();
            }

            return result;
        }
    }

    private class ExpressionVisitorNode : IExpressionVisitorNode
    {
        private readonly ICondition[] _conditions;
        private readonly IVisitorFactory[] _children;

        public ExpressionVisitorNode(ICondition[] conditions, IVisitorFactory[] children)
        {
            _conditions = conditions;
            _children = children;
        }

        public Expression? Visit(IVisitationContext context)
        {
            var current = context.MoveForward();

            var failedConditions = _conditions.Where(c => !c.Evaluate(current)).ToArray();

            if (failedConditions.Any())
            {
                // TODO pass in failedConditions
                context.Reject(this);

                foreach (var failedCondition in failedConditions)
                {
                    failedCondition.Evaluate(current);
                }

                // TODO figure out method of returning an Expression
                return null;
            }

            foreach (var child in _children)
            {
                var visitor = child.ToVisitor();
                visitor.Visit(context);

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

    public abstract class VisitorNodeBase : IVisitorNode
    {
        private readonly List<ICondition> _conditions = new(1);
        private readonly List<IVisitorFactory> _children = new(1);

        protected VisitorNodeBase()
        {
            AddCondition(new NotNullCondition());
        }

        public IVisitorNode Where(Func<Expression?, bool> predicate,
            [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "")
        {
            AddCondition(new WhereCondition(predicateExpression, predicate));
            return this;
        }

        public IVisitorNode HavingChild(IVisitorNode child)
            => HavingChildren([child]);

        public IVisitorNode HavingChildren(IVisitorNode[] children)
        {
            // TODO
            AddChildren((IVisitorNode[])children);
            return this;
        }

        public IVisitorNode HavingChildren(Func<IVisitorNode[]> buildChildren)
            => HavingChildren(buildChildren());

        public virtual IExpressionVisitorNode ToVisitor()
            => new ExpressionVisitorNode(_conditions.ToArray(), _children.ToArray());

        protected void AddCondition(ICondition condition)
            => _conditions.Add(condition);

        protected void AddChildren(params IVisitorFactory[] children)
            => _children.AddRange(children);
    }

    private class VisitorNode : VisitorNodeBase
    {
        public ExpressionType NodeType { get; }

        public VisitorNode(ExpressionType nodeType)
        {
            NodeType = nodeType;
            AddCondition(new NodeTypeCondition(nodeType));
        }
    }

    private class VisitorNode<TNode> : VisitorNodeBase, IVisitorNode<TNode>
        where TNode : Expression
    {
        public ExpressionType? NodeType { get; }

        public VisitorNode(ExpressionType? nodeType)
        {
            NodeType = nodeType;
            AddCondition(new NodeTypeCondition(typeof(TNode), nodeType));
        }

        public IVisitorNode<TNode> Where(Func<TNode, bool> predicate,
            [CallerArgumentExpression(nameof(predicate))]
            string predicateExpression = "")
        {
            AddCondition(new WhereCondition(predicateExpression, node =>
            {
                if (node is not TNode n)
                {
                    throw new InvalidOperationException();
                }
                
                return predicate(n);
            }));
            return this;
        }

        public new IVisitorNode<TNode> HavingChild(IVisitorNode child)
        {
            base.HavingChild(child);
            return this;
        }

        public new IVisitorNode<TNode> HavingChildren(IVisitorNode[] children)
        {
            base.HavingChildren(children);
            return this;
        }

        public IVisitorNode<TNode> HavingChildren(Func<TNode, IVisitorNode[]> buildChildren)
        {
            AddChildren(new HavingChildrenFactory(buildChildren));
            return this;
        }

        public IVisitorNode<TNode> WithEachChildBeing<TChild>(Func<TNode, IEnumerable<TChild>> findChildren,
            Func<TChild, IVisitorNode> buildChildren)
        {
            AddChildren(new EachChildFactory<TChild>(findChildren, buildChildren));
            return this;
        }

        private class HavingChildrenFactory : IVisitorFactory
        {
            private readonly Func<TNode, IVisitorNode[]> _buildChildren;

            public HavingChildrenFactory(Func<TNode, IVisitorNode[]> buildChildren)
            {
                _buildChildren = buildChildren;
            }

            public IExpressionVisitorNode ToVisitor()
                => new ProxyVisitor((self, context) =>
                {
                    var current = context.ReadCurrent()!;
                    var children = _buildChildren((TNode)current);
                    
                    foreach (var child in children)
                    {
                        var visitor = child.ToVisitor();
                        visitor.Visit(context);

                        if (context.HasRejection)
                        {
                            // TODO figure out method of returning an Expression
                            throw new NotImplementedException();
                        }
                    }

                    context.Accept(self);

                    // TODO figure out method of returning an Expression
                    return null;
                });
        }

        private class EachChildFactory<TChild> : IVisitorFactory
        {
            private readonly Func<TNode, IEnumerable<TChild>> _findChildren;
            private readonly Func<TChild, IVisitorNode> _buildChildren;

            public EachChildFactory(Func<TNode, IEnumerable<TChild>> findChildren,
                Func<TChild, IVisitorNode> buildChildren)
            {
                _findChildren = findChildren;
                _buildChildren = buildChildren;
            }

            public IExpressionVisitorNode ToVisitor()
                => new ProxyVisitor((self, context) =>
                {
                    var current = context.ReadCurrent()!;
                    var children = _findChildren((TNode) current);

                    foreach (var child in children)
                    {
                        var node = _buildChildren(child);
                        var visitor = node.ToVisitor();
                        visitor.Visit(context);

                        if (context.HasRejection)
                        {
                            // TODO figure out method of returning an Expression
                            throw new NotImplementedException();
                        }
                    }

                    context.Accept(self);

                    // TODO figure out method of returning an Expression
                    return null;
                });
        }
    }

    private class OneOfNode : VisitorNodeBase
    {
        private readonly IVisitorNode[] _options;

        public OneOfNode(IVisitorNode[] options)
        {
            _options = options;
        }

        // TODO
        //  this doesn't handle conditions
        //  it probably shouldn't be able to have children
        public override IExpressionVisitorNode ToVisitor()
            => new OneOfVisitor(_options);

        private class OneOfVisitor : IExpressionVisitorNode
        {
            private readonly IVisitorNode[] _options;

            public OneOfVisitor(IVisitorNode[] options)
            {
                _options = options;
            }

            public Expression? Visit(IVisitationContext context)
            {
                foreach (var option in _options)
                {
                    var tracker = context.Try(copy =>
                    {
                        var visitor = option.ToVisitor();
                        visitor.Visit(copy);
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
        public IVisitorNode OfType(ExpressionType nodeType)
            => new VisitorNode(nodeType);

        public IVisitorNode<TExpression> OfType<TExpression>(ExpressionType? nodeType = null)
            where TExpression : Expression
            => new VisitorNode<TExpression>(nodeType);

        public IVisitorNode OneOf(IVisitorNode[] children)
            => new OneOfNode(children);

        public IVisitorNode OneOf(Func<IVisitorNode[]> buildChildren)
            => OneOf(buildChildren());
    }
}
