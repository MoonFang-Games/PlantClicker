using UnityEngine;

public class Clicker : MonoBehaviour
{
    public int _click = 0;
    public void Click()
    {
        if(Random.Range(0f, 1f) <= 0.01f)
        {
            _click +=100;
        }
        _click++;

        Debug.Log("Liczba Listków: " + _click);
    }
}