using System.Linq.Expressions;
using System.Reflection;
using Reg.Roup.Expectation;

namespace Reg.Roup.Tests.Expectation;

[TestFixture]
public class UsingExpectation
{
    private static readonly ConstructorInfo TargetCtor =
        typeof(Target).GetConstructor([typeof(string), typeof(string), typeof(string)])!;

    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Add(Expression.Constant(0), Expression.Constant(0)),
        Expression.New(TargetCtor, Expression.Constant("_s1"), Expression.Constant("_s2"), Expression.Constant("_s3"))
    ];

    private IBaseExpectation _expectation;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _expectation = ExpectNode.OfType<NewExpression>()
            // TODO
            //  - evaluate whether 'Using' is even necessary enymore? see 'Each' tests
            //  - rework 'Using' to require fluent chain to be nested like 'WithChildren'
            //    to allow for proper disposal and address this warning.
            .Using(ctor => new {ctor, paramNames = ctor.GetParameterNames().GetEnumerator()})
            .WithChildren((state, expectNode) =>
                expectNode
                    .OfType<ConstantExpression>()
                    .Where(constant =>
                        state.paramNames.MoveNext()
                        && constant.Value is string s
                        && s == state.paramNames.Current
                    )
            );
    }

    [TestCaseSource(nameof(InvalidExpressions))]
    public void ThrowsOnInvalidSchemas(Expression invalidExpression)
    {
        Assert.That(() => new VisitorEngine(_expectation).Visit(invalidExpression), Throws.Exception);
    }

    [Test]
    public void PassesThroughValidSchemas()
    {
        var expression = Expression.New(TargetCtor, Expression.Constant("s1"), Expression.Constant("s2"),
            Expression.Constant("s3"));

        var validatedExpression = new VisitorEngine(_expectation).Visit(expression);

        Assert.That(validatedExpression, Is.Not.Null);

        var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

        Assert.That(validatedExpression.NodeType, Is.EqualTo(ExpressionType.New));
        Assert.That(expressionResult, Is.EqualTo(new Target("s1", "s2", "s3")));
    }

    private class Target
    {
        public string S1 { get; }
        public string S2 { get; }
        public string S3 { get; }

        public Target(string s1, string s2, string s3)
        {
            S1 = s1;
            S2 = s2;
            S3 = s3;
        }

        public override string ToString()
            => $"{S1}, {S2}, {S3}";

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Target) obj);
        }

        private bool Equals(Target other)
        {
            return string.Equals(S1, other.S1, StringComparison.InvariantCulture) &&
                   string.Equals(S2, other.S2, StringComparison.InvariantCulture) &&
                   string.Equals(S3, other.S3, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(S1, StringComparer.InvariantCulture);
            hashCode.Add(S2, StringComparer.InvariantCulture);
            hashCode.Add(S3, StringComparer.InvariantCulture);
            return hashCode.ToHashCode();
        }
    }
}