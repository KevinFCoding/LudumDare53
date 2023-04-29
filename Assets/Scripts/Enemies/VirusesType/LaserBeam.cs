using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : Spawnable
{
    private float _toggleLaserTimer;
    private float _toggleLaserTimerMax = 1.5f;
    private bool _isActive;

    private BoxCollider _collider;
    private SpriteRenderer _renderer;
    private Animator _animator;

    private void Start()
    {
        _isActive = true;
        _collider = GetComponent<BoxCollider>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _toggleLaserTimer -= Time.deltaTime;
        if (_toggleLaserTimer <= 0)
        {
            ToggleLaserActivation();
            _toggleLaserTimer = _toggleLaserTimerMax;
        }
    }

    public override void HasTouchedPlayer()
    {
        base.HasTouchedPlayer();

    }

    private void ToggleLaserActivation()
    {
        _isActive = !_isActive;
        _collider.enabled = _isActive;
        _renderer.enabled = _isActive;
        if (_isActive)
        {
            _animator.SetTrigger("Activation");
        }

    }
}
