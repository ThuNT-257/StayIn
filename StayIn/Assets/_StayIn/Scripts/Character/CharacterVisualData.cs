using UnityEngine;

[CreateAssetMenu(fileName = "NewVisualData", menuName = "StayIn/Character Visual Data")]
public class CharacterVisualData : ScriptableObject {
    [Header("Basic Avatars")]
    public Sprite normal;
    public Sprite sick;     
    public Sprite starved;  
    public Sprite insane;   
    public Sprite dead;     
    public Sprite exploring; 
}