using Assets._StayIn.Scripts.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    private static LogManager instance;
    public static LogManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<LogManager>();
                if(instance == null)
                {
                    Debug.Log("There is no LogManager in Scene.");
                }
            }
            return instance;
        }
    }

    [SerializeField] private List<DayStoryData> storyDatabase;

    //Page_1_Summary
    private List<string> dailySummaryLogs = new List<string>();

    //Page_2_Event
    private EventData currentEvent;
    private string selectedChoiceID;
    private Dictionary<string, bool> choiceResults = new Dictionary<string, bool>();

    //Page_3_Expedition
    private List<CharacterData> availableCharacters = new List<CharacterData>();
    private List<ResourceItem> availableItems = new List<ResourceItem>();
    private string selectedExpeditionCharID;
    private List<string> selectedExpeditionItemID = new List<string>();
    private bool isExpeditionLocked;

    //Page_4_Status
    private Dictionary<string, List<string>> characterStatuses = new Dictionary<string, List<string>>();

    public List<DayStoryData> StoryDatabase => storyDatabase;
    public List<string> DailySummaryLogs => dailySummaryLogs;
    public bool IsExpeditionLocked => isExpeditionLocked;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Init() {
        GenerateDailyReports();
    }

    public void GenerateDailyReports() {
        //Day Story Part
        dailySummaryLogs.Clear();
        int day = DayManager.Instance.CurrentDay;
        dailySummaryLogs.Add(GenerateStoryPart(day));
    }

    private string GenerateStoryPart(int day) {
        StringBuilder sb = new StringBuilder();

        DayStoryData todayStory = storyDatabase.Find(s => s.DayNumber == day);
        if(todayStory != null) {
            sb.Append(todayStory.StoryText);
        } else {
            sb.Append("Everything is weirdly peaceful!\nToo peaceful, honestly. Feels like the calm before something awful.");
        }

        if (day == 1) {
            List<ResourceItem> bonusItem = ResourceManager.Instance.GetStartingBonusItem();
            foreach (ResourceItem item in bonusItem) {
                sb.Append($"\n+ {item.quantity} {item.itemData.ItemName}");
            }
        }
        return sb.ToString();
    }
}
