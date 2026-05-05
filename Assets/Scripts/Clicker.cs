using UnityEngine;
using System;

/// <summary>
/// 1.Klasa Clicker odpowiada za logikę klikania w liście.
///  2.Zawiera informacje o ilości zebranych liści, ilości kliknięć oraz mechanizm losowego zwiększania ilości zebranych liści przy każdym kliknięciu.
/// </summary>
public class Clicker : MonoBehaviour
{
   private int leavesAmount = 0;
   public int LeavesAmount => leavesAmount;
   private int clicksAmount = 0;
   public int ClicksAmount => clicksAmount;
   public int clickBoosValue = 100;
   public float clickBoostChange = 0.01f;
   public event Action OnClick;

/// <summary>
/// 1.Funkcja kttóra jest wywoływana przy każdym kliknięciu. 
/// 2.Zwiększa ilość zebranych liści o 1 lub o wartość clickBoosValue, w zależności od losowego wyniku porównania z clickBoostChange.
///  3.Zwiększa również ilość kliknięć o 1 i wywołuje zdarzenie OnClick.
/// </summary>
    public void Click()
    {
        if(UnityEngine.Random.Range(0f, 1f) < clickBoostChange)
        {
            leavesAmount += clickBoosValue;
        } else
        {
            leavesAmount++;
        }
        clicksAmount++;
        Debug.Log($"Liczba listków: {leavesAmount}, Liczba kliknięć: {clicksAmount}, Szanse na zdobycie Boosta(+100): {clickBoostChange}");
        OnClick?.Invoke();
    }


}