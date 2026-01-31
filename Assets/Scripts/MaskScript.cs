public class MaskScript : Interactable
{
    public void OnPlayerNear()
    {
        //Iluminar efectos para que llame la atención del player
    }

    public override void OnInteract()
    {
        base.OnInteract();
        
        GameManager.Instance.OnPlayerTakeMask();
        
        // TODO Animacion coger Máscara
        
        Destroy(gameObject);
    }
}
