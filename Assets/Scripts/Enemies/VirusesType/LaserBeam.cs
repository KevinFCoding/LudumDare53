using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : Spawnable
{
    private float _toggleLaserTimer;
    private float _toggleLaserTimerMax;
    private bool _isActive;
    private float _waintingForActivationAnimation;

    private BoxCollider _collider;
    private SpriteRenderer _renderer;
    private Animator _animator;

    private void Start()
    {
        _isActive = true;
        _collider = GetComponent<BoxCollider>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _toggleLaserTimerMax = Random.Range(1f, 3f);
    }

    private void Update()
    {
        _toggleLaserTimer -= Time.deltaTime;
        if (_toggleLaserTimer <= 0)
        {
            ToggleLaserActivation();
            _toggleLaserTimer = _toggleLaserTimerMax;
        }

        if (_isActive)
        {
            // Permet de delayer l'activation du collider pour que le début de l'animation ne touche pas le joueur
            if (_waintingForActivationAnimation > 0)
            {
                _waintingForActivationAnimation -= Time.deltaTime;
            }
            else
            {
                _collider.enabled = true;
            }
        }
    }

    public override void HasTouchedPlayer()
    {
        base.HasTouchedPlayer();

    }

    private void ToggleLaserActivation()
    {
        _isActive = !_isActive;
        _renderer.enabled = _isActive;
        if (_isActive)
        {
            _waintingForActivationAnimation = 0.5f;
            _animator.SetTrigger("Activation");
        }
        else
        {
            _collider.enabled = false;
        }
    }
}
