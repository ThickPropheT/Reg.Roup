using System.Runtime.CompilerServices;
using TreeVal.Media;
using TreeVal.Scaffolding.Stage;

namespace TreeVal.Scaffolding;

public interface IVisitorBuilderFactory
{
    IStageDirector StageDirector { get; }

    IVisitorBuilder Where(
        Func<Node, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "");

    IVisitorBuilder<T> OfType<T>();

    IVisitorBuilder OneOf(
        IVisitorFactory option1, IVisitorFactory option2, params IVisitorFactory[] options);
}
