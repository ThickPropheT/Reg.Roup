using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class TreeRejectedException : Exception
{
    public TapeHead? Head { get; init; }

    private TreeRejectedException(string message)
        : base(message)
    {
    }

    private TreeRejectedException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public static TreeRejectedException ForTrace()
    {
        return new TreeRejectedException("Something was rejected.");
    }

    public static TreeRejectedException ForIncompleteRead(TapeHead head)
    {
        return new TreeRejectedException("Didn't finish reading head.")
        {
            Head = head
        };
    }

    // TODO should message be passable-in here?
    public static TreeRejectedException ForError(TapeHead head, Exception error)
        => new("Something exploded unexpectedly.", error)
        {
            Head = head
        };

    // TODO
    //  it would be cool if you could pass in a custom IDescription
    //  via the API at the ExpressionTreeEvaluator level
    private class Description : IDescription
    {
        public void EmitResult(Evaluation.Status status, IEvaluatorNode evaluator, ICondition[] failedConditions)
        {
            throw new NotImplementedException();
        }

        public void EmitNodeTypeCondition(ExpressionType nodeType)
        {
            throw new NotImplementedException();
        }

        public void EmitNodeTypeCondition(Type type, ExpressionType? nodeType = null)
        {
            throw new NotImplementedException();
        }

        public void EmitWhereCondition(string message)
        {
            throw new NotImplementedException();
        }
    }
}
