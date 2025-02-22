using Reg.Roup.Conversions;
using Reg.Roup.Expectation;
using Reg.Roup.Expectation._RecycleBin;
using Reg.Roup.Expectation.NewExpression;

namespace Reg.Roup.Tests.Expectation.Scenario;

[TestFixture]
public class RecordedExpressionTree
{
    private class VersionParser
    {
        public static VersionParser Instance { get; } = new();

        public Version Parse(string s)
        {
            throw new NotImplementedException();
        }
    }

    [Test]
    public void PoCo()
    {
        var versionParser = new VersionParser();

        var expressionTree = ExpressionFactory.InitializingType(parse =>
            new
            {
                isEnabled = false,
                index = 0,
                name = "",
                optional = (int?) null,
                version1 = parse.With(VersionParser.Instance.Parse),
                // version2 = parse.With(versionParser.Parse),
                // version3 = parse.With(Version.Parse),
                // version4 = parse.With(s => VersionParser.Instance.Parse(s)),
                // version5 = parse.With(s => versionParser.Parse(s)),
                // version6 = parse.With(s => Version.Parse(s)),
            });

        var evaluator = ExpressionVisitorNodeFactory.Create(node =>
            node.Lambda(
                parameters: [node.Parameter<IParse>()],
                body: node.New()
                    .WithEachChildBeing(
                        @new => @new.GetArgsMappedByParamName(),
                        _ => node.OneOf(
                            node.Constant(),

                            node.Cast(node.Constant(value: null)),

                            node.Debug(e => { }),

                            node
                                .IgnoreBoxing() // TODO i don't think this is working right
                                .MethodCall<IParse>(
                                    name: nameof(IParse.With),
                                    node.OneOf(
                                        node.Debug(e => { }),

                                        node
                                            .IgnoreBoxing()
                                            .MethodCallDelegate(target: node.AcceptChildren),

                                        node.Lambda(
                                            parameters: [node.Parameter<string>()],
                                            body: node.MethodCall(node.AcceptChildren)
                                        )
                                    )
                                )
                        )
                    )
            )
        );
        
        evaluator.Evaluate(expressionTree);
    }
}
