namespace TreeVal;

public partial class VisitationContext
{
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
