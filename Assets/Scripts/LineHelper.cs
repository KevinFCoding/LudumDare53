using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHelper
{
    public static bool HasIntersectionPoint2D(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, out Vector3 intersection)
    {
        if ((p1.x == p3.x && p2.x == p4.x) || (p1.x == p4.x && p2.x == p3.x) )
        {
            float a1 = (p2.z - p1.z) / (p2.x - p1.x);
            float b1 = p1.z - a1 * p1.x;

            float a2 = (p4.z - p3.z) / (p4.x - p3.x);
            float b2 = p3.z - a2 * p3.x;

            float x = (b2 - b1) / (a1 - a2);

            if (double.IsNormal(x))
            {
                float y = a1 * x + b1;
                intersection = new Vector3(x, 0, y) ;
                return true;
            }
        }
        intersection = Vector3.zero;
        return false;
    }

    public static bool IsBetweenPoints(Vector3 linePt1, Vector3 linePt2, Vector3 pt)
    {
        float s1 = pt.x - linePt1.x;
        float s2 = linePt2.x - linePt1.x;

        float d = s1/s2;

        if(d > 0 && d < 1)
        {
            return true;
        }
        return false;
    }
}
