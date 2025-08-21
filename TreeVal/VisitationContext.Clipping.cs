namespace TreeVal;

public partial class VisitationContext
{
    public Clip BranchFromHead() => new(Head, Head.CreateClip().From(p => p.Current).To(p => p.Last));

    private void FastForward(Clip clip)
    {
        FastForwardStatuses(clip);
    }

    public class Clip : VisitationContext
    {
        private readonly TapeHead _head;
        private readonly TapeHead.Clip _clip;

        public Clip(TapeHead head, TapeHead.Clip clip)
            : base(clip)
        {
            _head = head;
            _clip = clip;
        }

        public bool TrySpliceOnto(VisitationContext end)
        {
            if (IsAnyResultRejected)
            {
                return false;
            }

            _clip.SpliceOnto(_head);
            end.FastForward(this);

            return true;
        }
    }
}
