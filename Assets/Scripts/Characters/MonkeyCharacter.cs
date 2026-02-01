using Puzzles;

namespace Characters
{
    public class MonkeyCharacter : AnimalCharacter
    {
        protected override PuzzleManager.PuzzleType PuzzleType => PuzzleManager.PuzzleType.Macaco;

        protected override void OnMeetDialogueEnd()
        {
            base.OnMeetDialogueEnd();

            // TODO Antes de implementar los puzzles provisionalmente activamos que completa el puzzle
            OnCompletedPuzzle(PuzzleManager.PuzzleType.Macaco);
        }
    }
}
