using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class TreeRejectedException : Exception
{
    public EvaluationResult[] Trace { get; }

    private TreeRejectedException(EvaluationResult[] trace)
    {
        Trace = trace;
    }

    private TreeRejectedException(string message)
        : base(message)
    {
    }

    private TreeRejectedException(string message, Exception inner) 
        : base(message, inner)
    {
    }

    public static TreeRejectedException ForTrace(EvaluationResult[] trace)
    {
        // TODO figure out exactly what to do with the trace
        // var description = new Description();
        //
        // foreach (var result in trace)
        // {
        //     result.Describe(description);
        // }
        
        return new TreeRejectedException(trace);
    }

    public static TreeRejectedException ForIncompleteRead(EvaluationResult[] trace, TapeHead head)
    {
        // TODO figure out exactly what to do with the trace & head
        // var nodesRead = head.CreateClip()
        //     .From(p => p.First)
        //     .To(p => p.Current);
        //
        // var message = string.Join("\n\n", nodesRead.ReadToEnd());
        
        return new TreeRejectedException(trace);
    }

    // TODO should message be passable-in here?
    public static TreeRejectedException ForError(Exception error)
        => new("TODO", error);
    
    // TODO
    //  it would be cool if you could pass in a custom IDescription
    //  via the API at the ExpressionTreeEvaluator level
    private class Description : IDescription
    {
        public void EmitResult(EvaluatorStatus status, IEvaluatorNode evaluator, ICondition[] failedConditions)
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
