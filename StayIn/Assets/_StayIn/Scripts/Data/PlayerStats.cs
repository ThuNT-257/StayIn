using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "StayIn/Player Stats")]
public class PlayerStats : ScriptableObject {
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float smoothTime = 0.05f;
    [SerializeField]
    private int maxCapacity = 4;

    public float MoveSpeed => moveSpeed;
    public float SmoothTime => smoothTime;
    public int MaxCapacity => maxCapacity;
}