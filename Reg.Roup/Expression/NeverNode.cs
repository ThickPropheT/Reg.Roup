// TODO check VisitorEngine history @ d5e03fff for more of this historical stuff

//using System;
//using System.Diagnostics.CodeAnalysis;

//namespace Reg.Roup.Expression
//{
//    using Reg.Roup.Utility;
//    using System.Linq;
//    using System.Linq.Expressions;
//    using System.Reflection;
//    using System.Runtime.CompilerServices;
//    using System.Text;
//    using System.Text.RegularExpressions;
//    using System.Transactions;

//    public class ConstantTransformer : MonitoredVisitor, IVisitationStrategy
//    {
//        private readonly Group group;

//        public ConstantTransformer(IVisitorMonitor monitor, Group group)
//            : base(monitor)
//        {
//            this.group = group;
//        }

//        public SeekResult Seek(Expression? node)
//            => new(this);

//        protected override Expression VisitConstant(ConstantExpression node)
//        {
//            var constantType = node.Type;

//            var readGroupValueExpression = Expression.Property(
//                Expression.Constant(group),
//                nameof(group.Value)
//            );

//            return constantType != typeof(string)
//                ? Expression.Convert(readGroupValueExpression, constantType)
//                : readGroupValueExpression;
//        }

//        private static Expression CastExpression(Expression expression, Type toType)
//            => Expression.Convert(expression, toType);

//        private static Expression ConvertExpression(Expression expression, Type toType)
//            => Expression.Call(null, Convert_ChangeType, expression, Expression.Constant(toType.TryGetNullableType() ?? toType));

//        private static readonly MethodInfo Convert_ChangeType = typeof(Convert).GetMethod(nameof(Convert.ChangeType), [typeof(object), typeof(Type)])!;
//    }

//    public class Nop : _Expect
//    {
//        public Nop()
//            : base(_ => true, _ => null!)
//        {

//        }

//        public override SeekResult Seek(Expression? node)
//            => new(this);
//    }

//    public class ExpectOneOf : IVisitationStrategy
//    {
//        private readonly ITypalVisitationStrategy[] cases;

//        public IVisitationStrategy? Default { get; init; }

//        public ExpectOneOf(params ITypalVisitationStrategy[] cases)
//        {
//            this.cases = cases;
//        }

//        public SeekResult Seek(Expression? node)
//        {
//            foreach (var @case in cases)
//            {
//                try
//                {
//                    if (node?.NodeType == @case.NodeType)
//                    {
//                        return @case.Seek(node);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    // TODO aggregate exceptions
//                }
//            }

//            return Default?.Seek(node)
//                ?? throw new Exception();
//        }
//    }

//    public class ExpectNodeType : __Expect, ITypalVisitationStrategy
//    {
//        public ExpressionType NodeType { get; }

//        public ExpectNodeType(ExpressionType nodeType, Func<Expression?, bool> where, Func<Expression?, SeekResult>? seek = null)
//            : base(e => e?.NodeType == nodeType && where(e), seek)
//        {
//            NodeType = nodeType;
//        }

//        public ExpectNodeType(ExpressionType nodeType, Func<Expression?, SeekResult>? seek = null)
//            : base(e => e?.NodeType == nodeType, seek)
//        {
//            NodeType = nodeType;
//        }
//    }

//    public class __Expect : IVisitationStrategy
//    {
//        private readonly Func<Expression?, bool> where;
//        private readonly Func<Expression?, SeekResult>? seek;

//        public __Expect(Func<Expression?, bool> where, Func<Expression?, SeekResult>? seek = null)
//        {
//            this.where = where;
//            this.seek = seek;
//        }

//        public virtual SeekResult Seek(Expression? node)
//        {
//            if (!where(node))
//            {
//                throw new Exception();
//            }

//            return seek?.Invoke(node)
//                ?? new SeekResult(null);
//        }
//    }

//    public static class ExpectationBuilderExtensions
//    {
//        public static IExpectation NodeType(this IPartialExpectation partial, ExpressionType nodeType)
//            => partial.Where(e => e?.NodeType == nodeType);
//    }

//    public class _Expect<T> : _Expect, IExpectation<T>
//    {
//    }

