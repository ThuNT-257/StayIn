using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {
    public static CharacterManager Instance;

    [Header("Data")]
    [SerializeField] private List<CharacterData> allCharacters = new List<CharacterData>();

    [Header("Characters")]
    [SerializeField] private CharacterData mainCharacter;
    [SerializeField] private List<CharacterData> otherPool;

    public List<CharacterData> GetCharacterList()
    {
        return allCharacters;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        GameManager.DayAction += ProcessDaySummary;
    }

    private void OnDisable() {
        GameManager.DayAction -= ProcessDaySummary;
    }

    public void Init() {
        GenerateRandomTeam();
    }

    private void GenerateRandomTeam() {
        allCharacters.Clear();

        if(mainCharacter != null ) {
            CharacterData mainInst = Instantiate(mainCharacter);
            mainInst.ResetStats();
            allCharacters.Add(mainInst);
        }

        int extraMemberCount = Random.Range(0, otherPool.Count + 1);
        List<CharacterData> tempPool = new List<CharacterData>(otherPool);

        for(int i = 0; i < extraMemberCount; i++) {
            if (tempPool.Count == 0) break;
            int randomIndex = Random.Range(0, tempPool.Count);
            CharacterData memberInst = Instantiate(tempPool[randomIndex]);
            memberInst.ResetStats();
            allCharacters.Add(memberInst);
            tempPool.RemoveAt(randomIndex);
        }
    }

    public void ProcessDaySummary(List<DayActionData> actions) {
        foreach(DayActionData action in actions) {
            CharacterData character = action.character;
            if(character == null || character.IsDead) {
                continue;
            }

            if (!action.isFed && !character.IsDead && !character.IsExploring) {
                character.Hunger -= 1;
            }

            if (!action.isWatered && !character.IsDead && !character.IsExploring) {
                character.Thirsty -= 1;
            }

            if (!action.isHealed && !character.IsDead && !character.IsExploring && character.Health < 10) {
                character.Health -= 1;
            }

            character.Health = Mathf.Clamp(character.Health, 0, 10);
            character.Hunger = Mathf.Clamp(character.Hunger, 0, 10);
            character.Thirsty = Mathf.Clamp(character.Thirsty, 0, 5);

            if (character.Hunger <= 0 || character.Thirsty <= 0 || character.Health <= 0) {
                character.IsDead = true;
            }
        }
    }
}