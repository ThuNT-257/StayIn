using Assets._StayIn.Scripts.Definitions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance {
        get {
            if(instance == null) {
                instance = FindAnyObjectByType<GameManager>();
                if(instance == null ) {
                    Debug.Log("There is no GameManager in Scene.");
                }
            }
            return instance;
        }
    }

    public static event Action<List<DayActionData>> DayAction;
    public static event Action OnGameStateChanged;

    private void Awake()
    {
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void OnEnable() {
        DayManager.OnDayChanged += ProcessDaySummary;
    }

    private void OnDisable() {
        DayManager.OnDayChanged -= ProcessDaySummary;
    }

    private void Start() {
        DayManager.Instance.Init();
        CharacterManager.Instance.Init();
        ResourceManager.Instance.Init();

        DistributionPanelUI.Instance.OpenPanel();
        RefreshAllUI();
    }

    public void RefreshAllUI() {
        OnGameStateChanged?.Invoke();
        //DistributionPanelUI.Instance.DisplayDistributionList();
        //ResourcePanelUI.Instance.DisplayResourceList();
    }

    public void ProcessDaySummary(int nextDay) {
        if (nextDay == 1) return;
        List<DistributedItemUi> distributionList = DistributionPanelUI.Instance.GetCurrenDistributionList();
        List<DayActionData> actionPackets = new List<DayActionData>();

        foreach(DistributedItemUi itemUI in distributionList) {
            if (!itemUI.gameObject.activeSelf) {
                continue;
            }

            bool fed = false;
            bool watered = false;
            bool healed = false;

            if (itemUI.WillEat && ResourceManager.Instance.RemoveItem("item_01", 1)) {
                itemUI.CurrentCharacter.Eat(5);
                fed = true;
            }

            if (itemUI.WillDrink && ResourceManager.Instance.RemoveItem("item_02", 1)) {
                itemUI.CurrentCharacter.Drink(10);
                watered = true;
            }
            if (itemUI.WillHeal && ResourceManager.Instance.RemoveItem("item_03", 1)) {
                itemUI.CurrentCharacter.Heal(10);
                healed = true;
            }

            actionPackets.Add(new DayActionData {
                character = itemUI.CurrentCharacter,
                isFed = fed,
                isWatered = watered,
                isHealed = healed,
            });
        }

        DayAction?.Invoke(actionPackets);
        RefreshAllUI();
    }

    public void OnNextDayButtonClicked() {
        StartCoroutine(FadeManager.Instance.StartFade(DayManager.Instance.NextDay));
    }
}