//    public class _Expect : IPartialExpectation
//    {
//        public IExpectation<T> Select<T>(Func<Expression?, T> selector)
//            => new _Expect<T>();

//        public IExpectation Where(Func<Expression?, bool> condition)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public interface IExpectation<T> : IPartialExpectation
//    {
//    }

//    public interface IExpectation : IPartialExpectation
//    {

//    }

//    public interface IPartialExpectation
//    {
//        IExpectation<T> Select<T>(Func<Expression?, T> selector);
//        IExpectation Where(Func<Expression?, bool> condition);
//    }

//    public interface ITypalVisitationStrategy : IVisitationStrategy
//    {
//        ExpressionType NodeType { get; }
//    }

//    public interface IVisitationStrategy
//    {
//        public SeekResult Seek(Expression? node);
//    }

//    public class SeekResult
//    {
//        public IVisitationStrategy? Next { get; }
//        public MonitoredVisitor? Transformer { get; }

//        public SeekResult(IVisitationStrategy? next, MonitoredVisitor? transformer = null)
//        {
//            Next = next;
//            Transformer = transformer;
//        }

//        public static SeekResult TransformWith(MonitoredVisitor transformer)
//            => new(null, transformer);

//        public static SeekResult TransformWith<TTransformer>(TTransformer transformer)
//            where TTransformer : MonitoredVisitor, IVisitationStrategy
//            => new(transformer, transformer);
//    }

//    //public delegate Expression? VisitationStrategy(Expression? node);

//    //public class Head : ExpressionVisitor
//    //{
//    //    private Dictionary<ExpressionType, VisitationStrategy> visitationStrategies;

//    //    public Head()
//    //    {
//    //        visitationStrategies = FillAll();
//    //    }

//    //    private Dictionary<ExpressionType, VisitationStrategy> FillAll(VisitationStrategy? strategy = null)
//    //    {
//    //        var strat = strategy ?? base.Visit;

//    //        return Enum.GetValues<ExpressionType>()
//    //                    .ToDictionary(
//    //                        k => k,
//    //                        _ => strat
//    //                    );
//    //    }

//    //    public void ResetAll()
//    //        => visitationStrategies = this.FillAll();

//    //    public void Reset(ExpressionType nodeType)
//    //        => visitationStrategies[nodeType] = base.Visit;

//    //    public void SetAll(VisitationStrategy strategy)
//    //        => visitationStrategies = FillAll(strategy);

//    //    public void Set(ExpressionType nodeType, VisitationStrategy strategy)
//    //        => visitationStrategies[nodeType] = strategy;

//    //    [return: NotNullIfNotNull("node")]
//    //    public override Expression? Visit(Expression? node)
//    //        => node != null
//    //            ? visitationStrategies[node.NodeType](node)!
//    //            : null;
//    //}
//}

///*namespace Reg.Roup.Expression
//{
//    using System.Collections.Generic;
//    using System.Linq.Expressions;
//    using System.Runtime.CompilerServices;

//    public class ExpectAnyNode : ExpectNode
//    {
//        public ExpectAnyNode(IVisitorEngine engine)
//            : base(new Finder(engine))
//        {
//        }

//        private class Finder : FinderNode
//        {
//            public Finder(IVisitorEngine engine)
//                : base(engine)
//            {
//            }

//            public override Expression? Visit(Expression? node)
//                => new FoundExpression(node!, () => this);
//        }
//    }

//    public class NewNode : FinderNode<NewExpression>
//    {
//        public NewNode(IVisitorEngine engine, IVisitor<Expression>.Factory<NewExpression> visitorFactory)
//            : base(engine, visitorFactory)
//        {
//        }

//        protected override Expression VisitNew(NewExpression node)
//            => Found(node, base.VisitNew);
//    }

//    public class LambdaNode : FinderNode<LambdaExpression>
//    {
//        public LambdaNode(IVisitorEngine engine, IVisitor<Expression>.Factory<LambdaExpression> visitorFactory)
//            : base(engine, visitorFactory)
//        {
//        }

//        protected override Expression VisitLambda<T>(Expression<T> node)
//            => Found(node, (n, @in) => n.VisitLambda(node));
//    }

