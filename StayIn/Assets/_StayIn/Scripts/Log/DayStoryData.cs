using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class DayStoryData
{
    [SerializeField] private int dayNumber;
    [SerializeField] private LocalizedString storyText;

    public int DayNumber => dayNumber;
    public LocalizedString StoryText => storyText;
}
