using UnityEngine;

namespace Interactibles.States
{
    public class FocusInteractibleState: InteractibleState<Interactible>
    {
        public override string ToString() => "Focus";
        public override Color Color => Color.yellow;
        
        public override void Execute(Interactible interactible)
        {
        }

    }
}