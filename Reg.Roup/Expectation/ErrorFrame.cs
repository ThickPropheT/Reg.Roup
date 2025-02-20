using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Reg.Roup.Expectation;

public class ErrorFrame : IEvaluationFrame
{
    private readonly Func<string, Expression?, IEvaluationFrame.IStackController?, Exception> _buildException;

    public IEvaluationFrameBuilder Origin { get; }

    private ErrorFrame(Func<string, Expression?, IEvaluationFrame.IStackController?, Exception> buildException, IEvaluationFrameBuilder origin)
    {
        _buildException = buildException;
        Origin = origin;
    }

    public static IEvaluationFrame NotFound(IEvaluationFrameBuilder origin, ICondition? condition = null)
        => new ErrorFrame(
            (action, expression, controller) =>
                NotFoundException.FromController(origin, action, expression, condition, controller), 
            origin);

    IEvaluationFrame IEvaluationFrame.OnPush(IEvaluationFrame.Push onPush) => this;
    IEvaluationFrame IEvaluationFrame.OnPop(Action<IEvaluationFrame.IStackController> onPop) => this;

    public IEvaluationFrame SeekNext(Expression? node)
        => throw NewException(node);

    public void PushTo(IEvaluationFrame.IStackController controller)
        => throw NewException(controller: controller);

    public void PopFrom(IEvaluationFrame.IStackController controller)
        => throw NewException(controller: controller);

    private Exception NewException(Expression? node = null, IEvaluationFrame.IStackController? controller = null, [CallerMemberName] string callerName = "")
        => _buildException(callerName, node, controller);
    
    public class NotFoundException : Exception
    {
        public IEvaluationFrameBuilder OriginFrame { get; }
        public string Action { get; }
        public Expression? Expression { get; }
        public ICondition? Condition { get; }
        public IEvaluationFrame[]? FrameStack { get; }

        private NotFoundException(IEvaluationFrameBuilder origin, string action, Expression? expression, ICondition? condition, IEvaluationFrame[]? stack)
            : base(BuildMessage(origin, action, expression, condition, stack))
        {
            OriginFrame = origin;
            Action = action;
            Expression = expression;
            Condition = condition;
            FrameStack = stack;
        }

        public static NotFoundException FromController(IEvaluationFrameBuilder origin, string action, Expression? expression, ICondition? condition, IEvaluationFrame.IStackController? controller) 
            => new(origin, action, expression, condition, controller != null ? CopyStack(controller).ToArray() : null);

        private static string BuildMessage(IEvaluationFrameBuilder origin, string action, Expression? expression, ICondition? condition, IEvaluationFrame[]? stack)
        {
            var expectation = origin.Describe(expression);
            
            var conditionMessage = condition != null
                ? $"Condition:\n\n{condition.Describe(expression)}\n\n"
                : "";

            var stackMessage = stack != null
                ? $"Stack:\n\n{string.Join('\n', stack.Select(f => f.ToString()))}\n\n"
                : "";

            return
                $"Attempted {action} operation on an ErrorFrame.\n\nExpectation:\n\n{expectation}\n\n{conditionMessage}{stackMessage}";
        }

        private static IEnumerable<IEvaluationFrame> CopyStack(IEvaluationFrame.IStackController controller)
        {
            while (controller.PopFrame() is { } frame)
            {
                yield return frame;
            }
        }
    }
}
