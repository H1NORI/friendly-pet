using UnityEngine;

public class HungryState : IPetState
{
    public void EnterState(PetStateManager pet)
    {
        Debug.Log("Pet started beeing hungry.");
    }

    public void UpdateState(PetStateManager pet)
    {

    }

    public void ExitState(PetStateManager pet)
    {
        Debug.Log("Pet finished beeing hungry.");
    }
}