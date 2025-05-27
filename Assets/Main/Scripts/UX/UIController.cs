using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("Buttons")]
    public GameObject feedingButton;
    public GameObject activityButton;
    public GameObject cameraButton;

    [Header("Panels")]
    public GameObject feedingPanel;
    public GameObject activityPanel;

    [Header("Bars")]
    public Slider hungerBar;
    public Slider happinessBar;
    public Slider activityBar;


    [Header("Feeding Items")]
    public List<FoodItem> foodItems;
    public GameObject foodButtonPrefab;
    public Transform foodButtonContainer;
    public GameObject feedingCancelZone;
    private GameObject spawnedFood;

    [Header("Activity Items")]
    public List<ActivityItem> activityItems;
    public GameObject activityButtonPrefab;
    public Transform activityButtonContainer;
    public GameObject activityCancelZone;
    public SliderController activitySliderController;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PopulateFeedingUI();
        PopulateActivityUI();
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

        public void HideActivityUI()
    {
        Animator animator = activityPanel.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void ShowActivityBarUI()
    {
        Animator animator = activityBar.GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

        public void HideActivityBarUI()
    {
        Animator animator = activityBar.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void HideFeedingCancelZoneUI()
    {
        Animator animator = feedingCancelZone.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void ShowFeedingCancelZoneUI()
    {
        Animator animator = feedingCancelZone.GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

    public void HideActivityCancelZoneUI()
    {
        Animator animator = activityCancelZone.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void ShowActivityCancelZoneUI()
    {
        Animator animator = activityCancelZone.GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

    public void HideAllButtons()
    {
        feedingButton.SetActive(false);
        activityButton.SetActive(false);
        cameraButton.SetActive(false);
    }

    public void ShowAllButtons()
    {
        feedingButton.SetActive(true);
        activityButton.SetActive(true);
        cameraButton.SetActive(true);
    }

    public void HideAllPanels()
    {
        feedingPanel.SetActive(false);
        activityPanel.SetActive(false);
    }

    public void UpdateHungerProgressBars()
    {
        hungerBar.value = PetStateManager.Instance.Hunger;
    }

    public void UpdateHappinessProgressBars()
    {
        happinessBar.value = PetStateManager.Instance.Happiness;
    }

    public void UpdateAllProgressBars()
    {
        UpdateHungerProgressBars();
        UpdateHappinessProgressBars();
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
                HideFeedingUI();
                ShowFeedingCancelZoneUI();
                // ShowAllButtons();
                ShowFood(food);
            });
        }

        RectTransform rectTransform = foodButtonContainer.GetComponent<RectTransform>();
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        anchoredPos.x = -10000f;
        rectTransform.anchoredPosition = anchoredPos;
    }

    private void PopulateActivityUI()
    {
        foreach (var activity in activityItems)
        {
            GameObject buttonObj = Instantiate(activityButtonPrefab, activityButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.GetComponent<Image>();
            icon.sprite = activity.icon;

            button.onClick.AddListener(() =>
            {
                HideActivityUI();
                ShowActivityCancelZoneUI();
                StartActivity(activity);
            });
        }

        RectTransform rectTransform = foodButtonContainer.GetComponent<RectTransform>();
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        anchoredPos.x = 10000f;
        rectTransform.anchoredPosition = anchoredPos;
    }

    private void ShowFood(FoodItem food)
    {
        HideFood();

        if (PetStateManager.Instance != null)
        {
            if (food.prefab != null)
            {
                spawnedFood = Instantiate(food.prefab, PetStateManager.Instance.transform.position + Vector3.forward, Quaternion.identity);
            }
        }
    }

    private void StartActivity(ActivityItem activityItem)
    {
        if (PetActivityManager.Instance != null)
        {
            PetActivityManager.Instance.ChangeActivity(ActivityFactory.GetActivity(activityItem.activity));
        }
    }

    public void HideFood()
    {
        if (spawnedFood != null)
        {
            PetStateManager.Instance.LookAtFood(null);
            Destroy(spawnedFood);
        }
    }

    public void FeedPet(int hungerBonus)
    {
        if (PetStateManager.Instance != null)
        {
            PetStateManager.Instance.IncreaseHunger(hungerBonus);
        }
    }
}