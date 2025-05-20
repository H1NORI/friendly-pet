using UnityEngine;

public class DraggableFood : MonoBehaviour
{
    public int hungerBonus;
    public float throwForceMultiplier = 5f;

    private Camera cam;
    private Vector3 initialDragPosition;
    private Vector3 dragVelocity;
    private bool isHeld = true;

    private Rigidbody rb;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Prevent dropping before release
        PositionInFrontOfCamera();
    }

    void Update()
    {
        if (!isHeld) return;

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
        Vector3 targetPos = cam.transform.position + camForward * 1.0f + cam.transform.right * dragVelocity.x * 0.002f + cam.transform.up * dragVelocity.y * 0.002f;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 8f);
        transform.LookAt(cam.transform); // Optional: face the camera
    }

    void PositionInFrontOfCamera()
    {
        transform.position = cam.transform.position + cam.transform.forward * 1.5f;
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }

    void Release()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        Vector3 throwDirection = cam.transform.forward + cam.transform.up * 0.2f;
        rb.AddForce(throwDirection * dragVelocity.magnitude * throwForceMultiplier);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pet")) // Or use a FeedZone
        {
            UIController.Instance.FeedPet(hungerBonus);
            Destroy(gameObject);
        }
    }
}
