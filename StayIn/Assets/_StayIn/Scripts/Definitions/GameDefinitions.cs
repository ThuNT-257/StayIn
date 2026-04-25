using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._StayIn.Scripts.Definitions {
    //CHARACTER ZONE
    public static class GameConfig {
        public const int MAX_HUNGER = 10;
        public const int MAX_THIRSTY = 5;
        public const int MAX_HEALTH = 10;
        public const int MAX_SANITY = 10;

        public const int HUNGER_DANGER_LEVEL = 2;
        public const int THIRSTY_DANGER_LEVEL = 1;
        public const int SANITY_DANGER_LEVEL = 0;
    }

    [Serializable]
    public class StatusSpeechGroup {
        [TextArea(2, 4)] public List<string> lines;
        public string GetRandom() {
            return lines.Count > 0 ? lines[UnityEngine.Random.Range(0, lines.Count)] : "";
        }
    }
    //END CHARACTER ZONE

    [Serializable]
    public class CharacterPersonality {
        public StatusSpeechGroup hungerLine;
        public StatusSpeechGroup thirstyLine;
        public StatusSpeechGroup healthLine;
        public StatusSpeechGroup sanityLine;
    }



    [Serializable]
    public class ResourceItem {
        public ItemData itemData;
        public int quantity;
    }

    [Serializable]
    public struct DayActionData {
        public CharacterData character;
        public bool isFed;
        public bool isWatered;
        public bool isHealed;
        public bool isEntertained;
        public string sanityItemID;
    }

    [Serializable]
    public class ResourceChange {
        public string itemID;
        public int amount;
    }

    public enum EventInteractionType {
        YesNo,
        SendSomeone,
        Items,
        ChooseSomeone,
        TradeItem
    }

    public enum EventCategory {
        General,
        Food,
        Water,
        Health,
        Raid
    }

    public enum StatType { Hunger, Thirst, Health }
    public enum TargetGroup { All, Selected, Random }

    [Serializable]
    public class StatusEffect {
        public StatType stat;
        public TargetGroup target;
        public int changeValue;
    }

    [Serializable]
    public class EventOutcome {
        public string outcomeID;
        public bool isSuccess;

        [TextArea(3, 5)]
        public string outcomeText;

        public List<ResourceItem> rewards;
        public List<ResourceItem> penalties;

        public int weight = 10;

        public List<StatusEffect> statusEffects;
    }

    [Serializable]
    public class EventChoice {
        public string choiceID;
        public ChoiceConfig config;

        public ItemData requiredItem;
        public bool requiresCharacter;

        public List<EventOutcome> outcomes;
    }
}
