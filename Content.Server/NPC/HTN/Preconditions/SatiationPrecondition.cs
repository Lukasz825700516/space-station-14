using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.HTN.Preconditions;

/// <summary>
/// Returns true if the active hand entity has the specified components.
/// </summary>
public sealed partial class SatiationPrecondition : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [DataField(required: true)]
    public SatiationThreashold MinHungerState = SatiationThreashold.Desperate;
    [DataField(required: true)]
    public ProtoId<SatiationTypePrototype> SatiationType = "hungerSatiation";

    public override bool IsMet(NPCBlackboard blackboard)
    {
        if (!blackboard.TryGetValue<EntityUid>(NPCBlackboard.Owner, out var owner, _entManager))
        {
            return false;
        }

        if (!_entManager.TryGetComponent<SatiationComponent>(owner, out var component))
            return false;

        if (!component.Satiations.TryGetValue(SatiationType, out var satiation))
            return false;

        return satiation.CurrentThreshold <= MinHungerState;
    }
}
