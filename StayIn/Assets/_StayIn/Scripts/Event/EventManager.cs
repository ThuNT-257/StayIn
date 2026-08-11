using Assets._StayIn.Scripts.Definitions;
using Assets._StayIn.Scripts.Save;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    /// <summary>
    /// Filters all YesNo events based on alive character requirements. 
    /// Clones valid events; Special events are separated, normal events have base weight set to 0.
    /// </summary>
    public void GenerateFilteredEvents() {
        currentRunEvents.Clear();
        specialEvents.Clear();

        CharacterRequirement currentAliveMask = CharacterRequirement.None;
        foreach (var c in CharacterManager.Instance.GetCharacterList())
        {
            if (c != null && !c.isDead)
            {
                currentAliveMask |= c.RequirementType;
            }
        }

        foreach (var evt in allEvents) {
            if (evt.InteractionType != EventInteractionType.YesNo) continue;
            if ((evt.RequiredCharacters & ~currentAliveMask) != CharacterRequirement.None) continue;

            EventData clonedEvent = Instantiate(evt);

            if (clonedEvent.Category == EventCategory.Special) {
                specialEvents.Add(clonedEvent);
            } else {
                currentRunEvents.Add(clonedEvent);
            }
        }
        Debug.Log($"[EventManager] Initialized {currentRunEvents.Count} events for this runtime.");
    }

    public void UpdateDynamicWeights() {
        if (currentRunEvents == null || currentRunEvents.Count == 0) return;
        int totalFood = ResourceManager.Instance.GetItemQuantityByItemType(ItemType.Food);
        int totalWater = ResourceManager.Instance.GetItemQuantityByItemType(ItemType.Water);
        int totalMedicine = ResourceManager.Instance.GetItemQuantityByItemType(ItemType.Medicine);
        bool isSurvivalFull = (totalWater > 0 && totalFood > 0 && totalMedicine > 0);

        foreach(var evt in currentRunEvents) {
            if (evt.Category != EventCategory.Special)
            {
                evt.SetBaseWeight(evt.BaseWeight);
            }

            if (evt.InteractionType == EventInteractionType.YesNo) {
                switch (evt.Category)
                {
                    case EventCategory.Water:
                        if (totalWater == 0) evt.AddBaseWeight(1);
                        break;

                    case EventCategory.Food:
                        if (totalFood == 0) evt.AddBaseWeight(1);
                        break;

                    case EventCategory.Health:
                        if (totalMedicine == 0) evt.AddBaseWeight(1);
                        break;

                    case EventCategory.Raid:
                        if (isSurvivalFull && DayManager.Instance.CurrentDay > 10) evt.AddBaseWeight(1);
                        break;
                }
            }
        }
    }

    public EventData GetEventForToday() {
        List<EventData> validEvents = GetCurrentValidEvents();
        UpdateDynamicWeights();

        return GetWeightedRandomEvent(validEvents);
    }

    public EventData GetWeightedRandomEvent(List<EventData> validEvents)
    {
        if (validEvents == null || validEvents.Count == 0) return null;

        float totalWeight = 0;
        foreach (var evt in validEvents)
        {
            totalWeight += evt.BaseWeight;
        }

        if (totalWeight <= 0)
        {
            int randomIndex = Random.Range(0, validEvents.Count);
            return validEvents[randomIndex];
        }

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeightSum = 0f;

        foreach (var evt in validEvents)
        {
            currentWeightSum += evt.BaseWeight;
            if (randomValue <= currentWeightSum)
            {
                return evt;
            }
        }

        return validEvents[validEvents.Count - 1];
    }

    public EventOutcome RollOutcome(EventChoice choice = null)
    {
        if(choice == null)
        {
            Debug.Log("No choice chosen");
            return null;
        }

        if (choice.outcomes == null || choice.outcomes.Count == 0)
        {
            return null;
        }

        int totalWeight = 0;
        foreach (EventOutcome outcome in choice.outcomes)
        {
            totalWeight += outcome.weight;
        }

        if (totalWeight <= 0)
        {
            int randomNum = UnityEngine.Random.Range(0, choice.outcomes.Count);
            return choice.outcomes[randomNum];
        }

        int roll = UnityEngine.Random.Range(0, totalWeight);
        int currentWeight = 0;
        foreach (EventOutcome outcome in choice.outcomes)
        {
            currentWeight += outcome.weight;
            if (roll < currentWeight)
            {
                return outcome;
            }
        }

        return choice.outcomes[choice.outcomes.Count - 1];
    }

    public void ApplyOutcome(EventOutcome outcome)
    {
        if(outcome == null)
        {
            Debug.Log("[EventManager] No outcome today");
            return;
        }

        if(outcome.rewards != null && outcome.rewards.Count > 0)
        {
            foreach(ResourceItem reward in outcome.rewards)
            {
                if(reward != null && reward.itemData != null && reward.quantity > 0)
                {
                    ResourceManager.Instance.AddItem(reward.itemData, reward.quantity);
                }
            } 
        }

        if(outcome.penalties != null && outcome.penalties.Count > 0)
        {
            foreach(ResourceItem penalty in outcome.penalties)
            {
                if(penalty != null && penalty.itemData != null && penalty.quantity > 0)
                {
                    ResourceManager.Instance.RemoveItem(penalty.itemData, penalty.quantity);
                }
            }
        }

        if(outcome.statusEffects != null && outcome.statusEffects.Count > 0)
        {
            List<CharacterData> allCharacters = CharacterManager.Instance.GetCharacterList();
            if (allCharacters == null)
            {
                Debug.Log("[EventManager] - ApplyOutcome - Character list not found");
                return;
            }

            List<CharacterData> aliveCharacters = CharacterManager.Instance.GetInRoomCharacterList();


            foreach (StatusEffect effect in outcome.statusEffects)
            {
                if(effect == null)
                {
                    continue;
                }

                if (effect.target == TargetType.All)
                {
                    Debug.Log("Apply effect target All");
                    foreach (CharacterData character in aliveCharacters)
                    {
                        ApplyStatChange(character, effect);
                    }
                }
                else if (effect.target == TargetType.Random)
                {
                    Debug.Log("Apply effect target Random 1");
                    if (aliveCharacters.Count > 0)
                    {
                        CharacterData randomChar = aliveCharacters[Random.Range(0, aliveCharacters.Count)];
                        ApplyStatChange(randomChar, effect);
                    }
                }
                else if (effect.target == TargetType.RandomMultiple)
                {
                    Debug.Log("Apply effect target Random " + effect.targetCount);
                    if (aliveCharacters.Count > 0)
                    {
                        int countToTake = Mathf.Clamp(effect.targetCount, 1, aliveCharacters.Count);
                        List<CharacterData> randomCharacters = ListExtensions.TakeRandomByCount(aliveCharacters, countToTake);

                        foreach (CharacterData character in randomCharacters)
                        {
                            ApplyStatChange(character, effect);
                        }
                    }
                }
                else if (effect.target == TargetType.Specific)
                {

                    CharacterData targetChar = aliveCharacters.FirstOrDefault(x => (x.RequirementType & effect.specificCharacter) != CharacterRequirement.None);

                    if (targetChar != null)
                    {
                        ApplyStatChange(targetChar, effect);
                        Debug.Log("Apply effect target Specific " + targetChar.characterName);
                    }
                }
            }
        }
    }

    private void ApplyStatChange(CharacterData character, StatusEffect effect)
    {
        character.UpdateStats(effect.stat == CharacterStatType.Health ? effect.changeValue : 0,
        effect.stat == CharacterStatType.Hunger ? effect.changeValue : 0,
        effect.stat == CharacterStatType.Thirst ? effect.changeValue : 0,
        effect.stat == CharacterStatType.Sanity ? effect.changeValue : 0);
    }

    private List<EventData> GetCurrentValidEvents()
    {
        if(currentRunEvents == null || currentRunEvents.Count == 0)
        {
            return null;
        }

        List<EventData> res = new List<EventData>();

        foreach(EventData e in currentRunEvents)
        {
            if(IsValidEvent(e))
            {
                res.Add(e);
            }
        }
        return res;
    }

    private bool IsValidEvent(EventData e)
    {
        //check minimum day
        int day = DayManager.Instance.CurrentDay;
        if(day < e.MinimumDay || day > e.MaximumDay)
        {
            return false;
        }

        //check multiple used events and cool down
        Dictionary<int, string> usedEvents = SaveManager.Instance.UsedEvent;

        if (usedEvents.ContainsValue(e.EventID))
        {
            if (!e.CanTriggerMultipleTimes) {
                return false;
            }

            if(e.CooldownDays > 0) {
                int lastUsedDay = usedEvents.Where(x => x.Value == e.EventID).Max(x => x.Key);

                if(DayManager.Instance.CurrentDay - lastUsedDay < e.CooldownDays) {
                    return false;
                }
            }
        }

        if(e.TriggerConditions != null && e.TriggerConditions.Count > 0)
        {
            //check trigger condition
            foreach (EventTriggerCondition condition in e.TriggerConditions)
            {
                if(!CheckCondition(condition.type, condition.intValue, condition.floatValue, condition.stringValue)) {
                    return false;
                }
            }
        }

        return true;
    }

    private bool CheckCondition(ConditionType type, int intValue = 0, float floatValue = 0, string stringValue = null)
    {
        switch (type)
        {
            case ConditionType.HungerLessThan:
                List<CharacterData> inRoomCharacters = CharacterManager.Instance.GetInRoomCharacterList();
                foreach(CharacterData character in inRoomCharacters)
                {
                    if(character.Hunger < intValue)
                    {
                        return true;
                    }
                }
                return false;

            default:
                break;
        }
        return false;
    }
}
