using Controllers;
using Dialogue;

namespace Characters
{
    public class DogCharacter : AnimalCharacter
    {
        private void Start()
        {
            DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
        }

        public override void OnDialogueEnd(DialogueManager.DialogueTag currentDialogue)
        {
            if (currentDialogue != DialogueManager.DialogueTag.Perro) return;
            
            GameManager.Instance.OnPlayerTakeMask();
        }
    }
}
