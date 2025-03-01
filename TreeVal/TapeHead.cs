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

    public Branch CreateBranch() => new(this);
    private void Merge(Branch branch) => _currentIndex = branch._currentIndex;

    public class Branch : TapeHead
    {
        private readonly TapeHead _parent;

        public Branch(TapeHead parent)
            : base(parent._tape.ToArray(), parent._currentIndex - 1)
        {
            _parent = parent;
        }

        public TapeHead Merge()
        {
            _parent.Merge(this);
            return _parent;
        }
    }
}
