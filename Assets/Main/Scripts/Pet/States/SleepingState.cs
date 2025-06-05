using UnityEngine;

public class SleepingState : IPetState
{
    private float recoveryRate = 1f; // sleepiness recovery per minute

    public void EnterState(PetStateManager pet)
    {
        Debug.Log("Pet started sleeping.");
        pet.SetTriggerAnimation("sleep");
        pet.SetBoolAnimation("sleeping", true);
    }

    public void UpdateState(PetStateManager pet)
    {
        pet.ChangeSleepiness(recoveryRate / 60f);

        if (pet.Sleepiness >= 100f)
        {
            // pet.ChangeState(pet.idleState);
        }
    }

    public void ExitState(PetStateManager pet)
    {
        Debug.Log("Pet woke up.");
        pet.SetBoolAnimation("sleeping", false);
    }
}