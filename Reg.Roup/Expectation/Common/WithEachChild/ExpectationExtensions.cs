using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Reg.Roup.Expectation.Common.Each;

namespace Reg.Roup.Expectation.Common.WithEachChild;

public static class ExpectationExtensions
{
    // TODO
    //  consider adding other overloads for less specific node types
    //  and for other parameter configurations for 'seek'
    public static IExpectation<TNode> WithEachChild<TNode, TChild>(
        this IExpectation<TNode> e, IBaseExpectation.Selector<TNode, IEnumerable<TChild>> selectChildren,
        IBaseExpectation.NextChild<TNode, TChild> seek
    )
        where TNode : Expression
        where TChild : Expression
    {
        e.SetNext((n, expectNode) =>
        {
            var node = (TNode) n;
            var children = selectChildren(node);
            var indexedChildren = children.Select((c, i) => (c, i));
            using var enumerator = indexedChildren.GetEnumerator();

            return expectNode.Each(
                enumerator,
                a => seek(node, a.c, a.i, expectNode)
            );
        });

        return e;
    }
}
