using Assets._StayIn.Scripts.Definitions;
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
        int currentFood = ResourceManager.Instance.GetItemQuantity("item_01");
        int currentWater = ResourceManager.Instance.GetItemQuantity("item_02");
        int currentHealth = ResourceManager.Instance.GetItemQuantity("item_03");
        bool isSurvivalFull = (currentWater > 1 && currentFood > 1 && currentHealth > 1);

        foreach(var evt in currentRunEvents) {
            if (evt.Category != EventCategory.Special) {
                evt.SetBaseWeight(1);
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

            List<CharacterData> aliveCharacters = allCharacters.Where(x => x != null && !x.isDead).ToList();


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
                    Debug.Log("Apply effect target Specific " + targetChar.characterName);

                    if (targetChar != null)
                    {
                        ApplyStatChange(targetChar, effect);
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
}
