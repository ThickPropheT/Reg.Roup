using System.Text;
using TreeVal.Media;
using TreeVal.Stage.Eval;
using TreeVal.Visit;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Diagnostics;

[Flags]
public enum EmitOptions
{
    Minimal = 0,
    Targets = 1,
    TargetValues = 2,
    AcceptedStages = 4,
    AcceptedBehaviors = 8,
    Verbose = 15
}

public enum NodeStyle
{
    PropertyValue = 0,
    ArrayItem
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

    public void EmitBlock(Action body)
    {
        EmitOpenBlock();
        Indented(body);
        EmitFooter();
        _doNextIndent = true;
    }

    public void EmitBlock(string heading, Action body)
    {
        EmitHeader(heading);
        Indented(body);
        EmitFooter();
        _doNextIndent = true;
    }

    public void EmitBlock(BracketStyle bracketStyle, Action body)
    {
        EmitOpenBlock(bracketStyle);
        Indented(body);
        EmitFooter(bracketStyle);
        _doNextIndent = true;
    }

    public void EmitBlock(string heading, BracketStyle bracketStyle, Action body)
    {
        EmitHeader(heading, bracketStyle);
        Indented(body);
        EmitFooter(bracketStyle);
        _doNextIndent = true;
    }

    public void EmitArray<T>(T[] array, Action<T, int> callback)
    {
        EmitBlock(
            bracketStyle: BracketStyle.Square,
            () =>
            {
                for (var i = 0; i < array.Length; i++)
                {
                    var childEvaluation = array[i];

                    Emit($"[{i}]: ");
                    callback(childEvaluation, i);
                }
            });
    }

    public void EmitError(VisitationException error)
    {
        EmitBlock(() =>
        {
            Emit("Message: ");
            EmitLine($"'{error.Message}', ");

            error.Head.Describe(this);

            Emit("EvaluationTree: ");
            EmitVisitorContext(error.VisitorContext);
        });
    }

    public void EmitError(Exception error)
    {
        Emit("Error: ");
        EmitLine($"{error.Message},");
        _doNextIndent = true;
    }

    public void EmitTarget(Node target)
    {
        if (!Options.HasFlag(EmitOptions.Targets))
            return;

        Emit("Target: ");
        EmitNode(target);
    }

    public void EmitNode(Node node, NodeStyle style = NodeStyle.PropertyValue)
    {
        if (style == NodeStyle.ArrayItem)
        {
            EmitIndent();
        }

        EmitBlock(() =>
        {
            EmitLine($"Type: {node.Value.GetType()},");

            if (!Options.HasFlag(EmitOptions.TargetValues))
                return;

            EmitLine($"Value: {node.Value},");
        });
    }

    public void EmitVisitorContext(IVisitorContext visitorContext)
    {
        if (TryDescribe(visitorContext))
            return;

        EmitBlock(() =>
        {
            var results = visitorContext.StageVisitations.ToArray();

            if (results.Any())
            {
                EmitStageResults(results);
            }
        });
    }

    private void EmitStageResults(StageVisitationResult[] results)
    {
        if (!Options.HasFlag(EmitOptions.AcceptedStages)
            && results.All(e => e.Status == EvaluationStatus.Accepted))
            return;

        EmitBlock(
            "Stages: ",
            bracketStyle: BracketStyle.Square,
            () =>
            {
                for (var i = 0; i < results.Length; i++)
                {
                    EmitStageResult(i, results[i]);
                }
            });
    }

    private void EmitStageResult(int index, StageVisitationResult result)
    {
        if (!Options.HasFlag(EmitOptions.AcceptedStages)
            && result.Status == EvaluationStatus.Accepted)
            return;

        Emit($"[{index}]: ");

        if (result.Status == EvaluationStatus.Accepted)
        {
            EmitAcceptedStageResult(result);
        }
        else if (result.Status == EvaluationStatus.Rejected)
        {
            EmitRejectedStageResult(result, result.TapeHead.Read(), result.Error);
        }
    }

    private void EmitAcceptedStageResult(StageVisitationResult expected)
    {
        if (!Options.HasFlag(EmitOptions.AcceptedStages))
            return;

        EmitPassIcon();
        EmitStageResult(expected);
    }

    public void EmitRejectedStageResult(StageVisitationResult expected, Node actual, Exception? error)
    {
        EmitBlock(() =>
        {
            Emit("Status: ");
            EmitFailIcon(suffix: "");
            EmitLine(",");
            Emit("Expected: ");
            EmitStageResult(expected);

            Emit("Actual: ");
            EmitNode(actual);

            if (error == null)
                return;

            EmitError(error);
        });
    }

    private void EmitStageResult(StageVisitationResult result)
    {
        if (TryDescribe(result))
            return;

        EmitBlock(
            $"{result.Stage.CreatedBy} ",
            () =>
            {
                if (result.Message != null)
                {
                    EmitLine($"Message: {result.Message},");
                }

                EmitBehaviorResults(result.BehaviorVisitations.ToArray());
            });
    }

    private void EmitBehaviorResults(BehaviorVisitationResult[] results)
    {
        if (!Options.HasFlag(EmitOptions.AcceptedBehaviors)
            && results.All(r => r.Status == EvaluationStatus.Accepted))
            return;

        EmitBlock(
            "BehaviorResults: ",
            bracketStyle: BracketStyle.Square,
            () =>
            {
                for (var i = 0; i < results.Length; i++)
                {
                    EmitBehaviorResult(i, results[i]);
                }
            });
    }

    private void EmitBehaviorResult(int index, BehaviorVisitationResult result)
    {
        Emit($"[{index}]: ");

        if (result.Status == EvaluationStatus.Accepted)
        {
            EmitAcceptedBehaviorResult(result);
        }
        else if (result.Status == EvaluationStatus.Rejected)
        {
            EmitRejectedBehaviorResult(result, result.TapeHead.Read(), result.Error);
        }
    }

    private void EmitAcceptedBehaviorResult(BehaviorVisitationResult expected)
    {
        if (!Options.HasFlag(EmitOptions.AcceptedStages))
            return;

        EmitPassIcon();
        EmitBehaviorResult(expected);
    }

    public void EmitRejectedBehaviorResult(BehaviorVisitationResult expected, Node actual, Exception? error)
    {
        EmitBlock(() =>
        {
            Emit("Status: ");
            EmitFailIcon(suffix: "");
            EmitLine(",");
            Emit("Expected: ");
            EmitBehaviorResult(expected);

            Emit("Actual: ");
            EmitNode(actual);

            if (error == null)
                return;

            EmitError(error);
        });
    }

    private void EmitBehaviorResult(BehaviorVisitationResult result)
    {
        if (!Options.HasFlag(EmitOptions.AcceptedBehaviors))
            return;

        if (TryDescribe(result))
            return;

        if (!TryDescribe(result.VisitationResult))
        {
            EmitLine($"{result.VisitationResult},");
        }
    }

    public void EmitPassIcon(string suffix = " ")
    {
        Emit($"✅{suffix}");
    }

    public void EmitFailIcon(string suffix = " ")
    {
        Emit($"❌{suffix}");
    }

    private bool TryDescribe(object candidate)
    {
        if (candidate is not IDescribable describable)
            return false;

        describable.Describe(this);
        return true;
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
