using TreeVal.Media;

namespace TreeVal.Eval;

public abstract partial class MediaBehavior
{
    public class SkipWhile : IAfterEnteringBehavior
    {
        private readonly Func<Node, bool> _predicate;

        public SkipWhile(Func<Node, bool> predicate)
        {
            _predicate = predicate;
        }

        public IStageContext Perform(IStageContext current)
        {
            var tapeHead = current.TapeHead;

            Node node;

            do
            {
                node = tapeHead.MoveForward();
            } while (_predicate(node));

            return new StageContext(tapeHead);
        }
    }
}
