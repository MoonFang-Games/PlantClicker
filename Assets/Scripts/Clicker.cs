using System;
using UnityEngine;

/// <summary>
/// 1.Klasa Clicker odpowiada za logikę klikania w liście.
///  2.Zawiera informacje o ilości zebranych liści, ilości kliknięć oraz mechanizm losowego zwiększania ilości zebranych liści przy każdym kliknięciu.
/// </summary>
public class Clicker : MonoBehaviour
{
    [SerializeField]
    private int leavesAmount = 0;
    public int LeavesAmount => leavesAmount;
    private int clicksAmount = 0;
    public int ClicksAmount => clicksAmount;
    public event Action OnClick;

    public UpgradesManager upgradesManager;
    private int clickValue = 1;
    private float clickBoostChance = 0;
    private float clickBoostMultiplier = 2;

    void Start()
    {
        if (upgradesManager == null)
        {
            Debug.LogError("UpgradesManager is not set");
            return;
        }
        GetUpgrades();
    }

    void OnEnable()
    {
        upgradesManager.OnUpgradeBought += GetUpgrades;
    }

    void OnDisable()
    {
        upgradesManager.OnUpgradeBought -= GetUpgrades;
    }

    void GetUpgrades(string UniqueID = "")
    {
        clickValue = (int)
            Math.Ceiling(
                upgradesManager.CalculateUpgradesValue(
                    ModifierType.IncreaseClickValue,
                    UpgradesManager.BASE_CLICK_VALUE
                )
            );
        clickBoostChance = upgradesManager.CalculateUpgradesValue(
            ModifierType.IncreaseClickBoostChance,
            UpgradesManager.BASE_CLICK_BOOST_CHANCE
        );
        clickBoostMultiplier = upgradesManager.CalculateUpgradesValue(
            ModifierType.IncreaseClickBoostMultiplier,
            UpgradesManager.BASE_CLICK_BOOST_MULTIPLIER
        );
        Debug.Log(
            $"Obliczono ulepszenia: clickValue:{clickValue}, clickBoostChance:{clickBoostChance}, clickBoostMultiplier:{clickBoostMultiplier}"
        );
    }

    /// <summary>
    /// 1.Funkcja kttóra jest wywoływana przy każdym kliknięciu.
    /// 2.Zwiększa ilość zebranych liści o 1 lub o wartość clickBoosValue, w zależności od losowego wyniku porównania z clickBoostChange.
    ///  3.Zwiększa również ilość kliknięć o 1 i wywołuje zdarzenie OnClick.
    /// </summary>
    public void Click()
    {
        if (upgradesManager == null)
        {
            Debug.LogError("UpgradesManager is not set");
            return;
        }

        if (UnityEngine.Random.Range(0f, 1f) < clickBoostChance)
        {
            leavesAmount = (int)Math.Ceiling(clickValue * clickBoostMultiplier);
            Debug.Log("Boosted");
        }
        else
        {
            leavesAmount += clickValue;
        }
        clicksAmount++;
        Debug.Log(
            $"Liczba listków: {leavesAmount}, Liczba kliknięć: {clicksAmount}, Szanse na zdobycie Boosta: {clickBoostChance}, Mnożnik Boosta: {clickBoostMultiplier}"
        );
        OnClick?.Invoke();
    }

    public bool SpendCurrency(int amount)
    {
        if (leavesAmount < amount)
            return false;

        leavesAmount -= amount;
        return true;
    }
}
