using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooterController : MonoBehaviour
{
    public float moveSpeed;
    private float changeDirectionInterval = 2f;
    private float timeSinceLastDirectionChange = 0f;
    private float bulletForce;
    private float shootBulletTime;
    private float AttakCollTime;

    private Vector2 currentDirection;
   
    private GameObject player;
    private Animator animator;
    public GameObject bullet;
    private AudioSource _audioSource;
    void Start()
    {
        AttakCollTime = 3f;
        bulletForce = 5f;
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }
    private void Update()
    {
        timeSinceLastDirectionChange += Time.deltaTime;

        if (timeSinceLastDirectionChange >= changeDirectionInterval)
        {
            ChangeDirection();
            timeSinceLastDirectionChange = 0f;
        }

        MoveRandomly();

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        bool noObstacleBetween = !Physics2D.Linecast(transform.position, player.transform.position, LayerMask.GetMask("Obstacle"));

        if (noObstacleBetween && Time.time - shootBulletTime > AttakCollTime && distanceToPlayer < 5f)
        {
            StartCoroutine(ShootBullet());
            shootBulletTime = Time.time;
        }
    }
    IEnumerator ShootBullet()
    {
        animator.SetTrigger("OnAttack");
        yield return new WaitForSeconds(0.4f);
        Managers.Sound.EffectSoundChange("Sound_Enemy_PooterAttak");
        Vector3 direction = (player.transform.position - transform.position).normalized;
        var Bullet = EnemyBulletPoolManager.instance.Pool.Get();
        Bullet.transform.position = transform.position;
        Rigidbody2D rightBullet_rb = Bullet.GetComponent<Rigidbody2D>();
        rightBullet_rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    }
    private void MoveRandomly()
    {
        animator.SetTrigger("OnMove");
        transform.Translate(currentDirection.normalized * moveSpeed * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }
}
