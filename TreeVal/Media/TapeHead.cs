namespace TreeVal.Media;

public class TapeHead
{
    private readonly Node[] _tape;
    private int _currentIndex;

    private int CurrentOrFirstIndex => _currentIndex >= 0
        ? _currentIndex
        : 0;

    private int Length => _tape.Length;

    public TapeHead(Node[] tape)
    {
        _tape = tape;
        _currentIndex = -1;
    }

    private TapeHead(Node[] tape, int currentIndex)
    {
        _tape = tape;
        _currentIndex = currentIndex;
    }

    public Node Read()
        => _tape[_currentIndex];

    public IEnumerable<Node> ReadToEnd()
        => _tape.Take(new Range(CurrentOrFirstIndex, _tape.Length - 1));

    public bool CanMoveForward()
        => _currentIndex < _tape.Length - 1;

    public Node MoveForward()
    {
        if (!CanMoveForward())
        {
            throw new IndexOutOfRangeException();
        }

        _currentIndex++;

        return Read();
    }

    public Node? PeekForward()
        => CanMoveForward()
            ? _tape[_currentIndex + 1]
            : null;

    public bool CanMoveBackward()
        => _currentIndex > 0;

    public Node MoveBackward()
    {
        if (!CanMoveBackward())
        {
            throw new IndexOutOfRangeException();
        }

        _currentIndex--;

        return Read();
    }

    public Node? PeekBackward()
        => CanMoveBackward()
            ? _tape[_currentIndex - 1]
            : null;

    // TODO clean this & related stuff up
    public Clip.Builder CreateClip() => new(this);
    private void FastForward(Clip clip) => _currentIndex += clip._currentIndex;

    public class Clip : TapeHead
    {
        private Clip(Node[] tape)
            : base(tape)
        {
        }

        public static Clip Create(TapeHead current, int fromIndex, int toIndex)
        {
            var source = current._tape.ToArray();
            var destinationLength = toIndex + 1 - fromIndex;
            var destination = new Node[destinationLength];

            Array.Copy(source, fromIndex, destination, 0, destinationLength);

            return new Clip(destination);
        }

        public void SpliceOnto(TapeHead end)
        {
            end.FastForward(this);
        }

        public class Builder
        {
            private readonly TapeHead _current;

            private int _from;

            public Builder(TapeHead current)
            {
                _current = current;
            }

            public Builder From(Func<Positions, int> getIndex)
            {
                _from = getIndex(Positions.Of(_current));
                return this;
            }

            public Builder From(int index)
            {
                _from = index;
                return this;
            }

            public Clip To(Func<Positions, int> getIndex)
                => Create(_current, _from, getIndex(Positions.Of(_current)));

            public Clip To(int index)
                => Create(_current, _from, index);
        }
    }

    public struct Positions
    {
        public int First { get; init; }
        public int Current { get; init; }
        public int Last { get; init; }

        public static Positions Of(TapeHead head)
            => new()
            {
                First = 0,
                Current = head._currentIndex,
                Last = head.Length - 1
            };
    }
}
