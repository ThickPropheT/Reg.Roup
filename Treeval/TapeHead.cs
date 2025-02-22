using System.Linq.Expressions;

namespace Treeval;

public interface ICheckpoint
{
    ITapeHead Head { get; }
    
    void Commit();
}

public interface ITapeHead
{
    Expression? Read();

    bool CanMoveForward();
    Expression? MoveForward();
    
    bool CanMoveBackward();
    Expression? MoveBackward();

    ICheckpoint Checkpoint();
}

public class TapeHead : ITapeHead
{
    private readonly Expression?[] _tape;
    private int _currentIndex;

    public TapeHead(Expression?[] tape)
    {
        _tape = tape;
        _currentIndex = -1;
    }

    private TapeHead(Expression?[] tape, int currentIndex)
    {
        _tape = tape;
        _currentIndex = currentIndex;
    }

    public Expression? Read()
        => _tape[_currentIndex];

    public bool CanMoveForward()
        => _currentIndex < _tape.Length - 1;

    public Expression? MoveForward()
    {
        if (!CanMoveForward())
        {
            throw new IndexOutOfRangeException();
        }
        
        _currentIndex++;
        
        return Read();
    }

    public bool CanMoveBackward()
        => _currentIndex > 0;

    public Expression? MoveBackward()
    {
        if (!CanMoveBackward())
        {
            throw new IndexOutOfRangeException();
        }
        
        _currentIndex--;

        return Read();
    }

    public ICheckpoint Checkpoint() 
        => new TapeHeadCheckpoint(this, new TapeHead(_tape, _currentIndex));

    private void CopyFrom(TapeHead other)
    {
        _currentIndex = other._currentIndex;
        // TODO may have a problem if tape ever becomes read/write instead of read-only
    }

    private class TapeHeadCheckpoint : ICheckpoint
    {
        private readonly TapeHead _original;
        private readonly TapeHead _copy;

        public ITapeHead Head => _copy;
        
        public TapeHeadCheckpoint(TapeHead original, TapeHead copy)
        {
            _original = original;
            _copy = copy;
        }

        public void Commit()
        {
            _original.CopyFrom(_copy);
        }
    }
}
