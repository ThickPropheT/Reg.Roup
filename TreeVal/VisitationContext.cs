using TreeVal.Media;

namespace TreeVal;

public partial class VisitationContext
{
    // TODO
    //  doesn't seem super useful to keep this partial given all it's got is Head,
    //  but where else would you put Head?
    public TapeHead Head { get; }

    public VisitationContext(TapeHead head)
    {
        Head = head;
    }
}
