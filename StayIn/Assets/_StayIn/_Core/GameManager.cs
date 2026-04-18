using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] private ResourcePanelUI resourceUI;
    [SerializeField] private DistributionPanelUI distributionUI;

    private int currentDay = 1;

    public int GetCurrentDay() => currentDay;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(InitializeGameRoutine());
    }

    private System.Collections.IEnumerator InitializeGameRoutine()
    {
        yield return new WaitForEndOfFrame();
        distributionUI.OpenPanel();
    }

    public void EndDay()
    {
        CharacterManager.Instance.ProcessNewDay();

        currentDay++;

        resourceUI.DisplayResourceList();
        distributionUI.DisplayDistributionList();
    }
}