//    public class NeverNode : ExpressionVisitor
//    {
//        // TODO improve exception message, excpetion message visitor context, exception type, & method name
//        protected virtual Exception DontCallThat([CallerMemberName] string? callerName = null)
//            => new($"Don't call me {callerName}");

//        protected override Expression VisitBinary(BinaryExpression node) => throw DontCallThat();
//        protected override Expression VisitBlock(BlockExpression node) => throw DontCallThat();
//        protected override CatchBlock VisitCatchBlock(CatchBlock node) => throw DontCallThat();
//        protected override Expression VisitConditional(ConditionalExpression node) => throw DontCallThat();
//        protected override Expression VisitConstant(ConstantExpression node) => throw DontCallThat();
//        protected override Expression VisitDebugInfo(DebugInfoExpression node) => throw DontCallThat();
//        protected override Expression VisitDefault(DefaultExpression node) => throw DontCallThat();
//        protected override Expression VisitDynamic(DynamicExpression node) => throw DontCallThat();
//        protected override ElementInit VisitElementInit(ElementInit node) => throw DontCallThat();
//        protected override Expression VisitExtension(Expression node) => throw DontCallThat();
//        protected override Expression VisitGoto(GotoExpression node) => throw DontCallThat();
//        protected override Expression VisitIndex(IndexExpression node) => throw DontCallThat();
//        protected override Expression VisitInvocation(InvocationExpression node) => throw DontCallThat();
//        protected override Expression VisitLabel(LabelExpression node) => throw DontCallThat();
//        [return: NotNullIfNotNull("node")]
//        protected override LabelTarget? VisitLabelTarget(LabelTarget? node) => throw DontCallThat();
//        protected override Expression VisitLambda<T>(Expression<T> node) => throw DontCallThat();
//        protected override Expression VisitListInit(ListInitExpression node) => throw DontCallThat();
//        protected override Expression VisitLoop(LoopExpression node) => throw DontCallThat();
//        protected override Expression VisitMember(MemberExpression node) => throw DontCallThat();
//        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node) => throw DontCallThat();
//        protected override MemberBinding VisitMemberBinding(MemberBinding node) => throw DontCallThat();
//        protected override Expression VisitMemberInit(MemberInitExpression node) => throw DontCallThat();
//        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node) => throw DontCallThat();
//        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node) => throw DontCallThat();
//        protected override Expression VisitMethodCall(MethodCallExpression node) => throw DontCallThat();
//        protected override Expression VisitNew(NewExpression node) => throw DontCallThat();
//        protected override Expression VisitNewArray(NewArrayExpression node) => throw DontCallThat();
//        protected override Expression VisitParameter(ParameterExpression node) => throw DontCallThat();
//        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node) => throw DontCallThat();
//        protected override Expression VisitSwitch(SwitchExpression node) => throw DontCallThat();
//        protected override SwitchCase VisitSwitchCase(SwitchCase node) => throw DontCallThat();
//        protected override Expression VisitTry(TryExpression node) => throw DontCallThat();
//        protected override Expression VisitTypeBinary(TypeBinaryExpression node) => throw DontCallThat();
//        protected override Expression VisitUnary(UnaryExpression node) => throw DontCallThat();
//    }

//    public abstract class FinderNode<TIn> : FinderNode
//        where TIn : Expression
//    {
//        private readonly IVisitor<Expression>.Factory<TIn> visitorFactory;

//        protected FinderNode(IVisitorEngine engine, IVisitor<Expression>.Factory<TIn> visitorFactory)
//            : base(engine)
//        {
//            this.visitorFactory = visitorFactory;
//        }

//        protected FoundExpression Found<T>(T @in, Func<ExpressionVisitor, T, Expression> next)
//            where T : TIn
//        {
//            var n = visitorFactory(@in);
//            next(n, @in);
//        }
//            //=> new FoundExpression(next(@in), () => visitorFactory(@in)!);
//    }

//    public abstract class FinderNode : ExpressionVisitor, IFinder
//    {
//        private readonly IVisitorEngine engine;

//        protected FinderNode(IVisitorEngine engine)
//        {
//            this.engine = engine;
//        }

//        public override Expression? Visit(Expression? node)
//        {
//            var result = base.Visit(node);

//            if (result == null
//                || result is not FoundExpression fe)
//            {
//                return null;
//            }

