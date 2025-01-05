using Reg.Roup.Conversions;
using Reg.Roup.Expression;
using Reg.Roup.Tests.Resources.TestScenarios;
using Reg.Roup.Tests.Resources.TestScenarios.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Reg.Roup.Tests.Resources.TestScenarios.Default.DefaultSchema;

namespace Reg.Roup.Tests.Visitor
{
    using System.Linq.Expressions;

    [TestFixture]
    public class Tests
    {
        [Test]
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
                //version = parse.With(Version.Parse)
            }
            )
            .Assert((expected, actual) => { });
        }
    }

    public static class TestsExtensions
    {
        public static RegexDeserializationResult<TExpected, Actual> _WhenMatchDeserializedVia<TExpected, Actual>(this GivenTextMatchedByRegex<TExpected> given, Expression<Func<IParse, Actual>> schema)
        {
            //var engine = new DefaultVisitorEngine();

            //new RootNode(engine,
            //                new LambdaNode(engine, l =>
            //                    new OneOfNode(engine,
            //                        new NewNode(engine, e =>
            //                            new ExpectAnyNode(engine)
            //                        ))));

            var v = _ExpectExt
                .NodeType(ExpressionType.Lambda)
                .WithChildren((_, options) =>
                    options.OneOf(
                        options.NodeType<NewExpression>()
                            .Where(n => n.Constructor != null && n.Arguments.Any())
                            .Using(n => (n, ParamNames: n.GetParameterNames().GetEnumerator()))
                            .With((state, options) =>
                                options.Each(
                                    state.ParamNames,
                                    m =>
                                        //options.OneOf(
                                            options
                                                .NodeType<ConstantExpression>()
                                                //.Transform(n => )
                                        //)
                                )
                            ),

                        options.NodeType<MemberInitExpression>()
                    )
                );

            var eng = new VisitorEngine(v);
            var r = eng.Visit(schema);

            throw new NotImplementedException();

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
}
