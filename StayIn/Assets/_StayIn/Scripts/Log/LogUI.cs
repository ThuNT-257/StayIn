using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour {

    [SerializeField] private GameObject[] pages;
    private int currentPageIndex = 0;

    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private bool hasEventToday = false;
    private bool isExpeditionOpen = false;

    private void OnEnable() {
        GameManager.OnGameStateChanged += UpdateLogContent;
        GameManager.OnDayChanged += UpdateLogContent;
    }

    private void Start() {
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();

        leftButton.onClick.AddListener(() => OnLeftButtonClicked());
        rightButton.onClick.AddListener(() => OnRightButtonClicked());
        UpdateLogContent();
    }

    private void OnDisable() {
        GameManager.OnGameStateChanged -= UpdateLogContent;
        GameManager.OnDayChanged -= UpdateLogContent;
    }

    private void UpdateLogContent() {
        //Page1_Summary
        LogPageSummaryUI summaryPage = pages[0].GetComponent<LogPageSummaryUI>();
        if (summaryPage != null) {
            summaryPage.Setup(LogManager.Instance.DailySummaryLogs);
        }

        //Page2_Event
        EventData currentEvent = LogManager.Instance.CurrentEvent;
        hasEventToday = (currentEvent != null);

        if (hasEventToday && pages.Length > 1) {
            pages[1].GetComponent<LogPageEventUI>().SetUp(currentEvent);
        }

        int currentDay = DayManager.Instance.CurrentDay;
        isExpeditionOpen = (currentDay > 1) && !LogManager.Instance.IsExpeditionLocked;

        ShowPage(0);
        UpdateNavigationButtons();
    }

    public void ShowPage(int index) {
        currentPageIndex = index;

        for (int i = 0; i < pages.Length; i++) {
            pages[i].SetActive(i == currentPageIndex);
        }

        leftButton.gameObject.SetActive(currentPageIndex > 0);
        rightButton.gameObject.SetActive(currentPageIndex < pages.Length - 1);
    }

    public void OnLeftButtonClicked() {
        LogPageSummaryUI currentPage = pages[currentPageIndex].GetComponent<LogPageSummaryUI>();
        if (currentPage != null && currentPage.CanFlipBack()) {
            currentPage.FlipBack();
            UpdateNavigationButtons();
            return;
        }

        int prevIndex = currentPageIndex - 1;

        if (prevIndex == 2 && !isExpeditionOpen) {
            prevIndex--;
        }

        if (prevIndex == 1 && !hasEventToday) {
            prevIndex--;
        }

        if (prevIndex >= 0) {
            ShowPage(prevIndex);
            LogPageSummaryUI prevPage = pages[prevIndex].GetComponent<LogPageSummaryUI>();
            if (prevPage != null) {
                prevPage.FlipToLast();
                UpdateNavigationButtons();
            }
        }
    }

    public void OnRightButtonClicked() {
        var currentPage = pages[currentPageIndex].GetComponent<LogPageSummaryUI>();
        if (currentPage != null && currentPage.CanFlipNext()) {
            currentPage.FlipNext();
            UpdateNavigationButtons();
            return;
        }

        int nextIndex = currentPageIndex + 1;

        if (nextIndex == 1 && !hasEventToday) {
            nextIndex++;
        }

        if (nextIndex == 2 && !isExpeditionOpen) {
            nextIndex++;
        }

        if (nextIndex < pages.Length) {
            ShowPage(nextIndex);
        }
    }

    private void UpdateNavigationButtons() {
        LogPageSummaryUI currentPage = pages[currentPageIndex].GetComponent<LogPageSummaryUI>();

        bool canFlipBack = (currentPageIndex > 0) || (currentPage != null && currentPage.CanFlipBack());
        leftButton.gameObject.SetActive(canFlipBack);

        bool canFlipNext = (currentPageIndex < pages.Length - 1) || (currentPage != null && currentPage.CanFlipNext());
        rightButton.gameObject.SetActive(canFlipNext);
    }
}
