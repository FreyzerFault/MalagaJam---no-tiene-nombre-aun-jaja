using UnityEngine;
using UnityEngine.InputSystem;

public class MaskScript : MonoBehaviour, IInteractable
{

    [SerializeField] private EventTrigger EventTrigger;
    [SerializeField] private GameManager gameManager;
    
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
        gameManager.OnPlayerTakeMask();

        Destroy(gameObject);
    }

    private void StartBrightVFX()
    {
        
    }
}
