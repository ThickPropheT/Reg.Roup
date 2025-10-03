namespace TreeVal.Visit;

public interface IVisitor
{
    string CreatedBy { get; init; }
    
    void Visit(IVisitorContext visitorContext);
}
