using UnityEngine;
using UnityEngine.EventSystems;

public class BrushActivity : IPetActivity
{
    private float brushingProgress = 0f;
    public float requiredBrushingAmount = 20f;
    public float brushingPerSwipe = 1f;

    private bool isTouchingPet = false;
    public string Name => "Brushing";

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
            PetStateManager.Instance.Happiness += 10f;
            Debug.Log("Pet is happy!");

            brushingProgress = 0f; // Reset for next session
            PetActivityManager.Instance.ChangeActivity(null);
            UIController.Instance.HideActivityBarUI();
            UIController.Instance.HideActivityCancelZoneUI();
            UIController.Instance.ShowAllButtons();
        }
    }
}
