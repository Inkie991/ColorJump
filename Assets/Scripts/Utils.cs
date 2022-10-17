using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void SetY(GameObject obj, float y)
    {
        var position = obj.transform.position;
        position.y = y;
        obj.transform.position = position;
    }

    public static T GetRandomItem<T>(List<T> list)
    {
        int i = (int) Random.Range(0, list.Count);
        return list[i];
    }
}