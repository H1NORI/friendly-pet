using UnityEngine;
using UnityEngine.EventSystems;

public class BallActivity : IPetActivity
{
    private int _minCoins = 3;
    private int _maxCoins = 7;

    public string Name => "Ball";
    public int minCoins { get => _minCoins; set => _minCoins = value; }
    public int maxCoins { get => _maxCoins; set => _maxCoins = value; }

    private GameObject ballInstance;
    private int currentBounces = 0;
    public int maxBounces = 3;
    public float travelDuration = 2.0f;
    public float arcHeight = 1f;

    private bool isBallClickable = false;
    private float moveTimer = 0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private bool isBallMovingToPet = true;

    public void StartActivity(PetStateManager pet)
    {
        UIController.Instance.ShowActivityBarUI();
        UIController.Instance.activityBar.value = 0;
        UIController.Instance.activityBar.maxValue = maxBounces;
        Debug.Log("Started Ball Activity");

        if (PetActivityManager.Instance.ballPrefab != null)
        {
            ballInstance = GameObject.Instantiate(
                PetActivityManager.Instance.ballPrefab,
                Camera.main.transform.position + Camera.main.transform.forward * 0.2f,
                Quaternion.identity
            );
        }

        currentBounces = 0;
        isBallMovingToPet = true;
        BeginArcMovement(Camera.main.transform.position, PetActivityManager.Instance.ballBounceTransform.position);

        UIController.Instance.ShowActivityCancelZoneUI();
    }

    public void UpdateActivity(PetStateManager pet)
    {
        if (ballInstance == null) return;

        moveTimer += Time.deltaTime;
        float t = Mathf.Clamp01(moveTimer / travelDuration);
        
        Vector3 currentPosition = GetArcPoint(startPosition, targetPosition, arcHeight, t);
        ballInstance.transform.position = currentPosition;

        if (t >= 1f)
        {
            if (isBallMovingToPet)
            {
                Vector3 returnTarget = Camera.main.transform.position + Camera.main.transform.forward * 0.2f;
                isBallMovingToPet = false;
                isBallClickable = false;
                BeginArcMovement(ballInstance.transform.position, returnTarget);
            }
            else
            {
                isBallClickable = true;
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            TryBounceBall(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TryBounceBall(Input.GetTouch(0).position);
        }
#endif
    }

    private void TryBounceBall(Vector2 screenPosition)
    {
        if (!isBallClickable || ballInstance == null) return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == ballInstance)
            {
                currentBounces++;
                UIController.Instance.activityBar.value = currentBounces;
                Debug.Log("Ball bounced! Count: " + currentBounces);

                if (currentBounces >= maxBounces)
                {
                    PetActivityManager.Instance.ChangeActivity(null);
                    return;
                }

                isBallClickable = false;
                isBallMovingToPet = true;

                BeginArcMovement(ballInstance.transform.position, PetActivityManager.Instance.ballBounceTransform.position);
            }
        }
    }

    private void BeginArcMovement(Vector3 from, Vector3 to)
    {
        startPosition = from;
        targetPosition = to;
        moveTimer = 0f;
    }

    private float ParabolaEaseOut(float t)
    {
        return 4f * t * (1f - Mathf.Pow(t, 0.5f));
    }

    private Vector3 GetArcPoint(Vector3 start, Vector3 end, float height, float t)
    {
        Vector3 linear = Vector3.Lerp(start, end, t);
        float arc = ParabolaEaseOut(t) * height;
        return linear + Vector3.up * arc;
    }

    public void EndActivity(PetStateManager pet)
    {
        Debug.Log("Ended Ball Activity");

        if (ballInstance != null)
        {
            Object.Destroy(ballInstance);
        }

        PetStateManager.Instance.ChangeHappiness(10f);

        PetActivityManager.Instance.DropCoins(minCoins, maxCoins);
        PetStateManager.Instance.SetTriggerAnimation("jump");

        UIController.Instance.HideActivityBarUI();
        UIController.Instance.HideActivityCancelZoneUI();
        UIController.Instance.ShowAllButtons();
    }
}
