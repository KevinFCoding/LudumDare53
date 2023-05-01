using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLDestroyer : MonoBehaviour
{
    [SerializeField] GameObject dontdestroy;
    private void Start()
    {
        DestroyAllDontDestroyOnLoadObjects();
    }

    public void DestroyAllDontDestroyOnLoadObjects()
    {

        var go = new GameObject("Sacrificial Lamb");
        DontDestroyOnLoad(go);

        foreach (var root in go.scene.GetRootGameObjects())
            if(root.gameObject != dontdestroy) Destroy(root);
    }
}
