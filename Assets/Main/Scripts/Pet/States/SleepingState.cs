using UnityEngine;

public class SleepingState : IPetState
{
    private float sleepingDuration = 5f;
    private float timer;

    public void EnterState(PetStateManager pet)
    {
        Debug.Log("Pet started sleeping.");
        timer = sleepingDuration;
    }

    public void UpdateState(PetStateManager pet)
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            pet.ChangeState(pet.idleState);
        }
    }

    public void ExitState(PetStateManager pet)
    {
        Debug.Log("Pet woke up.");
    }
}