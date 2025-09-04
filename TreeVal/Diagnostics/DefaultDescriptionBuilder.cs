using System.Text;
using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Diagnostics;

[Flags]
public enum EmitOptions
{
    Minimal = 0,
    Targets = 1,
    TargetValues = 2,
    AcceptedConditions = 4,
    Verbose = 7
}

public class DefaultDescriptionBuilder : IDescriptionBuilder
{
    private readonly int _indentIncrement;
    private int _indentLevel;

    private bool _doNextIndent = true;

    private readonly StringBuilder _text = new();
    
    public EmitOptions Options { get; set; }

    public DefaultDescriptionBuilder(EmitOptions options = EmitOptions.Verbose, int indentIncrement = 2)
    {
        Options = options;
        _indentIncrement = indentIncrement;
    }

    public void Indented(Action body, int count = 1)
    {
        if (count <= 0)
            count = 1;

        count *= _indentIncrement;

        _indentLevel += count;

        body();

        _indentLevel -= count;
    }

    public void EmitNewline(int count = 1)
    {
        _text.AppendJoin("", Enumerable.Repeat("\n", count));
        _doNextIndent = true;
    }

    private void EmitIndent()
    {
        if (!_doNextIndent)
        {
            _doNextIndent = true;
            return;
        }

        _text.AppendJoin("", Enumerable.Repeat(" ", _indentLevel));
    }

    public void Emit(string text)
    {
        EmitIndent();
        _text.Append(text);
        _doNextIndent = false;
    }

    public void EmitLine(string text)
    {
        EmitIndent();
        _text.AppendLine(text);
        _doNextIndent = true;
    }

    public void EmitOpenBlock(BracketStyle bracketStyle = BracketStyle.Curly)
    {
        _text.AppendLine(Open(bracketStyle));
        _doNextIndent = true;
    }

    public void EmitCloseBlock(BracketStyle bracketStyle = BracketStyle.Curly)
    {
        EmitLine($"{Close(bracketStyle)},");
        _doNextIndent = true;
    }

    public void EmitHeader(string message, BracketStyle bracketStyle = BracketStyle.Curly)
    {
        Emit(message);
        EmitOpenBlock(bracketStyle);
        _doNextIndent = true;
    }

    public void EmitFooter(BracketStyle bracketStyle = BracketStyle.Curly)
    {
        EmitCloseBlock(bracketStyle);
        _doNextIndent = true;
    }

    public void EmitBlock(Action body, BracketStyle bracketStyle = BracketStyle.Curly)
    {
        EmitOpenBlock(bracketStyle);
        Indented(body);
        EmitFooter(bracketStyle);
        _doNextIndent = true;
    }

    public void EmitBlock(string heading, Action body)
    {
        EmitHeader(heading);
        Indented(body);
        EmitFooter();
        _doNextIndent = true;
    }

    public void EmitBlock(string heading, BracketStyle bracketStyle, Action body)
    {
        EmitHeader(heading, bracketStyle);
        Indented(body);
        EmitFooter(bracketStyle);
        _doNextIndent = true;
    }

    public void EmitError(Exception error)
    {
        Emit("Error: ");
        EmitLine($"{error.Message},");
        _doNextIndent = true;
    }

    public void EmitTarget(IDescribable target)
    {
        if (!Options.HasFlag(EmitOptions.Targets))
            return;

        Emit("Target: ");
        target.Describe(this);
    }

    public void EmitNode(Node node)
    {
        EmitBlock(() =>
        {
            EmitLine($"Type: {node.Value.GetType()},");

            if (!Options.HasFlag(EmitOptions.TargetValues))
                return;

            EmitLine($"Value: {node},");
        });
    }

    public void EmitEvaluations(IConditionEvaluation[] evaluations)
    {
        if (!Options.HasFlag(EmitOptions.AcceptedConditions)
            && evaluations.All(e => e.Status == EvaluationStatus.Accepted))
            return;

        EmitBlock(
            "Conditions: ",
            bracketStyle: BracketStyle.Square,
            () =>
            {
                for (var i = 0; i < evaluations.Length; i++)
                {
                    EmitEvaluation(i, evaluations[i]);
                }
            });
    }

    public void EmitEvaluation(int index, IConditionEvaluation evaluation)
    {
        if (!Options.HasFlag(EmitOptions.AcceptedConditions)
            && evaluation.Status == EvaluationStatus.Accepted)
            return;

        Emit($"[{index}]: ");
        evaluation.Describe(this);
    }

    public void EmitAcceptance(ICondition expected)
    {
        if (!Options.HasFlag(EmitOptions.AcceptedConditions))
            return;

        Emit("✅ ");

        // ReSharper disable once SuspiciousTypeConversion.Global
        if (expected is IDescribable d)
        {
            d.Describe(this);
        }
        else
        {
            expected.Describe(this);
        }
    }

    public void EmitRejection(ICondition expected, Node actual, Exception? error)
    {
        EmitBlock(() =>
        {
            EmitLine("Status: ❌,");
            Emit("Expected: ");

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (expected is IDescribable d)
            {
                d.Describe(this);
            }
            else
            {
                expected.Describe(this);
            }

            Emit("Actual: ");
            actual.Describe(this);

            if (error == null)
                return;

            EmitError(error);
        });
    }

    public override string ToString()
        => _text.ToString();

    private static string Open(BracketStyle style)
    {
        switch (style)
        {
            case BracketStyle.Curly:
                return "{";
            case BracketStyle.Square:
                return "[";
            case BracketStyle.Round:
                return "(";
            case BracketStyle.Angle:
                return "<";
            default:
                throw new ArgumentOutOfRangeException(nameof(style), style, null);
        }
    }

    private static string Close(BracketStyle style)
    {
        switch (style)
        {
            case BracketStyle.Curly:
                return "}";
            case BracketStyle.Square:
                return "]";
            case BracketStyle.Round:
                return ")";
            case BracketStyle.Angle:
                return ">";
            default:
                throw new ArgumentOutOfRangeException(nameof(style), style, null);
        }
    }
}
