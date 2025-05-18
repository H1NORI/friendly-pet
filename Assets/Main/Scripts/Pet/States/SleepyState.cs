using UnityEngine;

public class SleepyState : IPetState
{

    public void EnterState(PetStateManager pet)
    {
        Debug.Log("Pet started beeing sleepy.");
    }

    public void UpdateState(PetStateManager pet)
    {

    }

    public void ExitState(PetStateManager pet)
    {
        Debug.Log("Pet finished beeing sleepy.");
    }
}