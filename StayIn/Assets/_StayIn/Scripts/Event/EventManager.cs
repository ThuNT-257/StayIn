using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance {
        get {
            if(instance == null) {
                instance = FindAnyObjectByType<EventManager>();
                if(instance == null) {
                    Debug.LogError("There is no EventManager in Scene");
                }
            }
            return instance;
        }
    }

    [SerializeField] private List<EventData> allEvents;
    private List<EventData> currentRunEvents = new List<EventData>();
    private List<EventData> specialEvents = new List<EventData>();

    

    private void Awake() {
        if( instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Init() {
        GenerateFilteredEvents();
    }

    public void GenerateFilteredEvents() {
        currentRunEvents.Clear();
        specialEvents.Clear();

        CharacterRequirement currentAliveMask = CharacterRequirement.None;
        foreach (var c in CharacterManager.Instance.GetCharacterList()) {
            if (c.characterName == "Lynx") currentAliveMask |= CharacterRequirement.Lynx;
            if (c.characterName == "TrungBienHinh") currentAliveMask |= CharacterRequirement.TrungBienHinh;
            if (c.characterName == "MadLunaticz") currentAliveMask |= CharacterRequirement.MadLunaticz;
            if (c.characterName == "Plinkcanfly") currentAliveMask |= CharacterRequirement.Plinkcanfly;
        }

        foreach (var evt in allEvents) {
            if (evt.InteractionType != EventInteractionType.YesNo) continue;
            if ((evt.RequiredCharacters & ~currentAliveMask) != CharacterRequirement.None) continue;

            EventData clonedEvent = Instantiate(evt);

            if (clonedEvent.Category == EventCategory.Special) {
                specialEvents.Add(clonedEvent);
            } else {
                clonedEvent.SetBaseWeight(0);
                currentRunEvents.Add(clonedEvent);
            }
        }
        Debug.Log($"[EventManager] Initialized {currentRunEvents.Count} events for this runtime.");
    }

    public void UpdateDynamicWeights() {
        if (currentRunEvents == null || currentRunEvents.Count == 0) return;
        int currentFood = ResourceManager.Instance.GetItemQuantity("item_01");
        int currentWater = ResourceManager.Instance.GetItemQuantity("item_02");
        int currentHealth = ResourceManager.Instance.GetItemQuantity("item_03");
        bool isSurvivalFull = (currentWater > 1 && currentFood > 1 && currentHealth > 1);

        foreach(var evt in currentRunEvents) {
            if (evt.Category != EventCategory.Special) {
                evt.SetBaseWeight(0);
            }

            if (evt.InteractionType == EventInteractionType.YesNo) {
                if (evt.Category == EventCategory.Water && currentWater == 0) {
                    evt.AddBaseWeight(1);
                }
                if (evt.Category == EventCategory.Food && currentFood == 0) {
                    evt.AddBaseWeight(1);
                }
                if (evt.Category == EventCategory.Health && currentHealth == 0) {
                    evt.AddBaseWeight(1);
                }
                if (isSurvivalFull && evt.Category == EventCategory.Raid && DayManager.Instance.CurrentDay > 10) {
                    evt.AddBaseWeight(1);
                }
            }
        }
    }

    public EventData GetEventForToday() {
        UpdateDynamicWeights();
        return GetWeightedRandomEvent(currentRunEvents);
    }

    public EventData GetWeightedRandomEvent(List<EventData> validEvents) {
        if (validEvents == null || validEvents.Count == 0) return null;

        int totalWeight = 0;
        foreach (var evt in validEvents) {
            totalWeight += evt.BaseWeight;
        }

        if (totalWeight <= 0) {
            int randomIndex = Random.Range(0, validEvents.Count);
            return validEvents[randomIndex];
        }

        int randomValue = Random.Range(0, totalWeight);
        int currentWeightSum = 0;

        foreach (var evt in validEvents) {
            currentWeightSum += evt.BaseWeight;
            if (randomValue < currentWeightSum) {
                return evt;
            }
        }

        return validEvents[validEvents.Count - 1];
    }
}
