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
        EventManager.Instance.Init();

        OnDayChanged?.Invoke();
        RefreshAllUI();
    }

    public void RefreshAllUI() {
        OnGameStateChanged?.Invoke();
    }

    public void OnNextDayButtonClicked() {
        StartCoroutine(FadeManager.Instance.StartFade((Action)(() => {
            DistributionManager.Instance.EndDayConfirm();
            EventOutcome outcome = EventManager.Instance.RollOutcome(LogManager.Instance.CurrentChoice);
            if (outcome != null)
            {
                EventManager.Instance.ApplyOutcome(outcome);
                LogManager.Instance.ResetChoice();
            }
            DayManager.Instance.NextDay();
            LogManager.Instance.GenerateDailyReports(outcome);
            CharacterManager.Instance.DisplayData();
            OnDayChanged?.Invoke();
        })));
    }
}