using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {
    public static CharacterManager Instance;

    [Header("Data")]
    [SerializeField] private List<CharacterData> allCharacters = new List<CharacterData>();
    private List<CharacterUI> allCharacterUIs = new List<CharacterUI>();

    [Header("Characters")]
    [SerializeField] private CharacterData mainCharacter;
    [SerializeField] private List<CharacterData> otherPool;

    [Header("Settings")]
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Transform container;

    [Header("UI Settings")]
    [SerializeField] private DistributionPanelUI distributionUI;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GenerateRandomTeam();
        InitializeCharacters();
        distributionUI.DisplayDistributionList();
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

    private void InitializeCharacters() {
        if (container == null) {
            return;
        }

        foreach (Transform child in container) {
            Destroy(child.gameObject);
        }
        allCharacterUIs.Clear();

        if (characterPrefab == null) {
            return;
        }

        foreach (CharacterData data in allCharacters) {
            if(data == null) {
                continue;
            }
            GameObject newSlot = Instantiate(characterPrefab, container);
            CharacterUI slotUI = newSlot.GetComponent<CharacterUI>();
            if(slotUI != null) {
                slotUI.SetUp(data);
                allCharacterUIs.Add(slotUI);
            }
        }
    }

    public void RefreshAllUIs() {
        foreach(CharacterUI ui in allCharacterUIs) {
            ui.UpdateUI();
        }
    }

    public void ProcessNewDay() {
        foreach(CharacterData character in allCharacters) {
            if(character == null) {
                continue;
            }
            if (character.IsDead) {
                continue;
            }
            character.Hunger -= 1;
            character.Thirsty -= 1;

            character.Health = Mathf.Clamp(character.Health, 0, 10);
            character.Hunger = Mathf.Clamp(character.Hunger, 0, 10);
            character.Thirsty = Mathf.Clamp(character.Thirsty, 0, 5);

            if (character.Health == 0 || character.Hunger == 0 || character.Thirsty == 0) {
                character.IsDead = true;
            }
        }

        RefreshAllUIs();
    }

    public List<CharacterData> GetCharacterList() {
        return allCharacters;
    }
}