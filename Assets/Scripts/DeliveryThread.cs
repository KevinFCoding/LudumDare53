using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryThread : MonoBehaviour
{
    [SerializeField] private Texture2D _baseCursorTexture;
    [SerializeField] private Texture2D _hoverCursorTexture;

    private bool _isPaused = false;

    public void GamePaused(bool isPaused)
    {
        _isPaused = isPaused;
    }
    private void OnMouseOver()
    {
        if (_isPaused) return;
        Cursor.SetCursor(_hoverCursorTexture, new Vector2(_hoverCursorTexture.width / 2, _hoverCursorTexture.height / 2), CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(_baseCursorTexture, new Vector2(_baseCursorTexture.width / 2, 0), CursorMode.Auto);
    }
}
