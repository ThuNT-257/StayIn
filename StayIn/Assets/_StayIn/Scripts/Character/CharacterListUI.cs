using System.Collections.Generic;
using UnityEngine;

public class CharacterListUI : MonoBehaviour
{
    public static CharacterListUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Transform container;

    private List<CharacterUI> characterPool = new List<CharacterUI>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void DisplayCharacterList() {
        if(container == null || characterPrefab == null || CharacterManager.Instance == null) {
            return;
        }

        foreach(CharacterUI character in characterPool) {
            character.gameObject.SetActive(false);
        }

        List<CharacterData> currenCharacters = CharacterManager.Instance.GetCharacterList();

        int uiIndex = 0;
        foreach (CharacterData character in currenCharacters) {
            CharacterUI uiInstance;

            if(uiIndex <  characterPool.Count) {
                uiInstance = characterPool[uiIndex];
            } else {
                GameObject newCharacter = Instantiate(characterPrefab, container);
                uiInstance = newCharacter.GetComponent<CharacterUI>();
                characterPool.Add(uiInstance);
            }

            uiInstance.gameObject.SetActive(true);
            uiInstance.SetUp(character);
            uiIndex++;
            Debug.Log(character.CharacterName + ": " + "Hunger: " + character.Hunger + "; Thirsty: " + character.Thirsty + "; Health: " + character.Health);
        }

    }
}
