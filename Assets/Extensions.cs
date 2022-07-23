using System;
using UnityEngine;

public static class Extensions
{
    static public Vector2Int ToVector2Int(this Vector3 vector)
    {
        return new Vector2Int((int)Math.Round(vector.x), (int)Math.Round(vector.y));
    }
    static public Vector3 ToVector3(this Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}