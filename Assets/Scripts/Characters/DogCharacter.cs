namespace Characters
{
    public class DogCharacter : AnimalCharacter
    {
        private void Start()
        {
            DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
        }

        public override void OnDialogueEnd(DialogueManager.AnimalType currentDialogue)
        {
            if (currentDialogue != DialogueManager.AnimalType.Perro) return;
            
            // TODO Darle la mascara
        }
    }
}
