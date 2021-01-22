using UnityEngine;
using System.Collections;

public class Geometry
{
    public static void SetAngle(Vector2 vector, float angle)
    {
        vector.Set(vector.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad), vector.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public static void Rotate(Vector2 vector, float angle)
    {
        SetAngle(vector, Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg + angle);
    }

    public static Vector2 Rotated(Vector2 vector, float angle)
    {
        return ALVector2(Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg + angle, vector.magnitude);
    }

    public static Vector2 ALVector2(float angle, float length)
    {
        return new Vector2(length * Mathf.Cos(angle * Mathf.Deg2Rad), length * Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    //public static 
}