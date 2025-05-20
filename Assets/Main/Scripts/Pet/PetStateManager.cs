using System;
using UnityEngine;

public class PetStateManager : MonoBehaviour
{
    [Header("Pet Stats")]
    [Range(0, 100)] public float Hunger = 100f;
    [Range(0, 100)] public float Sleepiness = 100f;
    [Range(0, 100)] public float Happiness = 100f;

    public float statDecayRate = 5f; // per minute
    public event Action<float> OnHungerChanged;
    public event Action<float> OnSleepinessChanged;
    public event Action<float> OnHappinessChanged;

    private IPetState currentState;
    public IdleState idleState = new IdleState();
    public HungryState hungryState = new HungryState();
    public EatingState eatingState = new EatingState();
    public SleepyState sleepyState = new SleepyState();
    public SleepingState sleepingState = new SleepingState();

    private float statTimer;

    void Start()
    {
        ChangeState(idleState);
    }

    void Update()
    {
        statTimer += Time.deltaTime;
        if (statTimer >= 1f) // Every second
        {
            UpdateStats();
            statTimer = 0f;
        }
        currentState.UpdateState(this);
    }

    public void ChangeState(IPetState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    private void UpdateStats()
    {
        Hunger = Mathf.Max(0, Hunger - (statDecayRate / 60f));
        Sleepiness = Mathf.Max(0, Sleepiness - (statDecayRate / 60f));
        Happiness = Mathf.Max(0, Happiness - (statDecayRate / 60f));

        // Debug.Log(Hunger);

        OnHungerChanged?.Invoke(Hunger);
        OnSleepinessChanged?.Invoke(Sleepiness);
        OnHappinessChanged?.Invoke(Happiness);
    }

    public void Feed(float amount)
    {
        Hunger = Mathf.Min(100f, Hunger + amount);
        OnHungerChanged?.Invoke(Hunger);
        ChangeState(eatingState);
    }

    public void Sleep(float amount)
    {
        Sleepiness = Mathf.Min(100f, Sleepiness + amount);
        OnSleepinessChanged?.Invoke(Sleepiness);
        ChangeState(sleepingState);
    }

    public void Play(float amount)
    {
        Happiness = Mathf.Min(100f, Happiness + amount);
        OnHappinessChanged?.Invoke(Happiness);
        ChangeState(idleState); // temporary: may have PlayState later
    }

    public void IncreaseHunger(int hungerBonus)
    {
        if (Hunger + hungerBonus > 100)
        {
            Hunger = 100;
        }
        else
        {
            Hunger += hungerBonus;
        }
    }
}
