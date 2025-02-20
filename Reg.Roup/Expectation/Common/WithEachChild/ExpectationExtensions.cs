using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation.Common.WithEachChild;

public static class ExpectationExtensions
{
    // TODO
    //  consider adding other overloads for less specific node types
    //  and for other parameter configurations for 'seek'
    public static IExpectation<TNode> WithEachChild<TNode, TChild>(
        this IExpectation<TNode> e, IBaseExpectation.Selector<TNode, IEnumerable<TChild>> selectChildren, IBaseExpectation.NextChild<TNode, TChild> seek
    )
        where TNode : Expression
    {
        e.SetNext((n, expectNode) =>
        {
            var node = (TNode) n;
            var indexedChildren = selectChildren(node).Select((c, i) => (c, i));

            return new ExpectEach<(TChild c, int i)>(
                indexedChildren,
                a => seek(node, a.c, a.i, expectNode)
            );
        });

        return e;
    }
}
