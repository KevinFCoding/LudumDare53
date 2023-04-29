using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public bool collide = false;
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(name+"Collision with "+collision.name);
        if (collision.gameObject.GetComponent<Line>())
        {
            collide = true;
        }
    }
}
