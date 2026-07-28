using Assets._StayIn.Scripts.Definitions;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "StayIn/Character Data")]
public class CharacterData : ScriptableObject {
    public string characterID;
    public string characterName;

    [SerializeField] private int health = 10;
    [SerializeField] private int hunger = 10;
    [SerializeField] private int thirsty = 5;
    [SerializeField] private int sanity = 10;

    public CharacterVisualData visuals;
    public CharacterPersonality personality;

    public bool isDead;
    public bool isExploring;
    [SerializeField] private bool deathLogged;

    public string Name => characterName;
    public int Health => health;
    public int Hunger => hunger;
    public int Thirsty => thirsty;
    public int Sanity => sanity;
    public bool DeathLogged => deathLogged;

    public void SetDeathLogged(bool check) {
        deathLogged = check;
    }

    public void ResetStats() {
        health = GameConfig.MAX_HEALTH;
        hunger = GameConfig.MAX_HUNGER;
        thirsty = GameConfig.MAX_THIRSTY;
        sanity = GameConfig.MAX_SANITY;
        isDead = false;
        isExploring = false;
        deathLogged = false;
    }

    public void UpdateStats(int health1 = 0, int hunger1 = 0, int thirsty1 = 0, int sanity1 = 0) {
        if (isDead) return;

        health = Mathf.Clamp(health + health1, 0, GameConfig.MAX_HEALTH);
        hunger = Mathf.Clamp(hunger + hunger1, 0, GameConfig.MAX_HUNGER);
        thirsty = Mathf.Clamp(thirsty + thirsty1, 0, GameConfig.MAX_THIRSTY);
        sanity = Mathf.Clamp(sanity + sanity1, 0, GameConfig.MAX_SANITY);

        if (health <= 0 || hunger <= 0 || thirsty <= 0) {
            isDead = true;
        }
    }

    public Sprite GetCurrentAvatar() {
        if (visuals == null) return null;

        if (isDead) return visuals.dead;
        if (isExploring) return visuals.exploring;

        if (health <= 3) return visuals.sick;

        if (hunger <= GameConfig.HUNGER_DANGER_LEVEL || thirsty <= GameConfig.THIRSTY_DANGER_LEVEL)
            return visuals.starved;

        if (sanity <= GameConfig.SANITY_DANGER_LEVEL) return visuals.insane;

        return visuals.normal;
    }

    public string GetDailyStatusLine() {

        if (isDead) {
            if (!deathLogged) {
                deathLogged = true;
                return $"{characterName} passed away last night.";
            }
            return null;
        }

        if (isExploring) return null;

        if (personality == null) return $"{characterName} is here.";

        if (sanity <= GameConfig.SANITY_DANGER_LEVEL)
            return personality.sanityLine.GetRandom().Replace("{n}", characterName);

        if (health <= 3)
            return personality.healthLine.GetRandom().Replace("{n}", characterName);

        if (thirsty <= GameConfig.THIRSTY_DANGER_LEVEL)
            return personality.thirstyLine.GetRandom().Replace("{n}", characterName);

        if (hunger <= GameConfig.HUNGER_DANGER_LEVEL)
            return personality.hungerLine.GetRandom().Replace("{n}", characterName);

        return $"{characterName} seems to be doing fine today.";
    }
}