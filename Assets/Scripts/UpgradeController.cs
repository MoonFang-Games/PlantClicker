using System;
using System.Data.Common;
using TMPro;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public Upgrade data;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;

    const int THOUSAND = 1000;
    const int MILLION = 1000000;
    const int BILLION = 1000000000;

    // TOO BIG : const int TRILLION = 1000000000000;

    /// <summary>
    /// Wypełnia miejsca z tekstem przy przycisku zakupu ulepszenia
    /// </summary>
    public void PrepareButton()
    {
        if (data == null)
        {
            Debug.LogError($"Upgrade data is not set");
            return;
        }
        title.text = data.title;
        description.text = data.description;
        cost.text = GetNumberInNotation(data.Cost, "$");
    }

    /// <summary>
    /// Skraca liczbę tak, aby zajmowała mniej miejsca (1 000 000 -> 1M)
    /// </summary>
    /// <param name="number">Liczba do skrócenia</param>
    /// <param name="suffix">Dodatkowy sufiks na końcu (np. $ w 100M$)</param>
    /// <returns>string -> skrócona wersja liczby</returns>
    public string GetNumberInNotation(int number, string suffix = "")
    {
        string result = "";
        if (number < THOUSAND)
            result = number.ToString();
        else if (data.Cost < MILLION)
            result = Math.Round((double)number / THOUSAND, 2).ToString() + "k";
        else if (number < BILLION)
            result = Math.Round((double)number / MILLION, 2).ToString() + "M";
        else
            result = Math.Round((double)number / BILLION, 2).ToString() + "B";

        result += suffix;
        return result;
    }
}
