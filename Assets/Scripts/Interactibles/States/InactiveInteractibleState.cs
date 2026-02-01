using UnityEngine;

namespace Interactibles.States
{
    public class InactiveInteractibleState: InteractibleState<Interactible>
    {
        public override string ToString() => "Inactive";
        public override Color Color => Color.darkGray;
        
        public override void Execute(Interactible interactible)
        {
        }
    }
}