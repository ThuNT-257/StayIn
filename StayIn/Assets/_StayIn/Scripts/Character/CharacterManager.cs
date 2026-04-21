using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {
    private static CharacterManager instance;

    public static CharacterManager Instance {
        get {
            if(instance == null) {
                instance = FindAnyObjectByType<CharacterManager>();
                if(instance == null) {
                    Debug.Log("There is no Character Manager in Scene.");
                }
            }
            return instance;
        }
    }

    [SerializeField] private CharacterData mainCharacter;
    [SerializeField] private List<CharacterData> otherPool;

    [SerializeField] private List<CharacterData> allCharacters = new List<CharacterData>();

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
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
    public List<CharacterData> GetCharacterList() {
        return allCharacters;
    }

    private void GenerateRandomTeam() {
        allCharacters.Clear();

        if(mainCharacter != null ) {
            CharacterData mainInst = Instantiate(mainCharacter);
            mainInst.ResetStats();
            allCharacters.Add(mainInst);
        }

        int maxSlot = 3;
        int availableCharacter = otherPool.Count;

        int extraMemberCount = Random.Range(0, Mathf.Min(maxSlot, availableCharacter) + 1);
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
            if(action.character != null) {
                action.character.HandleDailyStatus(action.isFed, action.isWatered, action.isHealed);
            }
        }
    }
}