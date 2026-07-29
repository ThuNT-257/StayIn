using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{
    ///<summary>
    /// Take random elements from list by Fisher-Yates Shuffle algorithm with number of count
    ///</summary>
    public static List<T> TakeRandomByCount<T>(this List<T> list, int count)
    {
        if(list == null || list.Count == 0 || count <= 0)
        {
            return new List<T>();
        }

        int actualCount = Mathf.Min(list.Count, count);

        List<T> result = list.ToList();

        for (int i = 0; i < actualCount; i++)
        {
            int j = Random.Range(i, result.Count);
            (result[i], result[j]) = (result[j], result[i]);
        }

        return result.Take(actualCount).ToList();
    }
}
