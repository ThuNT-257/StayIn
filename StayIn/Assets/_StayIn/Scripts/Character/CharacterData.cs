using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "StayIn/Character Data")]
public class CharacterData : ScriptableObject {
    [Header("Basic Info")]
    [SerializeField] private string characterID;
    [SerializeField] private string characterName;
    [SerializeField] private Sprite avatarNormal;
    [SerializeField] private Sprite avatarHungryOrThirsty;
    [SerializeField] private Sprite avatarStarving;
    [SerializeField] private Sprite avatarSick;
    [SerializeField] private Sprite avatarDead;

    [Header("Stats")]
    [SerializeField][Range(0, 10)] private int health = 10;
    [SerializeField][Range(0, 10)] private int hunger = 10;
    [SerializeField][Range(0, 5)] private int thirsty = 5;

    [Header("Status")]
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isExploring = false;
    [SerializeField] private int daysToReturn = 0;

    public string CharacterName => characterName;
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, 10); }
    public int Hunger { get => hunger; set => hunger = Mathf.Clamp(value, 0, 10); }
    public int Thirsty { get => thirsty; set => thirsty = Mathf.Clamp(value, 0, 5); }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsExploring { get => isExploring; set => isExploring = value; }
    public int DaysToReturn { get => daysToReturn; set => daysToReturn = value; }

    public void ResetStats() {
        health = 10;
        hunger = 10;
        thirsty = 5;
        isDead = false;
        isExploring = false;
        daysToReturn = 0;
    }

    public Sprite GetCurrentAvatar() {
        if (isDead) {
            return avatarDead;
        }
        if (health < 10) {
            return avatarSick;
        }
        if (hunger < 2 || thirsty < 2) {
            return avatarStarving;
        }
        if(hunger < 6 || thirsty < 4) {
            return avatarHungryOrThirsty;
        }
        return avatarNormal;
    }

    public void Eat(int amount) {
        if(IsDead || isExploring) {
            return;
        }
        Hunger = Mathf.Clamp(Hunger + amount, 0, 10);
    }

    public void Drink(int amount) {
        if(isDead || isExploring) {
            return;
        }
        Thirsty = Mathf.Clamp(Thirsty + amount, 0, 5);
    }

    public void Heal(int amount) {
        if (IsDead || isExploring) {
            return;
        }
        Health = Mathf.Clamp(Health + amount, 0, 10);
    }

    public void HandleDailyStatus(bool isFed, bool isWatered, bool isHealed) {
        if(IsDead || IsExploring) {
            return;
        }

        if (!isFed) {
            Hunger -= 1;
        }

        if (!isWatered) {
            Thirsty -= 1;
        }

        if (!isHealed && Health < 10) Health -= 1;

        Hunger = Mathf.Clamp(Hunger, 0, 10);
        Thirsty = Mathf.Clamp(Thirsty, 0, 5);
        Health = Mathf.Clamp(Health, 0, 10);

        if(Hunger <= 0 || Thirsty <= 0 || Health <= 0) {
            IsDead = true;
        }
    }
}