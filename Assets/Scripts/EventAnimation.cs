using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimation : MonoBehaviour
{
    [SerializeField] GameCanvasManager _gameCanvasManager;
    public void EventFadeOut()
    {
        _gameCanvasManager.AnimationOver();
    }

    public void EventStamp()
    {
        _gameCanvasManager.PlayStampSound();
    }
}
