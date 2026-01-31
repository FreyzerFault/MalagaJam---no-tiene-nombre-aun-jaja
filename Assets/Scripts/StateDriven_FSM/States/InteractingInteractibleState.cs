using UnityEngine;

namespace StateDriven_FSM.States
{
    public class InteractingInteractibleState: InteractibleState<Interactible>
    {
        public override string ToString() => "Interacting";
        public override Color Color => Color.red;
        
        public override void Execute(Interactible interactible)
        {
        }
    }
}