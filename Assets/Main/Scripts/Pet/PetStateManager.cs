using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PetStateManager : MonoBehaviour
{
    public static PetStateManager Instance { get; private set; }

    [Header("Stats")]
    [SerializeField, Range(0, 100)]
    private float hunger = 100f;
    public float Hunger { get; private set; }

    [SerializeField, Range(0, 100)]
    private float sleepiness = 100f;
    public float Sleepiness { get; private set; }

    [SerializeField, Range(0, 100)]
    private float happiness = 100f;
    public float Happiness { get; private set; }

    public float statDecayRate = 5f; // per minute

    [Header("States")]
    public IPetState currentState;
    public IdleState idleState { get; private set; } = new IdleState();
    public HungryState hungryState { get; private set; } = new HungryState();
    public EatingState eatingState { get; private set; } = new EatingState();
    public SleepingState sleepingState { get; private set; } = new SleepingState();

    private float statTimer;

    private MultiAimConstraint multiAimConstraint;
    private RigBuilder rigBuilder;
    private Animator animator;

    void Awake()
    {
        Instance = this;

        Hunger = hunger;
        Sleepiness = sleepiness;
        Happiness = happiness;
    }

    void Start()
    {
        ChangeState(idleState);
        UIController.Instance.UpdateAllProgressBars();

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

    public void ChangeHunger(float hungerBonus)
    {
        if (Hunger + hungerBonus > 100)
        {
            Hunger = 100;
        }
        else if (Hunger + hungerBonus < 0)
        {
            Hunger = 0;
        }
        else
        {
            Hunger += hungerBonus;
        }
    }

    public void ChangeHappiness(float happinessBonus)
    {
        if (Happiness + happinessBonus > 100)
        {
            Happiness = 100;
        }
        else if (Happiness + happinessBonus < 0)
        {
            Happiness = 0;
        }
        else
        {
            Happiness += happinessBonus;
        }
    }

    public void ChangeSleepiness(float sleepinessBonus)
    {
        if (Sleepiness + sleepinessBonus > 100)
        {
            Sleepiness = 100;
        }
        else if (Sleepiness + sleepinessBonus < 0)
        {
            Sleepiness = 0;
        }
        else
        {
            Sleepiness += sleepinessBonus;
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
