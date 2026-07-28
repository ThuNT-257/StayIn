using Assets._StayIn.Scripts.Definitions;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class DistributionManager : MonoBehaviour
{
    private static DistributionManager instance;

    public static DistributionManager Instance {
        get {
            if(instance == null) {
                instance = FindAnyObjectByType<DistributionManager>();
                if(instance == null) {
                    Debug.Log("DistributionManager is not in the Scene.");
                }
            }
            return instance;
        }
    }

    private List<ResourceItem> tempResources = new List<ResourceItem>();
    private Dictionary<CharacterData, ActionPlan> characterPlans = new Dictionary<CharacterData, ActionPlan>();
    private List<ResourceItem> cachedSanityItems = new List<ResourceItem>(); 

    public Dictionary<CharacterData, ActionPlan> GetCharacterPlans() => characterPlans;
    public List<ResourceItem> GetCachedSanityItems() => cachedSanityItems;

    public static Action<List<ResourceItem>> OnTempStockChanged;

    public void Init() {
        tempResources.Clear();
        List<ResourceItem> resources = ResourceManager.Instance.GetCurrenResource();
        foreach (ResourceItem res in resources) {
            tempResources.Add(new ResourceItem {
                itemData = res.itemData,
                quantity = res.quantity
            });
        }

        characterPlans.Clear();
        cachedSanityItems = GetAvailableSanityItems();

        List<CharacterData> characters = CharacterManager.Instance.GetCharacterList();
        foreach(var character in characters) {
            ActionPlan newPlan = new ActionPlan();
            ValidateInitialLock(character, newPlan);
            characterPlans.Add(character, newPlan);
        }
    }

    public List<ResourceItem> GetAvailableSanityItems() {
        List<ResourceItem> sanityItems = new List<ResourceItem>();
        foreach (var res in tempResources) {
            if (res.quantity > 0 &&
                res.itemData.ItemType == ItemType.Utility &&
                res.itemData.SanityRestoreValue > 0) {
                sanityItems.Add(res);
            }
        }
        return sanityItems;
    }

    public void ValidateInitialLock(CharacterData character, ActionPlan plan) {
        bool hasFood = GetTempQuantity("item_01") > 0;
        bool hasWater = GetTempQuantity("item_02") > 0;
        bool hasMed = GetTempQuantity("item_03") > 0;

        bool isHealthFull = character.Health >= GameConfig.MAX_HEALTH;
        bool isSanityFull = character.Sanity >= GameConfig.MAX_SANITY;

        plan.IsFoodLocked = !hasFood;
        plan.IsWaterLocked = !hasWater;
        plan.IsMedLocked = !hasMed || isHealthFull;
        plan.IsSanityLocked = (cachedSanityItems.Count == 0) || isSanityFull;
    }

    private int GetTempQuantity(string itemId) {
        ResourceItem item = tempResources.Find(x => x.itemData.ItemID == itemId);
        return item != null ? item.quantity : 0;
    }

    public void ValidatePlannedDistribution() {
        List<ResourceItem> resources = ResourceManager.Instance.GetCurrenResource();

        foreach (ResourceItem item in resources) {
            ResourceItem temp = tempResources.Find(x => x.itemData.ItemID == item.itemData.ItemID);
            if (temp != null) {
                temp.quantity = item.quantity;
            }
        }

        foreach (ActionPlan plan in characterPlans.Values) {
            if (plan.WillEat) {
                var item = GetTempItem("item_01");
                if (item != null) item.quantity--;
            }

            if (plan.WillDrink) { 
                var item = GetTempItem("item_02");
                if (item != null) item.quantity--;
            }

            if (plan.WillHeal) { 
                var item = GetTempItem("item_03");
                if (item != null) item.quantity--;
            }

            if (!string.IsNullOrEmpty(plan.SelectedSanityItemID)) {
                ResourceItem sItem = GetTempItem(plan.SelectedSanityItemID);
                if (sItem != null) sItem.quantity--;
            }
        }

        foreach (var entry in characterPlans) {
            var character = entry.Key;
            var plan = entry.Value;

            plan.IsFoodLocked = !plan.WillEat && GetTempItem("item_01").quantity <= 0;
            plan.IsWaterLocked = !plan.WillDrink && GetTempItem("item_02").quantity <= 0;

            bool isHealthFull = character.Health == 10;
            plan.IsMedLocked = (!plan.WillHeal && GetTempItem("item_03").quantity <= 0) || isHealthFull;

            bool IsSanityFull = character.Sanity >= GameConfig.MAX_SANITY;
            plan.IsSanityLocked = IsSanityFull || string.IsNullOrEmpty(plan.SelectedSanityItemID) && !HasAnySanityLeftInTemp();
        }

        OnTempStockChanged?.Invoke(tempResources);
    }

    private ResourceItem GetTempItem(string id) {
        return tempResources.Find(x => x.itemData.ItemID == id);
    }

    private bool HasAnySanityLeftInTemp() {
        foreach (ResourceItem resItem in cachedSanityItems) {
            ResourceItem temp = tempResources.Find(x => x.itemData.ItemID == resItem.itemData.ItemID);

            if (temp != null && temp.quantity > 0) {
                return true; 
            }
        }

        return false;
    }

    public void UpdateSanitySelection(CharacterData character, string itemID) {
        if (!characterPlans.ContainsKey(character)) return;

        ActionPlan plan = characterPlans[character];

        if (plan.SelectedSanityItemID == itemID) {
            plan.SelectedSanityItemID = "";
        } else {
            plan.SelectedSanityItemID = itemID;
        }

        ValidatePlannedDistribution();
    }

    public void EndDayConfirm() {
        foreach(var entry in characterPlans) {
            CharacterData character = entry.Key;
            ActionPlan plan = entry.Value;

            if (character == null || character.isDead) continue;

            //Resource Check
            if (plan.WillEat) {
                ResourceManager.Instance.RemoveItem("item_01", 1);
            }
            if (plan.WillDrink) {
                ResourceManager.Instance.RemoveItem("item_02", 1);
            }
            if (plan.WillHeal) {
                ResourceManager.Instance.RemoveItem("item_03", 1);
            }
            if (!string.IsNullOrEmpty(plan.SelectedSanityItemID)) {
                ResourceManager.Instance.RemoveItem(plan.SelectedSanityItemID, 1);
            }

            //Character Check
            CharacterManager.Instance.ApplySurvivalStats(character, plan.WillEat, plan.WillDrink, plan.WillHeal, plan.SelectedSanityItemID ?? "");
        }
        Init();
    }
}
