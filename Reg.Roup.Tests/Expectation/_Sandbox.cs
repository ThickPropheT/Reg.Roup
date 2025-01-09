

using System.Linq.Expressions;
using Reg.Roup.Conversions;
using Reg.Roup.Expectation;
using Reg.Roup.Expectation.Common.OfType;
using Reg.Roup.Expectation.Common.OneOf;
using Reg.Roup.Expectation.Common.Where;
using Reg.Roup.Expectation.Common.WithChildren;
using Reg.Roup.Expectation.Common.WithEachChild;
using Reg.Roup.Expectation.NewExpression;
using Reg.Roup.Tests._TestResources.Scenarios;
using Reg.Roup.Tests._TestResources.Scenarios.Default;

namespace Reg.Roup.Tests.Expectation;

// TODO keep this around for refrence until the rest of the test suite in this namespace is complete
[TestFixture]
public class _Sandbox
{
    [Test]
    // [Ignore("migrating this into a set of real tests")]
    public void Test()
    {
        //new DefaultScenario()
        //    ._WhenMatchDeserializedVia(parse =>
        //        new DefaultSchema.SchemaClass(
        //            "",
        //            0,
        //            false,
        //            parse.With(Version.Parse)
        //        )
        //    )
        //    .Assert((expected, actual) => { });

        new DefaultScenario()
            ._WhenMatchDeserializedVia(parse =>
                new
                {
                    name = "",
                    index = 0,
                    isEnabled = false,
                    version = parse.With(Version.Parse)
                }
            )
            .Assert((expected, actual) => { });
    }
}

public static class TestsExtensions
{
    public static RegexDeserializationResult<TExpected, Actual> _WhenMatchDeserializedVia<TExpected, Actual>(
        this GivenTextMatchedByRegex<TExpected> given, Expression<Func<IParse, Actual>> schema)
    {
        //var engine = new DefaultVisitorEngine();

        //new RootNode(engine,
        //                new LambdaNode(engine, l =>
        //                    new OneOfNode(engine,
        //                        new NewNode(engine, e =>
        //                            new ExpectAnyNode(engine)
        //                        ))));

        var expectation = ExpectNode
            .OfType(ExpressionType.Lambda)
            // TODO
            //  should there be a 'WithChild'?
            //  - probly so - being strict about child count is probably a good idea. ig lambda will only ever have 1, but in cases with variable children, it might help to be specific
            //  - figure out the logistics of this. WithChildren is just a pass-thru wrapper to SetNext, so it doesn't have any agency over validation logic
            .WithChildren((_, expectNode) =>
                expectNode.OneOf(
                    // TODO add tests combining multiple/nested expectations like this (e.g. 'OneOf' + 'Each')
                    expectNode.OfType<NewExpression>()
                        .Where(n => n.Constructor != null && n.Arguments.Any())
                        .WithEachChild(
                            @new => @new.GetMappedArguments(),
                            (@new, arg, i, expectNode) =>
                                expectNode.OneOf(
                                    expectNode.OfType<ConstantExpression>(),
                                    expectNode
                                        .OfType<MethodCallExpression>()
                                        .Where(call => call.Method.DeclaringType == typeof(IParse))
                                )),
                    // .Using(n => (n, ParamNames: n.GetParameterNames().GetEnumerator()))
                    // .WithChildren((state, options) =>
                    //     options.Each(
                    //         state.ParamNames,
                    //         m =>
                    //             //options.OneOf(
                    //             options
                    //                 .OfType<ConstantExpression>()
                    //         //.Transform(n => )
                    //         //)
                    //     )
                    // ),
                    expectNode.OfType<MemberInitExpression>()
                )
            );

        var eng = new VisitorEngine(expectation);
        var r = eng.Visit(schema);

        throw new NotImplementedException("END OF THE LINE FOOL");

        //var engine = new VisitorEngine(
        //    en => new ExpectNodeType(
        //        ExpressionType.Lambda,
        //        e => new(
        //            new ExpectOneOf(
        //                new ExpectNodeType(
        //                    ExpressionType.New,
        //                    e => e is NewExpression ne && ne.Arguments.Any(),
        //                    e => new(
        //                        new ExpectOneOf(
        //                            new ExpectNodeType(
        //                                ExpressionType.Constant,
        //                                e => SeekResult.TransformWith(new Tx(en, given.Regex.Match(given._GetText()).Groups[e.])
        //                            ),
        //                            new ExpectNodeType(
        //                                ExpressionType.Call,
        //                                e => e is MethodCallExpression mce && mce.Method.DeclaringType == typeof(IParse),
        //                                e => SeekResult.TransformWith(new Tx(en))
        //                            )
        //                        )
        //                        //{ 
        //                        //    Default = new Nop()
        //                        //}
        //                    )
        //                ),
        //                new ExpectNodeType(
        //                    ExpressionType.MemberInit,
        //                    _ => true,
        //                    _ => new(new Nop())
        //                )
        //            )
        //        )
        //    )
        //);

        //return new(given.Expected, given.DeserializeMatch(schema, engine));
    }
}
