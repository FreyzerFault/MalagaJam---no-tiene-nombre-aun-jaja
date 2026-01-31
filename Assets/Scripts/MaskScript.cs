using UnityEngine;

public class MaskScript : MonoBehaviour, IInteractable
{
    public Material brightMaterial;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      

    }

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
        
    }
}
