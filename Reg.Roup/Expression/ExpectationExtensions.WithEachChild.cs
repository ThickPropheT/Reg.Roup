using System.Collections.Generic;
using System.Linq;

namespace Reg.Roup.Expression;

public static partial class ExpectationExtensions
{
    public static IExpectation<TNode> WithEachChild<TNode, TChild>(
        this IExpectation<TNode> e,
        IBaseExpectation.Selector<TNode, IEnumerable<TChild>> selectChildren,
        IBaseExpectation.NextChild<TNode, TChild> seek
    )
        where TNode : System.Linq.Expressions.Expression
        where TChild : System.Linq.Expressions.Expression
    {
        e.SetNext((n, options) =>
        {
            var node = (TNode) n;
            var children = selectChildren(node);
            var indexedChildren = children.Select((c, i) => (c, i));
            using var enumerator = indexedChildren.GetEnumerator();

            return options.Each(
                enumerator,
                a => seek(node, a.c, a.i, options)
            );
        });

        return e;
    }
}