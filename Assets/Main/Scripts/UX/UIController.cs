using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

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

    // [Header("Stat UI")]
    // public Slider hungerSlider;
    // public Slider sleepSlider;
    // public Slider happinessSlider;

    [Header("Feeding Items")]
    public List<FoodItem> foodItems;
    public GameObject foodButtonPrefab;
    public Transform foodButtonContainer;

    public GameObject feedingCancelZone;
    // public Image feedingCancelZoneImage;


    private PetStateManager petStateManager;
    private GameObject spawnedFood;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

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

    public void LookAtFood(Transform food)
    {
        MultiAimConstraint multiAimConstraint = FindObjectOfType<MultiAimConstraint>();
        RigBuilder rigBuilder = FindObjectOfType<RigBuilder>();

        if (multiAimConstraint == null || rigBuilder == null)
        {
            Debug.LogWarning("Constraint or target is missing.");
            return;
        }

        multiAimConstraint.data.sourceObjects = functionGetSources(food);
        multiAimConstraint.weight = 1f;
        rigBuilder.Build();

        Debug.Log("we found multiAimConstraint");
    }

    public WeightedTransformArray functionGetSources(Transform target)
    {
        WeightedTransformArray sources = new WeightedTransformArray();
        sources.Add(new WeightedTransform(target, 1f));
        return sources;
    }


    private void ShowFood(FoodItem food)
    {
        petStateManager = FindObjectOfType<PetStateManager>();

        HideFood();

        if (petStateManager != null)
        {
            if (food.prefab != null)
            {
                spawnedFood = Instantiate(food.prefab, petStateManager.transform.position + Vector3.forward, Quaternion.identity);
            }
        }
    }

    public void HideFood()
    {
        if (spawnedFood != null)
        {
            LookAtFood(null);
            Destroy(spawnedFood);
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

    // public bool IsPointerOverCancelZone(Vector2 screenPosition)
    // {
    //     return RectTransformUtility.RectangleContainsScreenPoint(feedingCancelZone, screenPosition, Camera.main);
    // }
    
    // public void CancelZoneDragged(int hungerBonus)
    // {

    // }
}