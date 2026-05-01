using UnityEngine;

/// <summary>
/// Ten skrypt odpowiada za przechowywanie informacji o wszystkich dostępnych ulepszeniach, mechanizmach ich działania oraz funkcjach umożliwiających zakup ulepszeń.
/// </summary>
[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    /// <summary>
    /// ulepszenie tego konketnego ScriptableObject
    /// </summary>
    [SerializeField]
    private Upgrade upgrade;

    void OnValidate()
    {
        if (upgrade == null)
            return;
        if (upgrade.Cost < 0)
        {
            throw new System.ArgumentException(
                $"{upgrade.title}: Cannot set upgrade cost to negative value"
            );
        }
        if (upgrade.modifier == null)
        {
            throw new System.ArgumentException($"{upgrade.title}: Upgrade modifier cannot be null");
        }
        if (upgrade.modifier.Amount <= 0)
        {
            throw new System.ArgumentException(
                $"{upgrade.title}: Modifier amount cannot be negative or zero"
            );
        }
    }
}

/// <summary>
/// Pojedyncze ulepszenie, np. zwiększ wartość kliknięcia o jeden, lub zwiększ pasywny przychód o 1/s
/// </summary>
[System.Serializable]
public class Upgrade
{
    /// <summary>
    /// Nazwa (tytuł) wyświetlana w sklepie
    /// </summary>
    [SerializeField]
    public string title;

    /// <summary>
    /// Opis działania (np. "Zwiększa wartość kliknięcia o 1")
    /// </summary>
    [SerializeField]
    public string description;

    /// <summary>
    /// Koszt ulepszenia wyrażony w podstawowej walucie
    /// </summary>
    [SerializeField]
    int cost;

    /// <summary>
    /// getter kosztu ulepszenia
    /// </summary>
    public int Cost => cost;

    /// <summary>
    /// Mechanizm działania ulepszenia (np. ZwiększWartośćKliknięcia:1)
    /// </summary>
    [SerializeField]
    public Modifier modifier;
}

/// <summary>
/// Typ ulepszenia, np. "Zwiększa Wartość Kliknięcia"
/// </summary>
public enum ModifierType
{
    IncreaseClickValue,
    IncreasePassiveIncome,
    IncreaseClickBoostChance,
    IncreaseClickBoostValue,
}

/// <summary>
/// Mechanizm działania ulepszenia - co robi i z jaką wartością
/// </summary>
[System.Serializable]
public class Modifier
{
    /// <summary>
    /// Enumerator - co robi modyfikator
    /// </summary>
    [SerializeField]
    ModifierType modifierType;

    /// <summary>
    /// Z jaką wartością działa modyfikator
    /// </summary>
    [SerializeField]
    float amount;

    /// <summary>
    /// getter wartości modyfikatora ulepszenia
    /// </summary>
    public float Amount => amount;
}
