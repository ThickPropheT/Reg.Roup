using System;

namespace Reg.Roup.Expression
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public class VisitorEngine
    {
        private readonly DroneVisitor drone;

        // TODO
        //  consider storing EvaluationScope/EvaluationFrame here to help control pushing/popping
        private readonly Stack<SeekResult> strategies = new();
        private readonly IBaseExpectation root;

        public VisitorEngine(IBaseExpectation root)
        {
            drone = new DroneVisitor(OnDroneVisited);
            this.root = root;
        }

        public Expression? Visit(Expression? node)
        {
            strategies.Clear();
            strategies.Push(new SeekResult(root));
            return drone.Visit(node);
        }

        private Expression? OnDroneVisited(Expression? node, DroneVisitor.VisitBase @base)
        {
            var transformer = WillVisit(node);

            if (transformer != null)
            {

            }

            var result = transformer != null
                ? transformer(node)
                : @base(node);

            DidVisit(node, result);

            return result;
        }

        private IBaseExpectation.Transformer<Expression?>? WillVisit(Expression? node)
        {
            var result = strategies.Peek().Next!
                // TODO
                //  consider giving expectations control over push/pop by passing engine in here
                .Evaluate(node);

            var next = result.SeekNext();
            var transformer = result.FindTransformer();

            if (next.Next != null)
            {
                strategies.Push(next);
            }
            else
            {

            }

            return transformer;
        }

        public void DidVisit(Expression? node, Expression? result)
        {
            strategies.Peek().OnPop(() => strategies.Pop().Next!);
        }

        private class DroneVisitor : ExpressionVisitor
        {
            public delegate Expression? VisitBase(Expression? node);

            private readonly Func<Expression?, VisitBase, Expression?> onVisit;

            public DroneVisitor(Func<Expression?, VisitBase, Expression?> onVisit)
                => this.onVisit = onVisit;

            [return: NotNullIfNotNull("node")]
            public sealed override Expression? Visit(Expression? node)
                => onVisit(node, base.Visit);
        }
    }
}
