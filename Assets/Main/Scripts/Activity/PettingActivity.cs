using UnityEngine;
using UnityEngine.EventSystems;

public class PettingActivity : IPetActivity
{
    private float pettingProgress = 0f;
    public float requiredPettingAmount = 5f;
    public float pettingPerSwipe = 1f;
    private bool isTouchingPet = false;

    private int _minCoins = 1;
    private int _maxCoins = 2;

    public string Name => "Petting";
    public int minCoins { get => _minCoins; set => _minCoins = value; }
    public int maxCoins { get => _maxCoins; set => _maxCoins = value; }

    public void StartActivity(PetStateManager pet)
    {
        UIController.Instance.ShowActivityBarUI();
        UIController.Instance.activityBar.value = 0;
        UIController.Instance.activityBar.maxValue = requiredPettingAmount;
        Debug.Log("Started petting");
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
        Debug.Log("Ended petting");
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

                    AddPettingProgress();
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
    
    private void AddPettingProgress()
    {
        pettingProgress += pettingPerSwipe;
        UIController.Instance.activityBar.value = pettingProgress;

        Debug.Log("Petting Progress: " + pettingProgress);

        if (pettingProgress >= requiredPettingAmount)
        {
            PetStateManager.Instance.Happiness += 10f;
            PetActivityManager.Instance.DropCoins(minCoins, maxCoins);
            Debug.Log("Pet is happy!");

            pettingProgress = 0f; // Reset for next session
            PetActivityManager.Instance.ChangeActivity(null);
            UIController.Instance.HideActivityBarUI();
            UIController.Instance.HideActivityCancelZoneUI();
            UIController.Instance.ShowAllButtons();
        }
    }
}
