using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool hasMask; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    public bool GetHasMask() => hasMask;
    public void SetHasMask(bool value) {  hasMask = value; }
}
