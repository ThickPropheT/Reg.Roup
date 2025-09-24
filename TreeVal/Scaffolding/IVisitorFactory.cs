using TreeVal.Eval;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public interface IVisitorFactory
{
    IVisitor CreateVisitor(Node node);
}