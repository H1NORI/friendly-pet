using UnityEngine;

public class PetActivityManager : MonoBehaviour
{
    public static PetActivityManager Instance { get; private set; }

    private IPetActivity currentActivity;

    public GameObject pettingParticlePrefab;
    public GameObject ballPrefab;
    public GameObject coinPrefab;
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

    public void DropCoins(int minInclusive, int maxExclusive)
    {
        int coinCount = Random.Range(minInclusive, maxExclusive);

        for (int i = 0; i < coinCount; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f); // Random angle in radians
            float radius = 0.3f;

            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            Vector3 dropPosition = transform.position + offset + Vector3.up * 0.2f; // Add some height

            GameObject coin = Instantiate(
                PetActivityManager.Instance.coinPrefab,
                dropPosition,
                Quaternion.identity
            );
        }
    }

}