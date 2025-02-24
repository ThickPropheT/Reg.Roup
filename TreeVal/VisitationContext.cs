namespace TreeVal;

public class VisitationContext
{
    public TapeHead Head { get; }

    public bool HasRejection { get; private set; }

    public VisitationContext(TapeHead head)
    {
        Head = head;
    }

    public void Accept(IEvaluatorNode visitor)
    {
        // TODO
        //  is this even useful? I can't think of anything interesting to use this for
        //  maybe just assume accepted unless rejected?
        //  maybe build graph of what was accepted and rejected?
    }

    public void Reject(IEvaluatorNode visitor)
    {
        // TODO start actually tracking what failed
        HasRejection = true;
    }

    public Branch CreateBranch() => new(this, Head.CreateBranch());

    private void Merge(Branch branch)
    {
        // TODO start actually tracking passes/fails
        HasRejection = branch.HasRejection;
    }

    public class Branch : VisitationContext
    {
        private readonly VisitationContext _parent;
        private readonly TapeHead.Branch _head;

        public Branch(VisitationContext parent, TapeHead.Branch head)
            : base(head)
        {
            _parent = parent;
            _head = head;
        }

        public bool TryMerge()
        {
            if (HasRejection)
            {
                return false;
            }

            _head.Merge();
            _parent.Merge(this);
            
            return true;
        }
    }
}
