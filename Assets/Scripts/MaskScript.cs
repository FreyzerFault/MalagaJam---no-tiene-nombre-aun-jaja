using UnityEngine;
using UnityEngine.InputSystem;

public class MaskScript : MonoBehaviour, IInteractable
{

    [SerializeField] private EventTrigger EventTrigger;
    [SerializeField] private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      

    }

    void Iluminar()
    {
        
    }

    public void OnInteract()
    {
        throw new System.NotImplementedException();
        // TODO animacion
        gameManager.SetHasMask(true);
      
        Destroy(gameObject);
    }
}
