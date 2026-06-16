using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steruje listą ulepszeń, kupionych i do kupienia, i kupowaniem ulepszeń
/// </summary>
public class UpgradesManager : MonoBehaviour
{
    // public List<UpgradeSO> boughtUpgrades = new();

    /// <summary>
    /// key = które ulepszenie, value = poziom ulepszenia (0 - brak)
    /// </summary>
    public List<UpgradeEntry> upgradesList = new();

    /// <summary>
    /// maksymalny poziom ulepszenia, domyślnie 10
    /// </summary>
    const int MAX_UPGRADE_LEVEL = 10;
    public Transform shopUpgradesContainer;
    public GameObject shopUpgradeElementPrefab;

    /// <summary>
    /// Wywoływany po zakupie jakiegoś ulepszenia
    /// <string> - unikalny identyfikator
    /// </summary>
    public event System.Action<string> OnUpgradeBought;

    public Clicker clickerManager;

    public const float BASE_CLICK_VALUE = 1;
    public const float BASE_PASSIVE_INCOME = 0;
    public const float BASE_CLICK_BOOST_CHANCE = 0;
    public const float BASE_CLICK_BOOST_MULTIPLIER = 2;

    void Start()
    {
        if (
            shopUpgradesContainer == null
            || shopUpgradeElementPrefab == null
            || clickerManager == null
        )
        {
            Debug.LogError("Objects are not set in UpgradesManager");
            return;
        }
        foreach (UpgradeEntry element in upgradesList)
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
    //TODO: żeby można było kupić kilka poziomów na raz, np. max bazując na walucie
    {
        UpgradeEntry foundUpgrade = FindUpgradeSO(identifier);
        if (foundUpgrade == null)
            return false;
        Debug.Log(
            $"Buying for {foundUpgrade.upgrade.data.Cost}, while having {clickerManager.LeavesAmount}"
        );
        if (foundUpgrade.level >= MAX_UPGRADE_LEVEL)
            return false;

        if (!clickerManager.SpendCurrency(foundUpgrade.upgrade.data.Cost))
            return false;

        foundUpgrade.level += 1;

        Debug.Log(
            $"NewClickValue: {CalculateUpgradesValue(ModifierType.IncreaseClickValue, BASE_CLICK_VALUE)}"
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
        foreach (var element in upgradesList)
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
        foreach (var upgradeListElement in upgradesList)
        {
            Upgrade data = upgradeListElement.upgrade.data;
            if (data.modifier.ModifierType == modifierType)
                result += data.modifier.Amount * upgradeListElement.level;
        }
        return result;
    }
}

[System.Serializable]
public class UpgradeEntry
{
    public UpgradeSO upgrade;
    public int level = 0;
}
