using System.Collections.Generic;
using System.Xml;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

/// <summary>
/// Steruje listą ulepszeń, kupionych i do kupienia, i kupowaniem ulepszeń
/// </summary>
public class UpgradesManager : MonoBehaviour
{
    /// <summary>
    /// <Upgrade, isbought>
    /// </summary>
    public List<UpgradeEntry> upgrades = new();
    public List<UpgradeSO> boughtUpgrades = new();
    public Transform shopUpgradesContainer;
    public GameObject shopUpgradeElementPrefab;

    /// <summary>
    /// Wywoływany po zakupie jakiegoś ulepszenia
    /// <string> - unikalny identyfikator
    /// </summary>
    public event System.Action<string> OnUpgradeBought;

    // TEMP
    public int currency = 0;

    float BASE_CLICK_VALUE = 1;
    float BASE_PASSIVE_INCOME = 0;
    float BASE_CLICK_BOOST_CHANCE = 0;
    float BASE_CLICK_BOOST_MULTIPLIER = 2;

    void Start()
    {
        if (
            shopUpgradesContainer == null
            || upgrades.Count == 0
            || shopUpgradeElementPrefab == null
        )
        {
            Debug.LogError("Objects are not set in UpgradesManager");
            return;
        }
        foreach (UpgradeEntry element in upgrades)
        {
            InstantiateButton(element);
        }
    }

    /// <summary>
    /// Tworzy przyciski ulepszeń w sklepie i przypisuje dane
    /// </summary>
    /// <param name="element">ScriptableObject ulepszenia z danymi i bool isBought</param>
    void InstantiateButton(UpgradeEntry element)
    {
        GameObject newElement = Instantiate(shopUpgradeElementPrefab, shopUpgradesContainer);
        UpgradeController controller = newElement.GetComponent<UpgradeController>();
        if (controller == null)
        {
            Debug.LogError("Upgrade does not have UpgradeController component");
            return;
        }
        if (element.upgrade == null)
        {
            Debug.LogError("Upgrade does not have Upgrade or UpgradeSO component");
            return;
        }
        controller.data = element.upgrade.data;
        controller.upgradesManager = this;
        controller.PrepareButton(element.upgrade.UniqueID);
    }

    /// <summary>
    /// Kupuje ulepszenie na podstawie niepowtarzalnego identyfikatora oraz ceny
    /// </summary>
    /// <param name="identifier">Niepowtarzalny identyfikator ulepszenia</param>
    /// <returns>bool - czy udało się kupić?</returns>
    public bool BuyUpgrade(string identifier)
    {
        UpgradeEntry foundUpgrade = FindUpgradeSO(identifier);
        if (foundUpgrade == null)
            return false;
        if (foundUpgrade.isBought)
            return false;
        if (foundUpgrade.upgrade.data.Cost > currency)
            return false;

        foundUpgrade.isBought = true;
        currency -= foundUpgrade.upgrade.data.Cost;
        boughtUpgrades.Add(foundUpgrade.upgrade);

        Debug.Log(
            $"ClickValue: {CalculateUpgradesValue(ModifierType.IncreaseClickValue, BASE_CLICK_VALUE)}"
        );

        OnUpgradeBought?.Invoke(identifier);
        return true;
    }

    /// <summary>
    /// Szuka ulepszenia w liście ScriptableObjects ulepszeń
    /// </summary>
    /// <param name="identifier">Niepowtarzalny identyfikator ulepszenia</param>
    /// <returns>UpgradeEntry - ScriptableObject i bool isBought</returns>
    public UpgradeEntry FindUpgradeSO(string identifier)
    {
        foreach (var element in upgrades)
        {
            if (element.upgrade.UniqueID == identifier)
                return element;
        }
        return null;
    }

    /// <summary>
    /// Liczy działanie ulepszeń
    /// </summary>
    /// <param name="modifierType">parametr do sprawdzenia</param>
    /// <param name="baseValue">wartość podstawowa, np. dla kliknięcia (ClickValue) jest to 1</param>
    /// <returns></returns>
    public float CalculateUpgradesValue(ModifierType modifierType, float baseValue = 0)
    {
        float result = baseValue;
        foreach (UpgradeSO upgrade in boughtUpgrades)
        {
            Upgrade data = upgrade.data;
            if (data.modifier.ModifierType == modifierType)
                result += data.modifier.Amount;
        }
        return result;
    }
}

[System.Serializable]
public class UpgradeEntry
{
    public UpgradeSO upgrade;
    public bool isBought;
}
