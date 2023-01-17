using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private int maxHealth;
    public int currentHealth;
    [Tooltip("Fire rate al secondo. 0.2 corrisponde a 5 colpi al secondo")][SerializeField] private float _fireRate;
    private float _timeToFire;
    [Tooltip("Distanza massima per cercare un nemico")][SerializeField] private float _attackRange;
    public LayerMask enemyLayerMask;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public int playerLevel;
    public float expToNextLevel;
    public float currentExp;

    private float _distance = 10f;
    private float _minDistance;
    private GameObject _shootingTarget;
    public Vector2 shootingDirection;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private PlayerUI _playerUI;
    private float _movement;

    public int myScore = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _playerUI = GetComponent<PlayerUI>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        InvokeRepeating("Shoot", _fireRate, _fireRate);
        _playerUI.SetMaxHealth(maxHealth);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _rb.velocity = ctx.ReadValue<Vector2>() * _speed;
    }

    private void Update()
    {
        _movement = _rb.velocity.magnitude;
        _animator.SetFloat("Movement", _movement);
        FlipSprite();
    }

    private void FlipSprite()
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

    private void Shoot()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _attackRange, enemyLayerMask);
        
        if(enemies.Length <= 0)
        {
            return;
        }

        _minDistance = _distance;

        foreach(Collider2D enemy in enemies)
        {
            float tempDistance = Vector3.Distance(transform.position, enemy.gameObject.transform.position);

            if (tempDistance <= _minDistance)
            {
                _minDistance = tempDistance;
                _shootingTarget = enemy.gameObject;
            }
        }

        if(_shootingTarget != null)
        {
            shootingDirection = _shootingTarget.transform.position - transform.position;
        }

        Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
    }

    public void AddPoint()
    {
        myScore++;
    }

    public void GainExperience()
    {
        currentExp++;

        if(currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        playerLevel++;
        currentExp = 0;
        expToNextLevel *= 1.25f;
        Debug.Log("Level Up! - Playere Levels: " + playerLevel);
    }

    public void TakeDamage()
    {
        currentHealth--;
        _playerUI.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Experience"))
        {
            GainExperience();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage"))
        {
            TakeDamage();
        }
    }

}
