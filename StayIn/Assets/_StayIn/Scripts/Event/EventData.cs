using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewEvent", menuName = "StayIn/Event Data")]
public class EventData : ScriptableObject {

    [Header("Basic Info")]
    [SerializeField] private string eventID;
    [SerializeField] private EventInteractionType interactionType;
    [SerializeField] private EventCategory category;
    [SerializeField] private float baseWeight = 1.0f;

    [Header("Character Requirements")]
    [SerializeField] private CharacterRequirement requiredCharacters;

    [Header("Trigger Conditions")]
    [SerializeField] private int minimumDay = 0;
    [SerializeField] private int maximumDay = 999;
    [SerializeField] private int cooldownDays = 3;
    [SerializeField] private bool canTriggerMultipleTimes = true;
    [SerializeField] private List<EventTriggerCondition> triggerConditions;

    [Header("Description & Choices")]
    [SerializeField] private List<LocalizedString> descriptions;
    [SerializeField] private List<EventChoice> eventChoices;

    public string EventID => eventID;
    public EventInteractionType InteractionType => interactionType;
    public EventCategory Category => category;
    public float BaseWeight => baseWeight;
    public CharacterRequirement RequiredCharacters => requiredCharacters;
    public List<LocalizedString> Descriptions => descriptions;
    public List<EventChoice> EventChoices => eventChoices;

    public void SetBaseWeight(float weight) => baseWeight = weight;
    public void AddBaseWeight(float addWeight) => baseWeight += addWeight;

    public int MinimumDay => minimumDay;
    public int MaximumDay => maximumDay;
    public int CooldownDays => cooldownDays;
    public bool CanTriggerMultipleTimes => canTriggerMultipleTimes; 
    public List<EventTriggerCondition> TriggerConditions => triggerConditions;

}