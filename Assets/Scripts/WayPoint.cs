using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    WayPoints points;

    public void setPoints(WayPoints wp)
    {
        this.points = wp;
    }

    public WayPoints getPoints()
    {
        return this.points;
    }
}
