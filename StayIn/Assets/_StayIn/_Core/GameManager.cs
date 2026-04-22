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
                if (instance == null) {
                    Debug.Log("There is no GameManager in Scene.");
                }
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
        ResourceManager.Instance.Init();

        RefreshAllUI();
    }

    public void RefreshAllUI() {
        OnGameStateChanged?.Invoke();
    }

    private void HandleDistributionConfirm(List<DayActionData> actions) {
        ProcessDaySummary(actions);
        DayManager.Instance.NextDay();
        RefreshAllUI();
    }

    private void ProcessDaySummary(List<DayActionData> actionPackets) {
        foreach (var packet in actionPackets) {
            if (packet.isFed && ResourceManager.Instance.RemoveItem("item_01", 1)) {
                packet.character.Eat(5);
            }
            if (packet.isWatered && ResourceManager.Instance.RemoveItem("item_02", 1)) {
                packet.character.Drink(10);
            }
            if (packet.isHealed && ResourceManager.Instance.RemoveItem("item_03", 1)) {
                packet.character.Heal(10);
            }
        }

        DayAction?.Invoke(actionPackets);
    }

    public void OnNextDayButtonClicked() {
        StartCoroutine(FadeManager.Instance.StartFade(OnNextDayConfirm));
    }
}
