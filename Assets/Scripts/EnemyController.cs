using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private int _maxHealth;
    public int currentHealth;
    public UIManager manager;
    public GameObject experiencePrefab;

    private float _movement;

    private Rigidbody2D _rb;
    private PlayerController _target;
    private Vector3 _direction;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = FindObjectOfType<PlayerController>();
        currentHealth = _maxHealth;
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        _movement = _rb.velocity.magnitude;
        _animator.SetFloat("Movement", _movement);
        SpriteFlip();
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        if(_target != null)
        {
            _direction = _target.transform.position - transform.position;
            _rb.velocity = new Vector2(_direction.x, _direction.y).normalized * _speed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }

        
    }

    private void SpriteFlip()
    {
        if (_rb.velocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_rb.velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        if(currentHealth <= 0)
        {
            _target.AddPoint();
            Death();
        }
    }
    
    public void Death()
    {
        Instantiate(experiencePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
