using UnityEngine;

public class EatingState : IPetState
{
    private float eatingDuration = 2f;
    private float timer;

    public void EnterState(PetStateManager pet)
    {
        Debug.Log("Pet started eating.");
        timer = eatingDuration;
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
        Debug.Log("Pet finished eating.");
    }
}