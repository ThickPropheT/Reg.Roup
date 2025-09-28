// using System.Diagnostics;
// using TreeVal.Diagnostics;
// using TreeVal.Media;
// using TreeVal.Scaffolding;
// using TreeVal.Stage.Eval;
// using TreeVal.Visit;
//
// namespace TreeVal.Eval;
//
// [DebuggerDisplay("{Status}")]
// public class DefaultNodeEvaluation : INodeEvaluation
// {
//     private readonly IVisitorFactory _evaluatorFactory;
//
//     private EvaluationStatus? _status;
//
//     public INodeEvaluation? Parent { get; }
//
//     public Node? Target { get; private set; }
//     public IVisitor? Evaluator { get; private set; }
//
//     public EvaluationStatus Status => _status ?? EvaluationStatus.Accepted;
//
//     public IConditionEvaluation[] ConditionEvaluations { get; private set; } = [];
//     public IEnumerable<INodeEvaluation> ChildEvaluations { get; private set; } = [];
//
//     public DefaultNodeEvaluation(INodeEvaluation? parent, IVisitorFactory evaluatorFactory)
//     {
//         Parent = parent;
//         _evaluatorFactory = evaluatorFactory;
//     }
//
//     public IVisitor GetEvaluator(Node node)
//         => Evaluator ??= _evaluatorFactory.CreateVisitor(node);
//
//     public Node? GetTarget(VisitationContext context)
//     {
//         if (Target != null)
//             return Target;
//
//         try
//         {
//             // var moveHead = evaluator.HeadMovementStrategy.GetStrategy(context, this);
//             // var target = moveHead(context.Head);
//             // var evaluator = GetEvaluator(target);
//             // return Target = target;
//         }
//         catch (IndexOutOfRangeException)
//         {
//             // throw TreeRejectedException.ForReadPastEnd(context.Head, this);
//         }
//
//         throw new NotImplementedException();
//     }
//
//     public void Record(IEnumerable<IConditionEvaluation> conditionEvaluations)
//     {
//         var evaluations = conditionEvaluations.ToArray();
//
//         ConditionEvaluations = evaluations;
//
//         if (evaluations.All(e => e.Status == EvaluationStatus.Accepted))
//             return;
//
//         Reject();
//     }
//
//     public void Record(IEnumerable<INodeEvaluation> childEvaluations)
//         => ChildEvaluations = childEvaluations;
//
//     public void Reject()
//     {
//         System.Diagnostics.Debug.Assert(
//             _status == null,
//             $"Should mutable status be allowed? {Status} -> Rejected");
//
//         _status = EvaluationStatus.Rejected;
//     }
//
//     public void Describe(IDescriptionBuilder descriptionBuilder)
//     {
//         descriptionBuilder.EmitBlock(() =>
//         {
//             if (Target != null)
//             {
//                 descriptionBuilder.EmitTarget(Target);
//             }
//
//             if (ConditionEvaluations.Any())
//             {
//                 descriptionBuilder.EmitEvaluations(ConditionEvaluations);
//             }
//
//             var childEvaluations = ChildEvaluations.ToArray();
//
//             if (childEvaluations.Any())
//             {
//                 descriptionBuilder.Emit("Children: ");
//                 descriptionBuilder.EmitArray(
//                     childEvaluations,
//                     (childEvaluation, _) => childEvaluation.Describe(descriptionBuilder));
//             }
//         });
//     }
// }
