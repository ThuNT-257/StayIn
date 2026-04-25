using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
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
        //InitMockData();
    }

    public void Init() {
        EventManager.Instance.DetermineDailyEvent(DayManager.Instance.CurrentDay);
        GenerateDailyReports();
    }

    private void InitMockData() {
        dailySummaryLogs.Clear();
        dailySummaryLogs.Add("Another sleepless night in the shelter. The distant groans of the undead still echo through the ventilation shafts, keeping everyone on edge.");
        dailySummaryLogs.Add("Our food supplies are dwindling faster than anticipated. If we don't find a stable source of nutrients soon, the hunger will become as dangerous as the monsters outside.");
        dailySummaryLogs.Add("Ted is showing signs of severe exhaustion. He spent most of the morning staring at a rusty pipe, claiming he could hear it whispering the coordinates to a secret stash.");
        dailySummaryLogs.Add("Dolores managed to patch up the leak in the water tank, but we lost nearly three liters of clean water during the process. Every drop is precious now.");
        dailySummaryLogs.Add("Around midnight, someone—or something—started pounding on the heavy steel door. We stayed silent, holding our breath in the dark.");
        dailySummaryLogs.Add("We found deep scratch marks on the outer hull this morning. Whatever was out there has long, sharp claws.");
        dailySummaryLogs.Add("Mary Jane returned from the abandoned pharmacy nearby. She looks pale and hasn't spoken a word since she crawled back through the hatch.");
        dailySummaryLogs.Add("She managed to bring back a small medical kit and some painkillers, but she lost her flashlight in the struggle.");
    }

    public void GenerateDailyReports() {
        dailySummaryLogs.Clear();

        //Day Story Part
        DayStoryData todayStory = storyDatabase.Find(s => s.dayNumber == DayManager.Instance.CurrentDay);
        if (todayStory != null) {
            StringBuilder sb = new StringBuilder();
            sb.Append(todayStory.storyText);
            if(todayStory.bonusItem != null && todayStory.bonusItem.Count > 0) {
                foreach(ResourceItem item in todayStory.bonusItem) {
                    sb.Append($"\n+ {item.quantity} {item.itemData.ItemName}");
                }
            }
            dailySummaryLogs.Add(sb.ToString());
        }

        //Event Part


    }
}
