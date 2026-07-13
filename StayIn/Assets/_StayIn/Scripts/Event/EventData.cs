using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewEvent", menuName = "StayIn/Event Data")]
public class EventData : ScriptableObject {
    [SerializeField] private string eventID;
    [SerializeField] private EventInteractionType interactionType;
    [SerializeField] private EventCategory category;

    [SerializeField] private int baseWeight = 10;

    [SerializeField] private CharacterRequirement requiredCharacters;

    [SerializeField] private List<LocalizedString> descriptions;
    [SerializeField] private List<EventChoice> eventChoices;

    public string EventID => eventID;
    public EventInteractionType InteractionType => interactionType;
    public EventCategory Category => category;
    public int BaseWeight => baseWeight;
    public CharacterRequirement RequiredCharacters => requiredCharacters;
    public List<LocalizedString> Descriptions => descriptions;
    public List<EventChoice> EventChoices => eventChoices;

    public void SetBaseWeight(int weight) => baseWeight = weight;
    public void AddBaseWeight(int addWeight) => baseWeight += addWeight;
}