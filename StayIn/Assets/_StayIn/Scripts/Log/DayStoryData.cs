using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDayStory", menuName = "StayIn/Day Story Data")]
public class DayStoryData : ScriptableObject
{
    public int dayNumber;
    [TextArea(10, 20)] public string storyText;
    public List<ResourceItem> bonusItem;
}
