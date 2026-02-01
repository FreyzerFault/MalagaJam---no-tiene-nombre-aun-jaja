namespace Characters
{
    public class DogCharacter : AnimalCharacter
    {
        protected override void OnMeetDialogueEnd()
        {
            GameManager.Instance.AddMaskFragment(); // Te la entrega sin mas
        }
    }
}
