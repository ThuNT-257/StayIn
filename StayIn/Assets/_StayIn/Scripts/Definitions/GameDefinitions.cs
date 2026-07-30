using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Assets._StayIn.Scripts.Definitions
{
    //CHARACTER ZONE
    public static class GameConfig
    {
        public const int MAX_HUNGER = 10;
        public const int MAX_THIRSTY = 5;
        public const int MAX_HEALTH = 10;
        public const int MAX_SANITY = 10;

        public const int HUNGER_DANGER_LEVEL = 2;
        public const int THIRSTY_DANGER_LEVEL = 1;
        public const int SANITY_DANGER_LEVEL = 0;

        public const int FOOD_RECOVERY = 5;
        public const int WATER_RECOVERY = 5;
        public const int HEALTH_RECOVERY = MAX_HEALTH;
        public const int SANITY_RECOVERY = 2;

        public const int HUNGER_PENALTY = -1;
        public const int THIRSTY_PENALTY = -1;
        public const int HEALTH_PENALTY = -1;
        public const int SANITY_PENALTY = -1;
    }

    [Serializable]
    public class StatusSpeechGroup
    {
        [TextArea(2, 4)] public List<string> lines;
        public string GetRandom()
        {
            return lines.Count > 0 ? lines[UnityEngine.Random.Range(0, lines.Count)] : "";
        }
    }

    [Flags]
    public enum CharacterRequirement
    {
        None = 0,
        Lynx = 1 << 0,
        TrungBienHinh = 1 << 1,
        MadLunaticz = 1 << 2,
        Plinkcanfly = 1 << 3,

        AllCharacters = Lynx | TrungBienHinh | MadLunaticz | Plinkcanfly,
    }
    //END CHARACTER ZONE

    //DISTRIBUTION ZONE
    [Serializable]
    public class ActionPlan
    {
        public bool WillEat = false;
        public bool WillDrink = false;
        public bool WillHeal = false;
        public string SelectedSanityItemID = "";

        public bool IsFoodLocked = false;
        public bool IsWaterLocked = false;
        public bool IsMedLocked = false;
        public bool IsSanityLocked = false;

        public void Reset()
        {
            WillEat = false;
            WillDrink = false;
            WillHeal = false;
            SelectedSanityItemID = "";
            IsFoodLocked = false;
            IsWaterLocked = false;
            IsMedLocked = false;
            IsSanityLocked = false;
        }
    }
    //END DISTRIBUTION ZONE

    //SAVE ZONE
    [System.Serializable]
    public class GameSaveData
    {
        public int currentDay;
        public List<CharacterSaveData> currentCharacters;
        public List<ResourceItemSaveData> currentResourceItems;
        public string mainCharacterId;
    }

    [Serializable]
    public class CharacterSaveData
    {
        public string characterId;
        public int currentHealth;
        public int currentHunger;
        public int currentThirsty;
        public int currentSanity;
        public bool isDead;
    }

    [Serializable]
    public class ResourceItemSaveData
    {
        public string itemId;
        public int quantity;
    }
    //END SAVE ZONE

    [Serializable]
    public class CharacterPersonality
    {
        public StatusSpeechGroup hungerLine;
        public StatusSpeechGroup thirstyLine;
        public StatusSpeechGroup healthLine;
        public StatusSpeechGroup sanityLine;
    }



    [Serializable]
    public class ResourceItem
    {
        public ItemData itemData;
        public int quantity;
    }

    [Serializable]
    public struct DayActionData
    {
        public CharacterData character;
        public bool isFed;
        public bool isWatered;
        public bool isHealed;
        public bool isEntertained;
        public string sanityItemID;
    }

    [Serializable]
    public class ResourceChange
    {
        public string itemID;
        public int amount;
    }

    public enum StatComparison
    {
        LessThan,           // <
        LessThanOrEqual,    // <=
        Equal,              // ==
        GreaterThanOrEqual, // >=
        GreaterThan         // >
    }

    public enum TargetGroup
    {
        All,
        Selected,
        Random
    }

    [Serializable]
    public class StatConditionRule
    {
        public CharacterStatType statType;
        public StatComparison comparison = StatComparison.LessThan;
        public int threshold = 4;
    }

    [Serializable]
    public class StatusEffect
    {
        public CharacterStatType stat;
        public TargetType target;

        public CharacterRequirement specificCharacter;
        public int targetCount = 1;

        public int changeValue;
    }

    [Serializable]
    public class EventOutcome
    {
        public string outcomeID;
        public bool isSuccess;

        public LocalizedString outcomeText;

        public List<ResourceItem> rewards;
        public List<ResourceItem> penalties;

        public int weight = 10;

        public List<StatusEffect> statusEffects;
    }

    [Serializable]
    public class EventChoice
    {
        public ChoiceConfig config;
        public List<EventOutcome> outcomes;
    }

    [Serializable]
    public class EventTriggerCondition
    {
        public enum ConditionType
        {
            HungerLessThan,
            HungerGreaterThan,
            HealthLessThan,
            HealthGreaterThan,
            SanityLessThan,
            SanityGreaterThan,
            ThirstLessThan,
            ThirstGreaterThan,

            HasFood,
            HasWater,
            HasMedicine,
            HasNoFood,
            HasNoWater,
            HasNoMedicine,

            CharacterHasExpeditionToday,
            CharacterHasNotExpeditionToday,
            CharacterIsAlive,
            CharacterIsDead,

            EventHappenedBefore,
            EventNotHappenedBefore,
            DaysSinceLastEvent,

            DayIsEven,
            DayIsOdd,
            RandomCheck,
        }

        public ConditionType type;

        [Header("Values")]
        public int intValue;        
        public float floatValue;    
        public string stringValue;  

        [Header("Target References")]
        public CharacterRequirement targetCharacter; // Chỉ định nhân vật cụ thể cần check status

        public bool negate = false;
    }
}
