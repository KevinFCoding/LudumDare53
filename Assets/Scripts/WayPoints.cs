using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WayPoints
{
    public readonly Transform startPoint;
    public readonly Transform endPoint;

    public WayPoints(Transform transform1, Transform transform2)
    {
        this.startPoint = transform1;
        this.endPoint = transform2;
    }
}
