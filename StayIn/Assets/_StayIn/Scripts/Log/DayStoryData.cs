using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDayStory", menuName = "StayIn/Day Story Data")]
public class DayStoryData : ScriptableObject
{
    [SerializeField] private int dayNumber;
    [SerializeField] [TextArea(10, 20)] private string storyText;

    public int DayNumber => dayNumber;
    public string StoryText => storyText;
}
