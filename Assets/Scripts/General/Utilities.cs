using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3[] BuildArc(Vector3 startPos, Vector3 endPos, float arcHeight)
    {
        Vector3 p0 = startPos;
        Vector3 p3 = endPos;

        Vector3 p1 = p0 + Vector3.up * arcHeight;
        Vector3 p2 = p3 + Vector3.up * arcHeight;

        Vector3 mid = (p1 + p2) * 0.5f + Vector3.up * arcHeight * 0.3f;

        List<Vector3> controlPoints = new List<Vector3> { p0, p1, mid, p2, p3 };
       
        return controlPoints.ToArray();
    }

    public static bool TryGetMouseWorldPosition(Camera cam, out Vector3 worldPos)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float t = -ray.origin.y / ray.direction.y;
        if (t > 0)
        {
            worldPos = ray.origin + ray.direction * t;
            return true;
        }

        worldPos = default;
        return false;
    }
}
