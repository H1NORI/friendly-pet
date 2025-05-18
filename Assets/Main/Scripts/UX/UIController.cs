using UnityEngine;

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

    private PetStateManager petStateManager;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        petStateManager = FindObjectOfType<PetStateManager>();

        HideAllPanels();
    }

    public void ShowFeedingUI()
    {
        feedingPanel.SetActive(true);
    }

    public void ShowPlayUI()
    {
        activityPanel.SetActive(true);
    }

    public void HideAllPanels()
    {
        feedingPanel.SetActive(false);
        activityPanel.SetActive(false);
    }

    // public void UpdateStats(float hunger, float sleep, float happiness)
    // {
    //     hungerSlider.value = hunger;
    //     sleepSlider.value = sleep;
    //     happinessSlider.value = happiness;
    // }

    // Button click hooks:
    // public void OnFeedBerry(int berryIndex)
    // {
    //     PetStateManager.Instance.TryFeedPet(berryIndex); // Tell manager to handle feeding logic
    // }

    // public void OnPlayWithToy(int toyIndex)
    // {
    //     PetStateManager.Instance.TryPlayWithPet(toyIndex);
    // }
}
