using System.Collections.Generic;
using UnityEngine;

public class DistributionPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject distributedItemPrefab;
    [SerializeField] private Transform container;

    private List<DistributedItemUi> distributedItemPool = new List<DistributedItemUi>();

    public void OpenPanel() {
        gameObject.SetActive(true);
        DisplayDistributionList();
    }

    public void DisplayDistributionList() {
        if(container == null || distributedItemPrefab == null) {
            return;
        }

        foreach(DistributedItemUi item in distributedItemPool) {
            item.gameObject.SetActive(false);
        }

        if(CharacterManager.Instance == null) {
            return;
        }

        List<CharacterData> characters = CharacterManager.Instance.GetCharacterList();

        for(int i = 0; i < characters.Count; i++) {
            DistributedItemUi uiInstance;

            if(i < distributedItemPool.Count) {
                uiInstance = distributedItemPool[i];
            } else {
                GameObject newObject = Instantiate(distributedItemPrefab, container);
                uiInstance = newObject.GetComponent<DistributedItemUi>();
                distributedItemPool.Add(uiInstance);
            }

            uiInstance.gameObject.SetActive(true);
            uiInstance.SetUp(characters[i]);
        }
    }
}
