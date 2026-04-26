using System.Collections.Generic;
using UnityEngine;

public class CharacterListUI : MonoBehaviour {

    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Transform container;

    private List<CharacterUI> characterPool = new List<CharacterUI>();

    private void OnEnable() {
        GameManager.OnDayChanged += DisplayCharacterList;
    }

    private void OnDisable() {
        GameManager.OnDayChanged -= DisplayCharacterList;
    }

    public void DisplayCharacterList() {
        if (container == null || characterPrefab == null || CharacterManager.Instance == null) return;

        List<CharacterData> currentCharacters = CharacterManager.Instance.GetCharacterList();
        if (currentCharacters == null) return;

        for (int i = 0; i < characterPool.Count; i++) {
            bool isNeeded = i < currentCharacters.Count;
            characterPool[i].gameObject.SetActive(isNeeded);
        }

        for (int i = 0; i < currentCharacters.Count; i++) {
            CharacterUI uiInstance;

            if (i < characterPool.Count) {
                uiInstance = characterPool[i];
            } else {
                GameObject newCharacter = Instantiate(characterPrefab, container);
                newCharacter.transform.localScale = Vector3.one;
                uiInstance = newCharacter.GetComponent<CharacterUI>();
                characterPool.Add(uiInstance);
            }

            uiInstance.gameObject.SetActive(true);
            uiInstance.SetUp(currentCharacters[i]);
        }
    }
}