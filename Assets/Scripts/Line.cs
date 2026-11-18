using System;
using UnityEngine;

[Serializable]
public class Line
{
    public Vector3 start;
    public Vector3 end;

    public Vector3 Midpoint => (start + end) * 0.5f;

    public Quaternion Rotation
    {
        get
        {
            Vector3 direction = end - start;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0f, 0f, angle);
        }
    }
}
