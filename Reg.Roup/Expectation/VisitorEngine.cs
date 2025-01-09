using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public class VisitorEngine : IEvaluationFrame.IStackController
{
    private readonly DroneVisitor _drone;
    private readonly Stack<IEvaluationFrame> _frames = new();
    private readonly IBaseExpectation _root;

    public VisitorEngine(IBaseExpectation root)
    {
        _drone = new DroneVisitor(OnDroneVisited);
        _root = root;
    }

    public Expression? Visit(Expression? node)
    {
        _frames.Clear();
        _frames.Push(new RootFrame(_root));
        return _drone.Visit(node);
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
        var frame = _frames.Peek().SeekNext(node);

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
        _frames.Peek().PopFrom(this);
    }

    public void TryPushFrame(IEvaluationFrame? frame)
    {
        if (frame == null)
        {
            return;
        }

        Console.WriteLine($"Pushing {frame.Origin.GetType().Name}");
        _frames.Push(frame);
    }

    public IEvaluationFrame? PopFrame()
    {
        var f = _frames.TryPop(out var frame)
            ? frame
            : null;

        Console.WriteLine($"Poped {f?.Origin.GetType().Name ?? "n/a"}");
        return f;
    }

    private class DroneVisitor : ExpressionVisitor
    {
        public delegate Expression? VisitBase(Expression? node);

        private readonly Func<Expression?, VisitBase, Expression?> _onVisit;

        public DroneVisitor(Func<Expression?, VisitBase, Expression?> onVisit)
            => _onVisit = onVisit;

        [return: NotNullIfNotNull("node")]
        public sealed override Expression? Visit(Expression? node)
            => _onVisit(node, base.Visit);
    }

    private class RootFrame : EvaluationFrame
    {
        public RootFrame(IBaseExpectation rootExpectation)
            : base(rootExpectation, rootExpectation.BuildFrame)
        {
        }
    }
}