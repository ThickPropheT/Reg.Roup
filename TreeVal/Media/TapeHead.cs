using TreeVal.Diagnostics;

namespace TreeVal.Media;

public interface ITapeHead : IDescribable
{
    bool CanMoveForward { get; }
    
    Node Read();
    IEnumerable<Node> ReadToStart();

    Node MoveForward();
    Node? PeekForward();
}

public class TapeHead : ITapeHead, IDescribable
{
    private readonly Node[] _tape;
    private int _currentIndex;

    public int CurrentOrFirstIndex => _currentIndex >= 0
        ? _currentIndex
        : 0;

    private int Length => _tape.Length;

    public bool IsBeforeFront => _currentIndex < 0;
    public bool IsAtFront => _currentIndex == 0;
    public bool IsAtBack => _currentIndex == _tape.Length - 1;
    public bool IsAfterBack => _currentIndex >= _tape.Length;
    
    public bool CanRead => 0 <= _currentIndex && _currentIndex <= _tape.Length - 1;
    
    public bool CanMoveForward
        => _currentIndex < _tape.Length - 1;
    
    public bool CanMoveBackward
        => _currentIndex > 0;

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

    public IEnumerable<Node> ReadToStart()
        => _tape.Take(new Range(0, CurrentOrFirstIndex + 1)).Reverse();

    public IEnumerable<Node> ReadToEnd()
        => _tape.Take(new Range(CurrentOrFirstIndex, _tape.Length - 1));

    public Node MoveForward()
    {
        if (!CanMoveForward)
        {
            throw new IndexOutOfRangeException();
        }

        _currentIndex++;

        return Read();
    }

    public Node? PeekForward()
        => CanMoveForward
            ? _tape[_currentIndex + 1]
            : null;

    public Node MoveBackward()
    {
        if (!CanMoveBackward)
        {
            throw new IndexOutOfRangeException();
        }

        _currentIndex--;

        return Read();
    }

    public Node? PeekBackward()
        => CanMoveBackward
            ? _tape[_currentIndex - 1]
            : null;

    private void FastForward(Clip clip) => _currentIndex += clip._currentIndex;

    public virtual void Describe(IDescriptionBuilder descriptionBuilder)
    {
        descriptionBuilder.EmitBlock(
            "Head: ",
            () =>
            {
                descriptionBuilder.EmitLine($"CurrentIndex: {_currentIndex},");
                descriptionBuilder.Emit("Current: ");
                descriptionBuilder.EmitNode(Read());

                descriptionBuilder.Emit("Tape: ");
                descriptionBuilder.EmitArray(
                    _tape,
                    (node, i) =>
                    {
                        if (i < _currentIndex)
                        {
                            descriptionBuilder.EmitPassIcon();
                        }
                        else if (i == _currentIndex)
                        {
                            descriptionBuilder.EmitFailIcon();
                        }

                        descriptionBuilder.EmitNode(node, NodeStyle.ArrayItem);
                    });
            });

        descriptionBuilder.Emit("],");
    }


    // TODO clean this & related stuff up
    public Clip.Builder CreateClip() => new(this);

    public class Clip : TapeHead
    {
        private readonly TapeHead _original;

        private Clip(TapeHead original, Node[] tape)
            : base(tape)
        {
            _original = original;
        }

        public static Clip Create(TapeHead current, int fromIndex, int toIndex)
        {
            var source = current._tape.ToArray();
            var destinationLength = toIndex + 1 - fromIndex;
            var destination = new Node[destinationLength];

            Array.Copy(source, fromIndex, destination, 0, destinationLength);

            return new Clip(current, destination);
        }

        public void SpliceOnto(TapeHead end)
        {
            end.FastForward(this);
        }

        public override void Describe(IDescriptionBuilder descriptionBuilder)
        {
            descriptionBuilder.EmitBlock(
                "Clip: ",
                () =>
                {
                    descriptionBuilder.Emit("Original: ");
                    _original.Describe(descriptionBuilder);

                    descriptionBuilder.EmitLine($"CurrentIndex: {_currentIndex},");
                    descriptionBuilder.Emit("Current: ");
                    descriptionBuilder.EmitNode(Read());

                    descriptionBuilder.Emit("Tape: ");
                    descriptionBuilder.EmitArray(
                        _tape,
                        (node, i) =>
                        {
                            if (i < _currentIndex)
                            {
                                descriptionBuilder.EmitPassIcon();
                            }
                            else if (i == _currentIndex)
                            {
                                descriptionBuilder.EmitFailIcon();
                            }

                            descriptionBuilder.EmitNode(node, NodeStyle.ArrayItem);
                        });
                });

            // TODO why was this here before? it's now generating extra an ],
            // descriptionBuilder.Emit("],");
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
