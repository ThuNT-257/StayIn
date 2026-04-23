using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._StayIn.Scripts.Definitions {

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
    }

    [Serializable]
    public class ResourceChange {
        public string itemID;
        public int amount;
    }

    [Serializable]
    public class EventOutcome {
        [TextArea]
        public string outcomeText;
        public List<ResourceChange> resourceChanges;
        public int selectedCharHealthChange;
    }

    [Serializable]
    public class EventChoice {
        public string choiceID;
        public Sprite choiceIcon;

        public bool requireCharacter;
        public string requiredItemID;

        public bool consumItems;
        public int durabilityCost = 1;

        public EventOutcome successOutcome;
        public EventOutcome failOutcome;
    }
}
