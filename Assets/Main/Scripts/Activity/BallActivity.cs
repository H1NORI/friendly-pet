using UnityEngine;
using UnityEngine.EventSystems;

public class BallActivity : IPetActivity
{
    public string Name => "Ball Play";

    private GameObject ballInstance;
    private Transform petTransform;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private float travelTime = 2f;
    private float elapsedTime = 0f;
    private bool isBallMoving = false;
    private bool isBounced = false;

    private float maxClickDistance = 1.5f;

    public void StartActivity(PetStateManager pet)
    {
        Debug.Log("Started ball play");

        // Save pet position
        petTransform = pet.transform;

        // Instantiate the ball
        startPoint = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
        endPoint = petTransform.position;

        ballInstance = GameObject.Instantiate(PetActivityManager.Instance.ballPrefab, startPoint, Quaternion.identity);

        elapsedTime = 0f;
        isBallMoving = true;
        isBounced = false;
    }

    public void UpdateActivity(PetStateManager pet)
    {
        if (ballInstance == null) return;

        // Handle ball movement
        if (isBallMoving && !isBounced)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / travelTime);
            ballInstance.transform.position = Vector3.Lerp(startPoint, endPoint, t);

            if (t >= 1f)
            {
                // Player failed to click in time
                EndActivity(pet);
            }
        }

        // Handle tap/click on the ball
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
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (ballInstance != null && hit.collider.gameObject == ballInstance)
            {
                // Bounce the ball back
                isBounced = true;
                startPoint = ballInstance.transform.position;
                endPoint = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
                elapsedTime = 0f;
                PetStateManager.Instance.Happiness += 5f;
                Debug.Log("Ball bounced!");
            }
        }
    }

    public void EndActivity(PetStateManager pet)
    {
        if (ballInstance != null)
        {
            GameObject.Destroy(ballInstance);
        }

        Debug.Log("Ended ball play");
        PetActivityManager.Instance.ChangeActivity(null);
        UIController.Instance.HideActivityCancelZoneUI();
    }
}
