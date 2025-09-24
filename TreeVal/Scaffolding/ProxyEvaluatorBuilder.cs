using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class ProxyEvaluatorBuilder : VisitorBuilder
{
    private readonly Func<
            IEnumerable<Func<Node, IEnumerable<ICondition>>>,
            IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>>,
            IVisitor
        >?
        _toEvaluator;

    protected ProxyEvaluatorBuilder()
    {
    }

    public ProxyEvaluatorBuilder(
        Func<
                IEnumerable<Func<Node, IEnumerable<ICondition>>>,
                IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>>,
                IVisitor
            >
            toEvaluator)
    {
        _toEvaluator = toEvaluator;
    }

    public static (
        IEnumerable<Func<Node, IEnumerable<ICondition>>> conditionLookups,
        IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>> childLookups
        ) Scoped(Action<IVisitorBuilder> body)
    {
        var builder = new ProxyEvaluatorBuilder();
        body(builder);
        return (builder.ConditionLookups, builder.ChildLookups);
    }

    public override IVisitor CreateVisitor(Node node)
        => _toEvaluator?.Invoke(conditionLookups, childLookups)
           ?? throw new NotSupportedException();
}

public class ProxyEvaluatorBuilder<T> : ProxyEvaluatorBuilder, IVisitorBuilder<T>
{
    private ProxyEvaluatorBuilder()
    {
    }

    public ProxyEvaluatorBuilder(
        Func<
                IEnumerable<Func<Node, IEnumerable<ICondition>>>,
                IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>>,
                IVisitor
            >
            toEvaluator)
        : base(toEvaluator)
    {
    }

    public static (
        IEnumerable<Func<Node, IEnumerable<ICondition>>> conditionLookups,
        IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>> childLookups
        ) Scoped(Action<IVisitorBuilder<T>> body)
    {
        var builder = new ProxyEvaluatorBuilder<T>();
        body(builder);
        return (builder.ConditionLookups, builder.ChildLookups);
    }
}

// using TreeVal.Eval;
// using TreeVal.Media;
//
// namespace TreeVal.Scaffolding;
//
// public class ProxyEvaluatorBuilder : VisitorBuilder
// {
//     private readonly Func<IEnumerable<Func<Node, IEnumerable<IBehavior>>>, IVisitor>? _toEvaluator;
//
//     // private readonly Func<
//     //         IEnumerable<ICondition>,
//     //         IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>>,
//     //         INodeEvaluator
//     //     >?
//     //     _toEvaluator;
//
//     protected ProxyEvaluatorBuilder(IVisitorBuilderFactory originator)
//         : base(originator)
//     {
//     }
//
//     public ProxyEvaluatorBuilder(
//         IVisitorBuilderFactory originator, Func<IEnumerable<Func<Node, IEnumerable<IBehavior>>>, IVisitor> toEvaluator)
//         : base(originator)
//     {
//         _toEvaluator = toEvaluator;
//     }
//
//     // public ProxyEvaluatorBuilder(
//     //     Func<
//     //             IEnumerable<ICondition>,
//     //             IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>>,
//     //             INodeEvaluator
//     //         >
//     //         toEvaluator)
//     // {
//     //     _toEvaluator = toEvaluator;
//     // }
//
//     public static IEnumerable<Func<Node, IEnumerable<IBehavior>>> Scoped(Action<IVisitorBuilder> body)
//     {
//         var builder = new ProxyEvaluatorBuilder();
//         body(builder);
//         return builder.BehaviorLookups;
//     }
//
//     // public static (
//     //     IEnumerable<ICondition> conditions,
//     //     IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups
//     //     ) Scoped(Action<IEvaluatorBuilder> body)
//     // {
//     //     var builder = new ProxyEvaluatorBuilder();
//     //     body(builder);
//     //     return (builder.Conditions, builder.ChildLookups);
//     // }
//
//     protected override IVisitor ToEvaluatorImpl(IEnumerable<Func<Node, IEnumerable<IBehavior>>> behaviorLookups)
//         => _toEvaluator?.Invoke(behaviorLookups)
//            ?? throw new NotSupportedException();
//
//     // protected override INodeEvaluator ToEvaluatorImpl(
//     //     IEnumerable<ICondition> conditions,
//     //     IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups)
//     //     => _toEvaluator?.Invoke(conditions.ToArray(), childLookups)
//     //        ?? throw new NotSupportedException();
// }
//
// public class ProxyEvaluatorBuilder<T> : ProxyEvaluatorBuilder, IVisitorBuilder<T>
// {
//     private ProxyEvaluatorBuilder()
//     {
//     }
//
//     public ProxyEvaluatorBuilder(Func<IEnumerable<Func<Node, IEnumerable<IBehavior>>>, IVisitor> toEvaluator)
//         : base(toEvaluator)
//     {
//     }
//
//     // public ProxyEvaluatorBuilder(
//     //     Func<
//     //             IEnumerable<ICondition>,
//     //             IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>>,
//     //             INodeEvaluator
//     //         >
//     //         toEvaluator)
//     //     : base(toEvaluator)
//     // {
//     // }
//
//     public static IEnumerable<Func<Node, IEnumerable<IBehavior>>> Scoped(Action<IVisitorBuilder<T>> body)
//     {
//         var builder = new ProxyEvaluatorBuilder<T>();
//         body(builder);
//         return builder.BehaviorLookups;
//     }
//
//     // public static (
//     //     IEnumerable<ICondition> conditions,
//     //     IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups
//     //     ) Scoped(Action<IEvaluatorBuilder<T>> body)
//     // {
//     //     var builder = new ProxyEvaluatorBuilder<T>();
//     //     body(builder);
//     //     return (builder.Conditions, builder.ChildLookups);
//     // }
// }
