using UnityEngine;

public class Clicker : MonoBehaviour
{
    public int click = 0;
    public void Click()
    {
        if(Random.Range(0f, 1f) <= 0.01f)
        {
            click +=100;
        }
        click++;

        Debug.Log("Liczba Listków: " + click);
    }
}