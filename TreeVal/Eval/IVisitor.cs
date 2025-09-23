using TreeVal.Media;

namespace TreeVal.Eval;

public interface IVisitor
{
    void Visit(TapeHead head);
}
