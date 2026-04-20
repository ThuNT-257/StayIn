using System.Collections.Generic;
using UnityEngine;

public class DistributionPanelUI : MonoBehaviour
{
    public static DistributionPanelUI Instance;

    [SerializeField] private GameObject distributedItemPrefab;
    [SerializeField] private Transform container;

    private List<DistributedItemUi> distributedItemPool = new List<DistributedItemUi>();
    private ResourceUI resourceUI;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

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

    public List<DistributedItemUi> GetCurrenDistributionList() {
        return distributedItemPool;
    }

    public void ValidateToggles() {
        int foodQuantity = ResourceManager.Instance.GetItemQuantity("item_01");
        int waterQuantity = ResourceManager.Instance.GetItemQuantity("item_02");
        int medicineQuantity = ResourceManager.Instance.GetItemQuantity("item_03");

        int foodPlanned = 0;
        int waterPlanned = 0;
        int medicinePlanned = 0;

        foreach (DistributedItemUi item in distributedItemPool) {
            if (!item.gameObject.activeSelf) continue;
            if (item.WillEat) foodPlanned++;
            if (item.WillDrink) waterPlanned++;
            if (item.WillHeal) medicinePlanned++;
        }

        foreach (DistributedItemUi item in distributedItemPool) {
            if (!item.gameObject.activeSelf) continue;

            item.FadeToggle(1, !(item.WillEat || (foodQuantity - foodPlanned > 0)));
            item.FadeToggle(2, !(item.WillDrink || (waterQuantity - waterPlanned > 0)));

            bool isFullHealth = item.CurrentCharacter.Health >= 10;

            bool canHeal = item.WillHeal || (!isFullHealth && (medicineQuantity - medicinePlanned > 0));

            item.FadeToggle(3, !canHeal);
        }
    }

    public void OnToggleChanged() {
        ValidateToggles();
    }
}
