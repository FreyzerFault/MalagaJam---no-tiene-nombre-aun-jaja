using UnityEngine;

namespace StateDriven_FSM.States
{
    public class ActiveInteractibleState: InteractibleState<Interactible>
    {
        public override string ToString() => "Active";
        public override Color Color => Color.white;
        
        public override void Execute(Interactible interactible) {} // DO NOTHING
    }
}
