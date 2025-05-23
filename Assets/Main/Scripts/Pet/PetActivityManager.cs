using UnityEngine;

public class PetActivityManager : MonoBehaviour
{
    public static PetActivityManager Instance { get; private set; }

    private IPetActivity currentActivity;

    public GameObject pettingParticlePrefab;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeActivity(new PettingActivity());
    }

    void Update()
    {
        currentActivity.UpdateActivity(PetStateManager.Instance);
    }

    public void ChangeActivity(IPetActivity newActivity)
    {
        if (currentActivity != null)
        {
            currentActivity.EndActivity(PetStateManager.Instance);
        }

        currentActivity = newActivity;
        currentActivity.StartActivity(PetStateManager.Instance);
    }
}