using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Diagnostics;

public enum BracketStyle
{
    Curly = 0,
    Square,
    Round,
    Angle
}

public interface IDescriptionBuilder
{
    EmitOptions Options { get; set; }

    void Indented(Action body, int count = 1);

    void EmitNewline(int count = 1);

    void Emit(string text);
    void EmitLine(string text);

    void EmitOpenBlock(BracketStyle bracketStyle = BracketStyle.Curly);
    void EmitCloseBlock(BracketStyle bracketStyle = BracketStyle.Curly);

    void EmitHeader(string message, BracketStyle bracketStyle = BracketStyle.Curly);
    void EmitFooter(BracketStyle bracketStyle = BracketStyle.Curly);

    void EmitBlock(Action body);
    void EmitBlock(string heading, Action body);
    void EmitBlock(BracketStyle bracketStyle, Action body);
    void EmitBlock(string heading, BracketStyle bracketStyle, Action body);

    void EmitArray<T>(T[] array, Action<T, int> callback);

    void EmitError(Exception error);
    void EmitTarget(Node node);
    void EmitNode(Node node, NodeStyle style = NodeStyle.PropertyValue);

    void EmitEvaluations(IConditionEvaluation[] evaluations);
    void EmitEvaluation(int index, IConditionEvaluation evaluation);

    void EmitPassIcon(string suffix = " ");
    void EmitFailIcon(string suffix = " ");
    void EmitAcceptance(ICondition expected);
    void EmitRejection(ICondition expected, Node actual, Exception? error);

    void EmitTreeRejection(TreeRejectedException error);

    string ToString();
}
