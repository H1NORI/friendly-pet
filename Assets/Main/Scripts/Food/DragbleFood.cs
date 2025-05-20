using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFood : MonoBehaviour
{
    public int hungerBonus;

    private Camera cam;
    private Vector3 initialDragPosition;
    private Vector3 dragVelocity;
    private bool isHeld = true;
    // private bool returningToCenter = false;
    private Vector3 returnTargetPosition;

    private Rigidbody rb;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Prevent dropping before release
        PositionInFrontOfCamera();
        UIController uiController = FindObjectOfType<UIController>();
        Debug.Log("UI controller");

        uiController.LookAtFood(transform);
    }

    void Update()
    {
        // if (returningToCenter)
        // {
        //     transform.position = Vector3.Lerp(transform.position, returnTargetPosition, Time.deltaTime * 8f);
        //     transform.LookAt(cam.transform);

        //     if (Vector3.Distance(transform.position, returnTargetPosition) < 0.01f)
        //     {
        //         returningToCenter = false;
        //         isHeld = true;
        //     }

        //     return;
        // }

        if (!isHeld)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            initialDragPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 current = Input.mousePosition;
            Vector3 delta = current - initialDragPosition;
            dragVelocity = delta;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }

        FollowCameraCenter();
    }

    void FollowCameraCenter()
    {
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        Vector3 camUp = cam.transform.up;

        Vector3 targetPos = cam.transform.position 
            + camForward * 0.5f 
            + camRight * dragVelocity.x * 0.0005f 
            + camUp * (dragVelocity.y * 0.0005f - 0.1f);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 8f);
        transform.LookAt(cam.transform);
    }

    void PositionInFrontOfCamera()
    {
        Vector3 camForward = cam.transform.forward;
        Vector3 camUp = cam.transform.up;

        transform.position = cam.transform.position + camForward * 1f - camUp * 0.1f;
        transform.rotation = Quaternion.LookRotation(camForward);
    }

    void Release()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.isKinematic = false;

        float minForceThreshold = 800f;
        float maxForceCap = 5f;
        float forceMagnitude = dragVelocity.magnitude;

        Debug.Log(forceMagnitude);

        if (forceMagnitude < minForceThreshold)
        {
            isHeld = true;
            rb.isKinematic = true;
            rb.useGravity = false;

            // returningToCenter = true;
            // returnTargetPosition = cam.transform.position + cam.transform.forward * 1.1f;
            return;
        }

        forceMagnitude = Mathf.Min(forceMagnitude, maxForceCap);

        Vector3 throwDirection = cam.transform.forward + cam.transform.up * 0.2f;
        rb.AddForce(throwDirection.normalized * forceMagnitude, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pet"))
        {
            UIController.Instance.FeedPet(hungerBonus);
            Destroy(gameObject);
        }
    }
}
