using System;
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
    public Upgrade data;

    /// <summary>
    /// Unikalny identyfikator
    /// </summary>
    [SerializeField]
    string uniqueID;

    /// <summary>
    /// Getter unikalnego identyfikatora
    /// </summary>
    public string UniqueID => uniqueID;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
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