//            engine.SetCurrentFinder(fe.GetNext());

//            return fe.Result;
//        }

//        protected class FoundExpression : Expression
//        {
//            private readonly Func<IVisitor<Expression?>> getNext;

//            public Expression Result { get; }

//            public FoundExpression(Expression result, Func<IVisitor<Expression?>> getNext)
//            {
//                Result = result;
//                this.getNext = getNext;
//            }

//            public IVisitor<Expression?> GetNext()
//                => getNext();
//        }
//    }

//    public class RootNode : ExpectNode
//    {
//        public RootNode(IVisitorEngine engine, IFinder finder)
//            : base(finder)
//        {
//            engine.SetCurrentExpector(this);
//        }
//    }

//    public class ExpectNode : ExpressionVisitor, IExpector
//    {
//        private readonly IFinder finder;

//        public ExpectNode(IFinder finder)
//        {
//            this.finder = finder;
//        }

//        [return: NotNullIfNotNull("node")]
//        Expression IVisitor<Expression?, Expression>.Visit(Expression? node)
//            => ExpectFound(node);

//        public override Expression? Visit(Expression? node)
//            => ExpectFound(node);

//        private Expression ExpectFound(Expression? node)
//        {
//            var result = finder.Visit(node);

//            if (result == null)
//            {
//                throw new InvalidOperationException("Required");
//            }

//            return result;
//        }
//    }

//    public class OneOfNode : ExpressionVisitor, IExpector
//    {
//        private readonly IVisitorEngine engine;
//        private readonly IFinder[] children;

//        public OneOfNode(IVisitorEngine engine, params IFinder[] children)
//        {
//            this.engine = engine;
//            this.children = children;
//        }

//        Expression IVisitor<Expression?, Expression>.Visit(Expression? node)
//            => this.ExpectOneFound(node);

//        [return: NotNullIfNotNull("node")]
//        public override Expression? Visit(Expression? node)
//            => this.ExpectOneFound(node);

//        private Expression ExpectOneFound(Expression? node)
//        {
//            foreach (var child in children)
//            {
//                var result = child.Visit(node);

//                if (result != null)
//                {
//                    return result;
//                }
//            }

//            throw new InvalidOperationException("None succeeded");
//        }
//    }

//    public interface IFinder : IVisitor<Expression?>
//    {
//    }

//    public interface IExpector : IVisitor<Expression>
//    {
//    }

//    public interface IVisitor<TIn, TOut>
//    {
//        TOut Visit(TIn @in);
//    }

//    public interface IVisitor<TOut> : IVisitor<Expression?, TOut>
//    {
//        public delegate IVisitor<TOut> Factory<TIn>(TIn @in);
//    }

//    public class DefaultVisitorEngine : ExpressionVisitor, IVisitorEngine
//    {
//        private IVisitor<Expression?, Expression?>? current;

//        public DefaultVisitorEngine(IVisitor<Expression?, Expression?>? current = null)
//        {
//            this.current = current;
//        }

//        public void SetCurrentExpector(IVisitor<Expression> expector)
//            => current = expector!;

//        public void SetCurrentFinder(IVisitor<Expression?> finder)
//            => current = finder;

//        [return: NotNullIfNotNull("node")]
//        public override Expression? Visit(Expression? node)
//        {
//            if (current != null)
//            {
//                return current.Visit(node);
//            }

//            return base.Visit(node);
//        }
//    }

//    public interface IVisitorEngine
//    {
//        void SetCurrentExpector(IVisitor<Expression> expector);
//        void SetCurrentFinder(IVisitor<Expression?> finder);
//    }
//}

//public class VisitorEngine
//{
//    private readonly DroneVisitor drone;
//    private readonly Stack<IVisitationStrategy> strategies = new();
//    private readonly Func<IVisitorMonitor, IVisitationStrategy> root;

//    public VisitorEngine(Func<IVisitorMonitor, IVisitationStrategy> root)
//    {
//        drone = new DroneVisitor(OnDroneVisited);
//        this.root = root;
//    }

//    public Expression? Visit(Expression? node)
//    {
//        strategies.Clear();
//        strategies.Push(root(this));
//        return drone.Visit(node);
//    }
//*/