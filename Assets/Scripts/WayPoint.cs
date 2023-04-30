using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WayPoints points;
    public bool IsActive { get; set; }

    private void Awake()
    {
        IsActive = true;
    }

    public void SetPoints(WayPoints wp)
    {
        this.points = wp;
    }

    public WayPoints GetPoints()
    {
        return this.points;
    }
}
