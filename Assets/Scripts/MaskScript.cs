using StateDriven_FSM;

public class MaskScript : Interactible
{
    public void OnPlayerNear()
    {
        //Iluminar efectos para que llame la atención del player
    }

    public override void SwitchState(InteractibleState<Interactible> newState)
    {
        base.SwitchState(newState);
        
        // Cuando interaccione con la mascara
        if (IsInteracting)
        {
            GameManager.Instance.OnPlayerTakeMask();

            // TODO Animacion coger Máscara

            Destroy(gameObject);
        }
    }
}
