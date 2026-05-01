using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    /// <summary>
    /// <Upgrade, isbought>
    /// </summary>
    public List<UpgradeEntry> upgrades = new();
    public List<UpgradeSO> boughtUpgrades = new();
    public Transform shopUpgradesContainer;
    public GameObject shopUpgradeElementPrefab;

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
        controller.PrepareButton();
    }

    /// <summary>
    /// Kupuje ulepszenie na podstawie niepowtarzalnego identyfikatora oraz ceny
    /// </summary>
    /// <param name="title">Niepowtarzalny identyfikator ulepszenia</param>
    /// <returns>bool - czy udało się kupić?</returns>
    public bool BuyUpgrade(string title)
    {
        UpgradeEntry foundUpgrade = FindUpgradeSO(title);
        if (foundUpgrade.upgrade == null)
            return false;
        if (foundUpgrade.isBought == true)
            return false;

        foundUpgrade.isBought = true;
        boughtUpgrades.Add(foundUpgrade.upgrade);
        return true;
    }

    /// <summary>
    /// Szuka ulepszenia w liście ScriptableObjects ulepszeń
    /// </summary>
    /// <param name="title">Niepowtarzalny identyfikator ulepszenia</param>
    /// <returns>UpgradeEntry - ScriptableObject i bool isBought</returns>
    public UpgradeEntry FindUpgradeSO(string title)
    {
        if (upgrades.Count == 0)
            return null;
        foreach (var element in upgrades)
        {
            if (element.upgrade.data.title == title)
                return element;
        }
        return null;
    }
}

[System.Serializable]
public class UpgradeEntry
{
    public UpgradeSO upgrade;
    public bool isBought;
}
