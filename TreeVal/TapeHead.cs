using System.Linq.Expressions;

namespace TreeVal;

public class TapeHead
{
    private readonly Expression[] _tape;
    private int _currentIndex;

    private int _currentOrFirst => _currentIndex >= 0
        ? _currentIndex
        : 0;
    
    private int _length => _tape.Length;

    public TapeHead(Expression[] tape)
    {
        _tape = tape;
        _currentIndex = -1;
    }

    private TapeHead(Expression[] tape, int currentIndex)
    {
        _tape = tape;
        _currentIndex = currentIndex;
    }

    public Expression Read()
        => _tape[_currentIndex];

    public IEnumerable<Expression> ReadToEnd()
        => _tape.Take(new Range(_currentOrFirst, _tape.Length - 1));

    public bool CanMoveForward()
        => _currentIndex < _tape.Length - 1;

    public Expression MoveForward()
    {
        if (!CanMoveForward())
        {
            throw new IndexOutOfRangeException();
        }

        _currentIndex++;

        return Read();
    }

    public Expression? PeekForward()
        => CanMoveForward()
            ? _tape[_currentIndex + 1]
            : null;

    public bool CanMoveBackward()
        => _currentIndex > 0;

    public Expression MoveBackward()
    {
        if (!CanMoveBackward())
        {
            throw new IndexOutOfRangeException();
        }

        _currentIndex--;

        return Read();
    }

    public Expression? PeekBackward()
        => CanMoveBackward()
            ? _tape[_currentIndex - 1]
            : null;

    // TODO clean this & related stuff up
    public Clip.Builder CreateClip() => new(this);
    private void FastForward(Clip clip) => _currentIndex += clip._currentIndex;

    public class Clip : TapeHead
    {
        private Clip(Expression[] tape)
            : base(tape)
        {
        }

        public static Clip Create(TapeHead current, int fromIndex, int toIndex)
        {
            var source = current._tape.ToArray();
            // var sourceIndex = current._currentIndex;
            // var destinationLength = source.Length - sourceIndex;
            var destinationLength = toIndex + 1 - fromIndex;
            var destination = new Expression[destinationLength];

            Array.Copy(source, fromIndex, destination, 0, destinationLength);
            // Array.Copy(source, sourceIndex, destination, 0, destinationLength);

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
                Last = head._length - 1
            };
    }
}
