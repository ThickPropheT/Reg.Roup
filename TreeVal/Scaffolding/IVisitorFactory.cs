using TreeVal.Eval;

namespace TreeVal.Scaffolding;

public interface IVisitorFactory
{
    IVisitor CreateVisitor();
}