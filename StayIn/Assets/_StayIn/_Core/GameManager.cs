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

    public static event Action OnDayChanged;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        Debug.Log("[GameManager] Initialized");
    }

    private void OnEnable() {
    }

    private void OnDisable() {
    }

    private void Start() {
        DayManager.Instance.Init();
        CharacterManager.Instance.Init();
        ResourceManager.Instance.Init();
        DistributionManager.Instance.Init();
        LogManager.Instance.Init();

        OnDayChanged?.Invoke();
        RefreshAllUI();
    }

    public void RefreshAllUI() {
        OnGameStateChanged?.Invoke();
    }

    private void HandleDistributionConfirm(List<DayActionData> actions) {
        ProcessDaySummary(actions);

        DayManager.Instance.NextDay();

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
        StartCoroutine(FadeManager.Instance.StartFade((Action)(() => {
            DistributionManager.Instance.EndDayConfirm();
            DayManager.Instance.NextDay();
            OnDayChanged?.Invoke();
        })));
    }
}