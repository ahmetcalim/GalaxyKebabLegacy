using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Personality
{
    public Impatience impatianceValue;
    public int orderTime;
    [System.NonSerialized]
    public bool counterActive=true;

    public enum Impatience
    {
       low,medium,high
    }

    WaitForSeconds wts = new WaitForSeconds(1);

    public IEnumerator TimeCounter(MonoBehaviour m)
    {
        yield return wts;
        if (counterActive)
        {
            orderTime++;
            m.StartCoroutine(TimeCounter(m));
        }
    }

    public void SetCounter(bool active)
    {
        orderTime = 0;
        counterActive = active;
    }
}
