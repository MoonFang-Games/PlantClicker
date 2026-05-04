using System;
using UnityEngine;

/// <summary>
/// Ten skrypt odpowiada za przechowywanie informacji o wszystkich dostępnych ulepszeniach, mechanizmach ich działania oraz funkcjach umożliwiających zakup ulepszeń.
/// Pojedyncze ulepszenie.
/// </summary>
[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    /// <summary>
    /// ulepszenie tego konketnego ScriptableObject
    /// </summary>
    public Upgrade data;

    /// <summary>
    /// unikalny identyfikator ScriptableObject - taki sam jak data
    /// </summary>
    [SerializeField]
    private string uniqueID;

    /// <summary>
    /// getter unikalnego identyfikatora
    /// </summary>
    public string UniqueID => uniqueID;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (string.IsNullOrEmpty(UniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
        }

        // walidacja danych
        if (data == null)
            return;

        if (data.Cost < 0)
        {
            throw new System.ArgumentException(
                $"{data.title}: Cannot set upgrade cost to negative value"
            );
        }

        if (data.modifier == null)
        {
            throw new System.ArgumentException($"{data.title}: Upgrade modifier cannot be null");
        }

        if (data.modifier.Amount <= 0)
        {
            throw new System.ArgumentException(
                $"{data.title}: Modifier amount cannot be negative or zero"
            );
        }
    }
#endif
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
    /// <summary> Wartość kliknięcia </summary>
    IncreaseClickValue,

    /// <summary> Waluta na sekundę </summary>
    IncreasePassiveIncome,

    /// <summary> Szansa na to, że kliknięcie da więcej waluty (ulepszone kliknięcie) </summary>
    IncreaseClickBoostChance,

    /// <summary> Siła działania ulepszonego kliknięcia, np. daje 3 razy więcej </summary>
    IncreaseClickBoostMultiplier,
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
    private ModifierType modifierType;
    public ModifierType ModifierType => modifierType;

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
