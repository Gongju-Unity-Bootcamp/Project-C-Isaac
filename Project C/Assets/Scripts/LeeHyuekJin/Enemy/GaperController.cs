using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GaperController : MonoBehaviour
{
    private GameObject player;
    private new SpriteRenderer renderer;
    private Animator _animator;
    private Rigidbody2D _rb;
    private AudioSource _audioSource;   
    
    private Vector2 direction;
    
    [SerializeField] private float detectionRange = 3f;
    [SerializeField] private float moveSpeed;
    
    private bool OnAttak = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
       
        if (!OnAttak && distanceToPlayer < detectionRange)
        {
            _animator.SetTrigger("OnHit");
            OnAttak = true;
            Move();
        }
        else if(OnAttak)
        {
            Move();

            if (player.transform.position.x + 1f > transform.position.x && player.transform.position.x - 1f < transform.position.x)
            {
                if (player.transform.position.y > transform.position.y || player.transform.position.y < transform.position.y)
                {
                    _animator.SetTrigger("MoveIdle");
                }
            }
            else
            {
                _animator.SetTrigger("MoveSide");
            }
        }
    }

    private void FixedUpdate()
    {
        Flip();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!OnAttak && collision.gameObject.CompareTag("PlayerBullet"))
        {
            OnAttak = true;
        }
    }
    private void Move()
    {
        _audioSource.Play();
        direction = player.transform.position - transform.position;
        _rb.velocity = direction.normalized * moveSpeed;
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }
    }
}