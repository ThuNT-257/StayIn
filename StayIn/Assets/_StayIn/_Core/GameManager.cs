using Assets._StayIn.Scripts.Definitions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = FindAnyObjectByType<GameManager>();
            }
            return instance;
        }
    }

    public static event Action<List<DayActionData>> DayAction;
    public static event Action OnGameStateChanged;
    public static event Action OnNextDayConfirm;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void OnEnable() {
        DistributionPanelUI.OnDistributionConfirmChanged += HandleDistributionConfirm;
    }

    private void OnDisable() {
        DistributionPanelUI.OnDistributionConfirmChanged -= HandleDistributionConfirm;
    }

    private void Start() {
        DayManager.Instance.Init();
        CharacterManager.Instance.Init();
        LogManager.Instance.Init();
        ResourceManager.Instance.Init();

        Debug.Log("CharacterCount = " + CharacterManager.Instance.GetCharacterList().Count);

        RefreshAllUI();
    }

    public void RefreshAllUI() {
        OnGameStateChanged?.Invoke();
    }

    private void HandleDistributionConfirm(List<DayActionData> actions) {
        ProcessDaySummary(actions);

        DayManager.Instance.NextDay();

        EventManager.Instance.DetermineDailyEvent(DayManager.Instance.CurrentDay);
        ResourceManager.Instance.ApplyStoryBonus(DayManager.Instance.CurrentDay, LogManager.Instance.StoryDatabase);

        LogManager.Instance.GenerateDailyReports();

        RefreshAllUI();
    }

    private void ProcessDaySummary(List<DayActionData> actionPackets) {
        foreach (var packet in actionPackets) {
            if (packet.character == null || packet.character.isDead) continue;

            if (packet.isFed && ResourceManager.Instance.RemoveItem("item_01", 1)) {
            }

            if (packet.isWatered && ResourceManager.Instance.RemoveItem("item_02", 1)) {
            }

            if (packet.isHealed && ResourceManager.Instance.RemoveItem("item_03", 1)) {
            }

            if (packet.isEntertained && !string.IsNullOrEmpty(packet.sanityItemID)) {
                ResourceManager.Instance.RemoveItem(packet.sanityItemID, 1);
            }
        }

        DayAction?.Invoke(actionPackets);
    }

    public void OnNextDayButtonClicked() {
        DistributionPanelUI panel = FindFirstObjectByType<DistributionPanelUI>();

        if (panel != null) {
            StartCoroutine(FadeManager.Instance.StartFade(() => {
                panel.OnConfirmDistribution();
            }));
        } else {
            Debug.LogError("Không tìm thấy DistributionPanelUI trong Scene!");
        }
    }
}