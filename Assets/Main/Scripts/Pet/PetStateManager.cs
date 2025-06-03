using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PetStateManager : MonoBehaviour
{
    public static PetStateManager Instance { get; private set; }

    [Header("Stats")]
    [Range(0, 100)] public float Hunger = 100f;
    [Range(0, 100)] public float Sleepiness = 100f;
    [Range(0, 100)] public float Happiness = 100f;

    public float statDecayRate = 5f; // per minute

    [Header("States")]
    private IPetState currentState;
    public IdleState idleState { get; private set; } = new IdleState();
    public HungryState hungryState { get; private set; } = new HungryState();
    public EatingState eatingState { get; private set; } = new EatingState();
    public SleepyState sleepyState { get; private set; } = new SleepyState();
    public SleepingState sleepingState { get; private set; } = new SleepingState();

    private float statTimer;

    private MultiAimConstraint multiAimConstraint;
    private RigBuilder rigBuilder;
    private Animator animator;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(idleState);

        multiAimConstraint = FindObjectOfType<MultiAimConstraint>();
        rigBuilder = GetComponent<RigBuilder>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        statTimer += Time.deltaTime;
        if (statTimer >= 1f)
        {
            UpdateStats();
            UIController.Instance.UpdateAllProgressBars();
            statTimer = 0f;
        }

        currentState.UpdateState(this);
    }

    public void ChangeState(IPetState newState)
    {
        if (currentState != null)
            currentState.ExitState(this);

        currentState = newState;
        currentState.EnterState(this);
    }

    private void UpdateStats()
    {
        Hunger = Mathf.Max(0, Hunger - (statDecayRate / 60f));
        Sleepiness = Mathf.Max(0, Sleepiness - (statDecayRate / 60f));
        Happiness = Mathf.Max(0, Happiness - (statDecayRate / 60f));
    }

    public void Feed(float amount)
    {
        Hunger = Mathf.Min(100f, Hunger + amount);
        ChangeState(eatingState);
        UIController.Instance.UpdateAllProgressBars();
    }

    public void Sleep(float amount)
    {
        Sleepiness = Mathf.Min(100f, Sleepiness + amount);
        ChangeState(sleepingState);
    }

    public void Play(float amount)
    {
        Happiness = Mathf.Min(100f, Happiness + amount);
        ChangeState(idleState); // todo: may have PlayState later
        UIController.Instance.UpdateAllProgressBars();
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


    public void LookAtFood(Transform food)
    {
        if (multiAimConstraint == null || rigBuilder == null)
        {
            Debug.LogWarning("Constraint or target is missing.");
            return;
        }

        if (food == null)
        {
            SetBoolAnimation("feeding", false);
        }
        else
        {
            SetBoolAnimation("feeding", true);
        }

        multiAimConstraint.data.sourceObjects = functionGetSources(food);
        multiAimConstraint.weight = 1f;
        rigBuilder.Build();

        Debug.Log("we found multiAimConstraint");
    }

    public WeightedTransformArray functionGetSources(Transform target)
    {
        WeightedTransformArray sources = new WeightedTransformArray();
        sources.Add(new WeightedTransform(target, 1f));
        return sources;
    }

    public void SetTriggerAnimation(string name)
    {
        animator.SetTrigger(name);
    }

    public void SetBoolAnimation(string name, bool value)
    {
        animator.SetBool(name, value);
    }
}
