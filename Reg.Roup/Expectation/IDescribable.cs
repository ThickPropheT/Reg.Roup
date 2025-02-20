using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public interface IDescribable
{
    // TODO
    //  - stop returning string
    //  - add `DescriptionContext` param w/ members:
    //    - Emit(string header, string? value = null)
    //    - Visit(IDescribable child)
    //    - Visit(IEnumerable<IDescribable> children)
    string Describe(Expression? node);
}
