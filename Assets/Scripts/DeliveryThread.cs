using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryThread : MonoBehaviour
{
    [SerializeField] private Texture2D _baseCursorTexture;
    [SerializeField] private Texture2D _hoverCursorTexture;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        Cursor.SetCursor(_hoverCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(_baseCursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
