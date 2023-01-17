using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _speed = 10f;
    [SerializeField] private float _timeToLive = 2f;

    private PlayerController _playerController;
    private Vector3 _direction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerController = FindObjectOfType<PlayerController>();
        _direction = _playerController.shootingDirection.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, _timeToLive);
        float rotationZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

    }

    private void Update()
    {
        _rb.velocity = _direction * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")){
            EnemyController enemy = collision.GetComponent<EnemyController>();
            enemy.TakeDamage();

            Destroy(gameObject);
        }
    }
}
