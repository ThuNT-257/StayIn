using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "StayIn/Event")]
public class EventData : ScriptableObject
{
    private string eventID;
    private string eventTitle;
    [TextArea]
    private string eventDescription;

    private List<EventChoice> choices;
    private EventChoice defaultChoice;
}
