using UnityEngine;
using UnityEngine.EventSystems;

public class BrushActivity : IPetActivity
{
    private float brushingProgress = 0f;
    public float requiredBrushingAmount = 20f;
    public float brushingPerSwipe = 1f;

    private bool isTouchingPet = false;
    private int _minCoins = 2;
    private int _maxCoins = 4;

    public string Name => "Brushing";
    public int minCoins { get => _minCoins; set => _minCoins = value; }
    public int maxCoins { get => _maxCoins; set => _maxCoins = value; }

    public void StartActivity(PetStateManager pet)
    {
        UIController.Instance.ShowActivityBarUI();
        UIController.Instance.activityBar.value = 0;
        UIController.Instance.activityBar.maxValue = requiredBrushingAmount;
        Debug.Log("Started brushing");
    }

    public void UpdateActivity(PetStateManager pet)
    {       
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            CheckTouch(pet, Input.mousePosition);
        }
#else
        if (Input.touchCount > 0)
        {
            CheckTouch(pet, Input.GetTouch(0).position);
        }
#endif
    }

    public void EndActivity(PetStateManager pet)
    {
        Debug.Log("Ended brushing");
        PetStateManager.Instance.ChangeHappiness(10f);
        brushingProgress = 0f;

        PetActivityManager.Instance.DropCoins(minCoins, maxCoins);
        PetStateManager.Instance.SetTriggerAnimation("jump");

        UIController.Instance.HideActivityBarUI();
        UIController.Instance.HideActivityCancelZoneUI();
        UIController.Instance.ShowAllButtons();
    }

    private void CheckTouch(PetStateManager pet, Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == pet.gameObject)
            {
                if (!isTouchingPet)
                {
                    isTouchingPet = true;

                    if (PetActivityManager.Instance.pettingParticlePrefab != null)
                    {
                        GameObject.Instantiate(PetActivityManager.Instance.pettingParticlePrefab, hit.point, Quaternion.identity);
                    }

                    AddBrushingProgress();
                }
            }
            else
            {
                isTouchingPet = false;
            }
        }
        else
        {
            isTouchingPet = false;
        }
    }
    
    private void AddBrushingProgress()
    {
        brushingProgress += brushingPerSwipe;
        UIController.Instance.activityBar.value = brushingProgress;

        Debug.Log("Brushing Progress: " + brushingProgress);

        if (brushingProgress >= requiredBrushingAmount)
        {
            PetActivityManager.Instance.ChangeActivity(null);
        }
    }
}
