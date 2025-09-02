using TreeVal.Media;

namespace TreeVal;

public partial class VisitationContext
{
    public Clip BranchFromHead() => new(Head, Head.CreateClip().From(p => p.Current).To(p => p.Last));

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

        public void SpliceOnto(VisitationContext end)
        {
            _clip.SpliceOnto(_head);
        }
    }
}
