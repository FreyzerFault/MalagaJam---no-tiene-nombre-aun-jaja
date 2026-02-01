using Dialogue.Dialogue;

namespace Characters
{
    public class DogCharacter : AnimalCharacter
    {
        private void Start()
        {
            DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
        }

        public override void OnDialogueEnd()
        {
            if (DialogueManager.Instance.CurrentCharacter != DialogueManager.Dialogue.Character.Perro) return;
            
            GameManager.Instance.AddMaskFragment();
        }
    }
}
