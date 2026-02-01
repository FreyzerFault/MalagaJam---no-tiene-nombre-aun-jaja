using Puzzles;

namespace Characters
{
    public class PheasantCharacter : AnimalCharacter
    {
        protected override PuzzleManager.PuzzleType PuzzleType => PuzzleManager.PuzzleType.Faisan;
        
        protected override void OnMeetDialogueEnd()
        {
            base.OnMeetDialogueEnd();

            // TODO Antes de implementar los puzzles provisionalmente activamos que completa el puzzle
            OnCompletedPuzzle(PuzzleManager.PuzzleType.Faisan);
        }
    }
}
