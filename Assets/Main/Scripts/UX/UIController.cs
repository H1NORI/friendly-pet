using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("Panels")]
    public GameObject feedingPanel;
    public GameObject activityPanel;

    // [Header("Stat UI")]
    // public Slider hungerSlider;
    // public Slider sleepSlider;
    // public Slider happinessSlider;

    [Header("Feeding Items")]
    public List<FoodItem> foodItems;
    public GameObject foodButtonPrefab;
    public Transform foodButtonContainer;


    private PetStateManager petStateManager;

    public RectTransform feedZoneUI;


    void Awake()
    {
        PopulateFeedingUI();
    }

    public void ShowFeedingUI()
    {
        Animator animator = feedingPanel.GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

    public void HideFeedingUI()
    {
        Animator animator = feedingPanel.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void ShowActivityUI()
    {
        Animator animator = activityPanel.GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

        public void DisableActivityUI()
    {
        Animator animator = activityPanel.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void HideAllPanels()
    {
        feedingPanel.SetActive(false);
        activityPanel.SetActive(false);
    }

    private void PopulateFeedingUI()
    {
        foreach (var food in foodItems)
        {
            GameObject buttonObj = Instantiate(foodButtonPrefab, foodButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.GetComponent<Image>();
            icon.sprite = food.icon;

            button.onClick.AddListener(() =>
            {
                ShowFood(food);
            });

            Debug.Log("Event added");
        }

        RectTransform rectTransform = foodButtonContainer.GetComponent<RectTransform>();
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        anchoredPos.x = -10000f;
        rectTransform.anchoredPosition = anchoredPos;
    }

    private void ShowFood(FoodItem food)
    {
        petStateManager = FindObjectOfType<PetStateManager>();

        if (petStateManager != null)
        {
            Debug.Log("petStateManager exists");

            if (food.prefab != null)
            {
                Debug.Log("food prefab exists");
                Instantiate(food.prefab, petStateManager.transform.position + Vector3.forward, Quaternion.identity);
            }
        }
    }

    public void FeedPet(int hungerBonus)
    {
        petStateManager = FindObjectOfType<PetStateManager>();

        if (petStateManager != null)
        {
            petStateManager.IncreaseHunger(hungerBonus);
        }
    }

    public bool IsInFeedZone(Vector3 worldPos)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        return RectTransformUtility.RectangleContainsScreenPoint(feedZoneUI, screenPos);
    }
}
