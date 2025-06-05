using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("Buttons")]
    public GameObject feedingButton;
    public GameObject activityButton;
    public GameObject cameraButton;
    public GameObject sleepButton;
    public GameObject shopButton;
    public GameObject closeButtonPrefab;

    [Header("Panels")]
    public GameObject feedingPanel;
    public GameObject activityPanel;

    [Header("Bars")]
    public Slider hungerBar;
    public Slider happinessBar;
    public Slider sleepinessBar;
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

    [Header("Sleeping Items")]
    public GameObject sleepingCancelZone;

    [Header("Shop Items")]

    private int activeTab = 0;
    public GameObject shopContainer;
    public TMP_Text coinsText;
    public Transform shopFoodTab;
    public Transform shopActivityTab;
    public Transform shopFoodContainer;
    public Transform shopActivityContainer;
    public GameObject shopFoodPrefab;
    public GameObject shopActivityPrefab;


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

        PopulateShopFooodUI();
        PopulateShopActivityUI();
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

    public void HideSleepingCancelZoneUI()
    {
        Animator animator = sleepingCancelZone.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void ShowSleepingCancelZoneUI()
    {
        Animator animator = sleepingCancelZone.GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

    public void HideShopUI()
    {
        Animator animator = shopContainer.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void ShowShopUI()
    {
        Animator animator = shopContainer.GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

    public void UpdateCoinsText()
    {
        coinsText.text = $"{CurrencyManager.Instance.coins}";
    }

    public void ToggleFoodTabUI(int tab)
    {
        if (tab == activeTab)
        {
            return;
        }
        else if (tab == 0 && activeTab != tab)
        {
            ShowShopFoodTabUI();
            HideShopActivityTabUI();
            activeTab = tab;
        }
        else if (tab == 1 && activeTab != tab)
        {
            ShowShopActivityTabUI();
            HideShopFoodTabUI();
            activeTab = tab;
        }
    }

    public void ShowShopFoodTabUI()
    {
        Animator animator = shopFoodTab.GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

    public void ShowShopActivityTabUI()
    {
        Animator animator = shopActivityTab.GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

    public void HideShopFoodTabUI()
    {
        Animator animator = shopFoodTab.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void HideShopActivityTabUI()
    {
        Animator animator = shopActivityTab.GetComponent<Animator>();
        animator.SetTrigger("Disable");
    }

    public void HideAllButtons()
    {
        feedingButton.SetActive(false);
        activityButton.SetActive(false);
        cameraButton.SetActive(false);
        sleepButton.SetActive(false);
        shopButton.SetActive(false);
    }

    public void ShowAllButtons()
    {
        feedingButton.SetActive(true);
        activityButton.SetActive(true);
        cameraButton.SetActive(true);
        sleepButton.SetActive(true);
        shopButton.SetActive(true);
    }

    public void HideAllPanels()
    {
        feedingPanel.SetActive(false);
        activityPanel.SetActive(false);
    }

    public void UpdateHungerProgressBar()
    {
        hungerBar.value = PetStateManager.Instance.Hunger;
    }

    public void UpdateHappinessProgressBar()
    {
        happinessBar.value = PetStateManager.Instance.Happiness;
    }

    public void UpdateSleepinessProgressBar()
    {
        sleepinessBar.value = PetStateManager.Instance.Sleepiness;
    }


    public void UpdateAllProgressBars()
    {
        UpdateHungerProgressBar();
        UpdateHappinessProgressBar();
        UpdateSleepinessProgressBar();
    }

    private void PopulateFeedingUI()
    {
        GameObject closeButtonObj = Instantiate(closeButtonPrefab, foodButtonContainer);
        Button closeButton = closeButtonObj.GetComponent<Button>();

        closeButton.onClick.AddListener(() =>
        {
            HideFeedingUI();
            ShowAllButtons();
        });

        foreach (var food in foodItems)
        {
            GameObject buttonObj = Instantiate(foodButtonPrefab, foodButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.GetComponent<Image>();
            icon.sprite = food.icon;

            if (!InventoryManager.Instance.IsFoodOwned(food))
            {
                button.interactable = false;
            }

            button.onClick.AddListener(() =>
            {
                HideFeedingUI();
                ShowFeedingCancelZoneUI();
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
        GameObject closeButtonObj = Instantiate(closeButtonPrefab, activityButtonContainer);
        Button closeButton = closeButtonObj.GetComponent<Button>();

        closeButton.onClick.AddListener(() =>
        {
            HideActivityUI();
            ShowAllButtons();
        });

        foreach (var activity in activityItems)
        {
            GameObject buttonObj = Instantiate(activityButtonPrefab, activityButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.GetComponent<Image>();
            icon.sprite = activity.icon;

            if (!InventoryManager.Instance.IsActivityOwned(activity))
            {
                button.interactable = false;
            }

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

    private void PopulateShopFooodUI()
    {
        foreach (var food in foodItems)
        {
            GameObject buttonObj = Instantiate(shopFoodPrefab, shopFoodContainer);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.GetComponentsInChildren<Image>()[1];
            icon.sprite = food.icon;
            TMP_Text[] texts = buttonObj.GetComponentsInChildren<TMP_Text>();
            if (texts.Length == 2)
            {
                texts[0].text = $"{food.name} - {food.price}$";
                texts[1].text = food.descripion;
            }

            if (InventoryManager.Instance.IsFoodOwned(food))
            {
                button.interactable = false;
            }

            button.onClick.AddListener(() =>
            {
                ShopManager.Instance.BuyFood(food);
            });
        }

        RectTransform rectTransform = shopFoodContainer.GetComponent<RectTransform>();
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        anchoredPos.y = -10000f;
        rectTransform.anchoredPosition = anchoredPos;
    }


    private void PopulateShopActivityUI()
    {
        foreach (var activity in activityItems)
        {
            GameObject buttonObj = Instantiate(shopActivityPrefab, shopActivityContainer);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.GetComponentsInChildren<Image>()[1];
            icon.sprite = activity.icon;
            TMP_Text[] texts = buttonObj.GetComponentsInChildren<TMP_Text>();
            if (texts.Length == 2)
            {
                texts[0].text = $"{activity.name} - {activity.price}$";
                texts[1].text = activity.descripion;
            }

            if (InventoryManager.Instance.IsActivityOwned(activity))
            {
                button.interactable = false;
            }

            button.onClick.AddListener(() =>
            {
                ShopManager.Instance.BuyActivity(activity);
            });
        }

        RectTransform rectTransform = shopActivityContainer.GetComponent<RectTransform>();
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        anchoredPos.y = -10000f;
        rectTransform.anchoredPosition = anchoredPos;
    }

    public void ReloadFoodInventoryUI()
    {
        ClearChildren(foodButtonContainer);
        PopulateFeedingUI();
    }

    public void ReloadActivityInventoryUI()
    {
        ClearChildren(activityButtonContainer);
        PopulateActivityUI();
    }

    public void ReloadShopFoodUI()
    {
        ClearChildren(shopFoodContainer);
        PopulateShopFooodUI();
    }

    public void ReloadShopActivityUI()
    {
        ClearChildren(shopActivityContainer);
        PopulateShopActivityUI();
    }

    public void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
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
            PetStateManager.Instance.ChangeHunger(hungerBonus);
        }
    }

    public void ToggleSleepingState()
    {
        if (PetStateManager.Instance.currentState != PetStateManager.Instance.sleepingState)
        {
            PetStateManager.Instance.ChangeState(PetStateManager.Instance.sleepingState);
        }
        else
        {
            PetStateManager.Instance.ChangeState(PetStateManager.Instance.idleState);
        }
    }
}