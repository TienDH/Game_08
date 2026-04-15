using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// General-purpose extension methods for the VeChai project.
/// </summary>
public static class Extensions
{
    // --- List helpers ---

    public static T Random<T>(this List<T> list)
    {
        if (list == null || list.Count == 0) return default;
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    // --- Transform helpers ---

    public static void ResetLocal(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale    = Vector3.one;
    }

    // TODO: Add more helpers as needed
}
