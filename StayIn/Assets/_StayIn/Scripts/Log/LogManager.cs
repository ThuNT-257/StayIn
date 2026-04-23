using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    private static LogManager instance;
    public static LogManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<LogManager>();
                if(instance == null)
                {
                    Debug.Log("There is no LogManager in Scene.");
                }
            }
            return instance;
        }
    }


}
