using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyRandom 
{
    public static float GetRandom(float min, float max) 
    {
        return min + (Random.value * (max-min));
    }

    public static T Roulette<T>(Dictionary<T, float> items)
    {
        float total = 0f;
        foreach (var item in items) 
        {
            total += item.Value;
        } 
        float random = Random.Range(0, total);
        foreach (var item in items)
        {
            random = random - item.Value;
            if (random <= 0)
            {
                return item.Key;
            }
           
        }
        return default;
    }
}
