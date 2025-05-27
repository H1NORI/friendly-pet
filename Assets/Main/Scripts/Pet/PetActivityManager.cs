using UnityEngine;

public class PetActivityManager : MonoBehaviour
{
    public static PetActivityManager Instance { get; private set; }

    private IPetActivity currentActivity;

    public GameObject pettingParticlePrefab;
    public GameObject ballPrefab;
    public Transform ballBounceTransform;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeActivity(null);
    }

    void Update()
    {
        if (currentActivity != null)
            currentActivity.UpdateActivity(PetStateManager.Instance);
    }

    public void ChangeActivity(IPetActivity newActivity)
    {
        if (currentActivity != null)
            currentActivity.EndActivity(PetStateManager.Instance);

        currentActivity = newActivity;

        if (currentActivity != null)
            currentActivity.StartActivity(PetStateManager.Instance);
    }
}