using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._StayIn.Scripts.Definitions
{
    public enum EventInteractionType
    {
        YesNo,
        SendSomeone,
        Items,
        ChooseSomeone,
        TradeItem
    }

    public enum EventCategory
    {
        General,
        Food,
        Water,
        Health,
        Raid,
        Special
    }

    public enum TargetType
    {
        All,
        Random,
        RandomMultiple,
        Specific,
        Self
    }

    public enum ConditionTargetType
    {
        Resource,       
        CharacterStat, 
        CharacterStatus,
        EventHistory,   
        GameState       
    }

    public enum ComparisonOperator
    {
        Equal,              
        NotEqual,           
        GreaterThan,        
        GreaterThanOrEqual, 
        LessThan,           
        LessThanOrEqual     
    }

    [Serializable]
    public class EventCondition
    {
        public ConditionTargetType targetType;
        public string targetID;
        public ComparisonOperator op;
        public int value;
        public string characterName;
    }
}
