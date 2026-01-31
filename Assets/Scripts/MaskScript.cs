using UnityEngine;

public class MaskScript : MonoBehaviour, IInteractable
{
    public Material brightMaterial;

    public void OnPlayerNear()
    {
        //Iluminar efectos
   
    }

    public void OnInteract()
    {
        // TODO animacion
        Debug.Log("ME HAS COGIDO");
        
        GameManager.Instance.OnPlayerTakeMask();

        Destroy(gameObject);
    }

    private void StartBrightVFX()
    {
        // TODO
    }
}
