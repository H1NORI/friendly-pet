using UnityEngine;

public class IdleState : IPetState
{
    public void EnterState(PetStateManager pet)
    {
        Debug.Log("Pet entered Idle state.");
    }

    public void UpdateState(PetStateManager pet)
    {
        if (pet.Hunger < 30f)
        {
            pet.ChangeState(pet.hungryState);
        }
        else if (pet.Sleepiness < 30f)
        {
            pet.ChangeState(pet.sleepyState);
        }
    }

    public void ExitState(PetStateManager pet)
    {
        Debug.Log("Pet exiting Idle state.");
    }
}