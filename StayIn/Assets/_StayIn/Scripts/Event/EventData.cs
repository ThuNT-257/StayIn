using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "StayIn/Event Data")]
public class EventData : ScriptableObject {
    public string eventID;
    public EventInteractionType interactionType;
    public EventCategory category;
    public int baseWeight = 10;

    [TextArea(5, 10)]
    public List<string> descriptions;
    public List<EventChoice> eventChoices;
}
