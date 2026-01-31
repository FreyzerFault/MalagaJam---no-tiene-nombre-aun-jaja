using UnityEngine;
using UnityEngine.InputSystem;

public class MaskController: MonoBehaviour
{
    private bool maskOn;
        
    private void OnPutMask(InputValue value) => maskOn = value.Get<float>() > 0;
}