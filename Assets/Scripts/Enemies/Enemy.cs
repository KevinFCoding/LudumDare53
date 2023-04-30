using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void HasTouchedPlayer();

    protected string name;

    public string getVirusName()
    {
        return name;
    }
}
