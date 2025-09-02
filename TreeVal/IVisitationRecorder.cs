using TreeVal.Condition;

namespace TreeVal;

public interface IVisitationRecorder
{
    IEnumerable<Node> RecordVisitationOf(Node node);
}

public interface IVisitationRecorder<T>
{
    IEnumerable<Node<T>> RecordVisitationOf(Node<T> node);
}
