using System.Collections.Generic;
using UnityEngine;

public enum EventType { YesNo, SendSomeone, UseItem, Trade, MultiChoice }

[CreateAssetMenu(fileName = "NewEvent", menuName = "StayIn/Game Event")]
public class GameEvent : ScriptableObject {
    public EventType type;
    [TextArea] public string description;

    public List<string> options;
}