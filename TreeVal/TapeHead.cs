using System.Linq.Expressions;

namespace TreeVal;

public class TapeHead
{
    private readonly Expression[] _tape;
    private int _currentIndex;

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

    public Clip StartClip() => Clip.StartAt(this);
    private void FastForward(Clip clip) => _currentIndex += clip._currentIndex;

    public class Clip : TapeHead
    {
        private Clip(Expression[] tape)
            : base(tape)
        {
        }

        public static Clip StartAt(TapeHead current)
        {
            var source = current._tape.ToArray();
            var sourceIndex = current._currentIndex;
            var destinationLength = source.Length - sourceIndex;
            var destination = new Expression[destinationLength];

            Array.Copy(source, sourceIndex, destination, 0, destinationLength);

            return new Clip(destination);
        }

        public void SpliceOnto(TapeHead end)
        {
            end.FastForward(this);
        }
    }
}
