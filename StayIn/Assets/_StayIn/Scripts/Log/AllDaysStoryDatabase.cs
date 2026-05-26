using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AllDaysStoryDatabase", menuName ="StayIn/All Days Story Database")]
public class AllDaysStoryDatabase : ScriptableObject
{
    [SerializeField] private List<DayStoryData> allStories = new List<DayStoryData>();

    public DayStoryData GetStoryData(int day) {
        return allStories.Find(x => x.DayNumber == day);
    }
}
