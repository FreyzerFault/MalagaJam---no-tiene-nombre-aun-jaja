namespace Characters
{
    public class Pheasant : AnimalCharacter
    {
        public override void OnDialogueEnd()
        {
            // TODO Empezar el puzzle
            // Provisionalmente: Te da ya el trozo de Máscara
            GameManager.Instance.AddMaskFragment();
        }
    }
}
