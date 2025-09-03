namespace TreeVal.Eval;


public class Evaluation
{
    private Status? _status;
    public Status CurrentStatus => _status ?? Status.Accepted;

    public void Reject()
    {
        System.Diagnostics.Debug.Assert(_status == null, $"Should mutable status be allowed? {CurrentStatus} -> Rejected");
        _status = Status.Rejected;
    }

    public enum Status
    {
        Accepted,
        Rejected
    }
}
