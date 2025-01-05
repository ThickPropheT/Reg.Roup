using System;

namespace Reg.Roup.Expression
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public class VisitorEngine : IEvaluationFrame.IStackController
    {
        private readonly DroneVisitor drone;
        private readonly Stack<IEvaluationFrame> frames = new();
        private readonly IBaseExpectation root;

        public VisitorEngine(IBaseExpectation root)
        {
            drone = new DroneVisitor(OnDroneVisited);
            this.root = root;
        }

        public Expression? Visit(Expression? node)
        {
            frames.Clear();
            frames.Push(new RootFrame(root));
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
            var frame = frames.Peek().SeekNext(node);

            frame?.PushTo(this);

            return null;

            //var transformer = result.FindTransformer();

            //if (next.Next != null)
            //{
            //    frames.Push(next);
            //}
            //else
            //{

            //}

            //return transformer;
        }

        public void DidVisit(Expression? node, Expression? result)
        {
            frames.Peek().PopFrom(this);
        }

        public void TryPushFrame(IEvaluationFrame? frame)
        {
            if (frame == null)
            {
                return;
            }

            Console.WriteLine($"Pushing {frame.Origin.GetType().Name}");
            frames.Push(frame);
        }

        public IEvaluationFrame? PopFrame()
        {
            var f = frames.TryPop(out var frame)
                    ? frame
                    : null;

            Console.WriteLine($"Poped {f?.Origin.GetType().Name ?? "n/a"}");
            return f;
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

        private class RootFrame : EvaluationFrame
        {
            public RootFrame(IBaseExpectation rootExpectation)
                : base(rootExpectation, rootExpectation.BuildFrame)
            {
            }
        }
    }
}
