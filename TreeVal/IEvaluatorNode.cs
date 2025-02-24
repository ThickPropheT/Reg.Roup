using System.Linq.Expressions;

namespace TreeVal;

public interface IEvaluatorNode
{
    // TODO
    //  does this need to return a nullable expression?
    //  maybe this shouldn't be ITapeHead per se.
    //  maybe something that could offer a more tailored experience to visitor nodes. 
    Expression? Evaluate(IVisitationContext context);
}