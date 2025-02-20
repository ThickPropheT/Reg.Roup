using System.Linq.Expressions;
using Reg.Roup.Conversions;
using Reg.Roup.Expectation;
using Reg.Roup.Expectation.Common.OfType;
using Reg.Roup.Expectation.Common.OneOf;
using Reg.Roup.Expectation.Common.Where;
using Reg.Roup.Expectation.Common.WithChildren;
using Reg.Roup.Expectation.Common.WithEachChild;
using Reg.Roup.Expectation.NewExpression;

namespace Reg.Roup.Tests.Expectation.Scenario;

[TestFixture]
public class ObjectInitialization
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Add(Expression.Constant(0), Expression.Constant(1)),
        ExpressionFactory.InitializingType(parse =>
            new
            {
                version = parse.With(Version.Parse),
                optional = (int?) null,
                name = "",
                index = 0,
                isEnabled = false,
            })
    ];
    
    private static readonly Expression[] ValidExpressions =
    [
        ExpressionFactory.InitializingType(parse =>
            new
            {
                // TODO including version, optional, or more than one constant breaks the test 
                version = parse.With(Version.Parse),
                optional = (int?) null,
                name = "",
                index = 0,
                isEnabled = false, // TODO any single constant passes
            })
    ];
    
    private IExpectation<Expression> _expectation;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _expectation = ExpectNode
            .OfType(ExpressionType.Lambda)
            .WithChildren((_, expectNode) =>
                expectNode
                    .OneOf(
                        expectNode
                            .OfType<NewExpression>()
                            .Where(n => n.Constructor != null && n.Arguments.Any())
                            .WithEachChild(
                                node => node.GetArgsMappedByParamName(),
                                (node, arg, i, _) =>
                                    expectNode
                                        .OneOf(
                                            expectNode
                                                .OfType<ConstantExpression>(),
                                            expectNode
                                                .OfType<MethodCallExpression>()
                                                .Where(call => call.Method.DeclaringType == typeof(IParse))
                                        )
                            )
                    )
            );
    }

    [TestCaseSource(nameof(InvalidExpressions))]
    public void ThrowsOnInvalidSchemas(Expression invalidExpression)
    {
        Assert.That(() => new VisitorEngine(_expectation).Visit(invalidExpression), Throws.Exception);
    }
    
    [TestCaseSource(nameof(ValidExpressions))]
    public void PassesThroughValidSchemas(Expression validExpression)
    {
        Expression? validatedExpression = null;
        Assert.DoesNotThrow(() => validatedExpression = new VisitorEngine(_expectation).Visit(validExpression));
        Assert.That(validatedExpression, Is.Not.Null);
    }
}
