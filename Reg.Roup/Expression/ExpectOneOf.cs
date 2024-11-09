using System.Linq;

namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public class ExpectOneOf : IExpectationEvaluator
    {
        private readonly IExpectationEvaluator[] options;

        public ExpectOneOf(IExpectationEvaluator[] options)
        {
            this.options = options;
        }

        public EvaluationResult Evaluate(Expression? node)
        {
            var match = options
                .Select(o => o.Evaluate(node))
                .FirstOrDefault(r => r.IsMatch);

            if (match == null)
            {
                return EvaluationResult.FailWith(new System.Exception());
            }

            // TODO this needs to push itself AND the match somehow
            return new EvaluationResult(
                true,
                () => new SeekResult(
                    match.SeekNext().Next,
                    pop => {
                        pop();
                        //while (pop() != this)
                        //{

                        //}
                    }),
                match.FindTransformer
            );
        }
    }
}