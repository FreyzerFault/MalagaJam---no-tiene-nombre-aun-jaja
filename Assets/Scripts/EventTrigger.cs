using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    private void Start() => GetComponent<MeshRenderer>().enabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
            onTriggerEnter?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
            onTriggerExit?.Invoke();
    }
}
