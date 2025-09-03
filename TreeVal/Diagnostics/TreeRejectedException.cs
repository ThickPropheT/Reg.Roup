using System.Text;
using TreeVal.Eval;
using TreeVal.Media;

namespace TreeVal.Diagnostics;

public class TreeRejectedException : Exception
{
    public TapeHead? Head { get; init; }
    public Evaluation Evaluation { get; }

    private TreeRejectedException(Evaluation evaluation, string message)
        : base(message)
    {
        Evaluation = evaluation;
    }

    private TreeRejectedException(Evaluation evaluation, string message, Exception inner)
        : base(message, inner)
    {
        Evaluation = evaluation;
    }

    public static TreeRejectedException ForRejection(Evaluation evaluation)
        => new(evaluation, "An evaluator rejected the source tree");

    public static TreeRejectedException ForIncompleteRead(TapeHead head, Evaluation evaluation)
        => new(evaluation, "Head contains unread nodes")
        {
            Head = head
        };
    
    public static TreeRejectedException ForError(TapeHead head, Evaluation evaluation, Exception error)
        => new(evaluation, "An unexpected error occurred while evaluating the source tree", error)
        {
            Head = head
        };

    public static TreeRejectedException Rethrow(
        TreeRejectedException error, IDescriptionBuilder? descriptionBuilder = null)
    {
        descriptionBuilder ??= new DefaultDescriptionBuilder();
        
        descriptionBuilder.EmitNewline(count: 2);

        descriptionBuilder.Indented(
            () => error.Evaluation.Describe(descriptionBuilder),
            count: 2);

        return new TreeRejectedException(
            error.Evaluation,
            descriptionBuilder.ToString(),
            error
        )
        {
            Head = error.Head
        };
    }

    public class DefaultDescriptionBuilder : IDescriptionBuilder
    {
        private readonly int _indentIncrement;
        private int _indentLevel;

        private readonly StringBuilder _text = new();

        public DefaultDescriptionBuilder(int indentIncrement = 2)
        {
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
        }

        private void EmitIndent()
        {
            _text.AppendJoin("", Enumerable.Repeat(" ", _indentLevel));
        }

        public void Emit(string text)
        {
            EmitIndent();
            _text.Append(text);
        }

        public void EmitLine(string text)
        {
            EmitIndent();
            _text.AppendLine(text);
        }

        public void EmitOpenBlock()
        {
            _text.AppendLine(" {");
        }

        public void EmitCloseBlock()
        {
            EmitLine("},");
        }

        public void EmitHeader(string message)
        {
            Emit(message);
            EmitOpenBlock();
        }

        public void EmitFooter()
        {
            EmitCloseBlock();
        }

        public void EmitBlock(Action body)
        {
            EmitOpenBlock();
            Indented(body);
            EmitFooter();
        }

        public void EmitBlock(string heading, Action body)
        {
            EmitHeader(heading);
            Indented(body);
            EmitFooter();
        }
        
        public void EmitError(Exception error)
        {
            EmitBlock("Error:", () => EmitLine(error.ToString()));
        }

        public override string ToString()
            => _text.ToString();
    }
}
