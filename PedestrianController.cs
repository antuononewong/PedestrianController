using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    // Events
    public static event Action OnPedestrianDeath;

    // Components
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    // Movement
    private Vector3 _nextPosition;
    private float _maxWalkDistance = 3f;
    private float _maxWalkTimer = 2f;
    private float _walkTimer;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        ResetWalkTimer();
    }

    private void FixedUpdate()
    {
        _walkTimer -= Time.fixedDeltaTime;

        if (_walkTimer <= 0)
        {
            RandomPosition();
            ResetWalkTimer();
        }

        if (transform.position != _nextPosition)
        {
            if (_spriteRenderer.isVisible)
            {
                Move();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            OnPedestrianDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public void SetAnimationController(RuntimeAnimatorController controller)
    {
        _animator.runtimeAnimatorController = controller;
    }

    public void SetInitialNextPosition(Vector3 position)
    {
        _nextPosition = position;
    }

    private void ResetWalkTimer()
    {
        _walkTimer = UnityEngine.Random.Range(0f, _maxWalkTimer);
    }

    private void RandomPosition()
    {
        float moveX = UnityEngine.Random.Range(-_maxWalkDistance, _maxWalkDistance);
        float moveY = UnityEngine.Random.Range(-_maxWalkDistance, _maxWalkDistance);
        _animator.SetFloat("MoveX", moveX);
        _animator.SetFloat("MoveY", moveY);
        _nextPosition = new Vector3(moveX, moveY, 0f);
    }

    private void Move()
    {
        _rigidbody.MovePosition(transform.position + _nextPosition * Time.fixedDeltaTime);
    }
}
