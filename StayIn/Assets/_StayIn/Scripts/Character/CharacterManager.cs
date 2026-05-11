using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {
    private static CharacterManager instance;
    public static CharacterManager Instance {
        get {
            if (instance == null) {
                instance = FindAnyObjectByType<CharacterManager>();
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

    private void OnEnable() => GameManager.DayAction += ProcessDaySummary;
    private void OnDisable() => GameManager.DayAction -= ProcessDaySummary;

    public void Init() {
        ClearExistingCharacters();
        GenerateRandomTeam();
    }
    public List<CharacterData> GetCharacterList() => allCharacters;

    private void GenerateRandomTeam() {
        allCharacters.Clear();
        if (mainCharacter != null) {
            CharacterData mainInst = Instantiate(mainCharacter);
            mainInst.ResetStats();
            allCharacters.Add(mainInst);
        }

        //int extraMemberCount = Random.Range(0, Mathf.Min(3, otherPool.Count) + 1);
        int extraMemberCount = 3;
        List<CharacterData> tempPool = new List<CharacterData>(otherPool);

        for (int i = 0; i < extraMemberCount; i++) {
            int randomIndex = Random.Range(0, tempPool.Count);
            CharacterData memberInst = Instantiate(tempPool[randomIndex]);
            memberInst.ResetStats();
            allCharacters.Add(memberInst);
            tempPool.RemoveAt(randomIndex);
        }
        Debug.Log("[CharacterManager] Generate Random Team: " + (extraMemberCount + 1) + " characters");
    }

    public void ClearExistingCharacters() {
        foreach(CharacterData character in allCharacters) {
            if(character != null) {
                Destroy(character);
            }
        }

        allCharacters.Clear();
        Debug.Log("[CharacterManager] Clear Existing Characters");
    }

    public void ProcessDaySummary(List<DayActionData> actions) {

        //int newDeathsToday = 0;

        //foreach (DayActionData action in actions) {
        //    if (action.character == null || (action.character.isDead && action.character.DeathLogged)) continue;

        //    int hungerChange = action.isFed ? 5 : -1;
        //    int thirstChange = action.isWatered ? 5 : -1;

        //    int healthChange = action.isHealed ?
        //        (GameConfig.MAX_HEALTH - action.character.Health) :
        //        (action.character.Health < GameConfig.MAX_HEALTH ? -1 : 0);

        //    int sanityChange = action.isEntertained ? 2 : -1;

        //    if (action.character.Health >= GameConfig.MAX_HEALTH &&
        //        action.character.Hunger >= GameConfig.MAX_HUNGER &&
        //        action.character.Thirsty >= GameConfig.MAX_THIRSTY) {
        //        sanityChange += 1;
        //    }

        //    bool wasAliveBefore = !action.character.isDead;

        //    action.character.UpdateStats(healthChange, hungerChange, thirstChange, sanityChange);

        //    if (wasAliveBefore && action.character.isDead) {
        //        newDeathsToday++;
        //        action.character.SetDeathLogged(true);
        //    }
        //}

        //if (newDeathsToday > 0) {
        //    ApplyDeathTrauma(newDeathsToday);
        //}
    }

    private void ApplyDeathTrauma(int count) {
        foreach (var character in allCharacters) {
            if (!character.isDead) {
                character.UpdateStats(0, 0, 0, -(3 * count));
            }
        }
    }

    public bool IsEveryoneInsane() {
        foreach (var c in allCharacters) {
            if (!c.isDead && c.Sanity > 0) return false;
        }
        return true;
    }
}