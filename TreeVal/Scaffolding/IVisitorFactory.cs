using TreeVal.Media;
using TreeVal.Visit;

namespace TreeVal.Scaffolding;

public interface IVisitorFactory
{
    IVisitor CreateVisitor(Node node);
}
