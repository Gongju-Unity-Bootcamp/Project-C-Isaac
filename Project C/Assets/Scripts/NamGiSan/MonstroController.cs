﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonstroController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private new SpriteRenderer renderer;
    private new Collider2D collider;
    private Animator animaotr;
    private BossHealth bossHp;
    private Vector2 savePos;

    public Transform bulletPoint;

    [SerializeField] private float shockwaveTime = 0.75f;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animaotr = GetComponent<Animator>();
        bossHp = GetComponent<BossHealth>();
    }

    void Start()
    {
        StartCoroutine(RandomState());
    }

    void FixedUpdate()
    {
        Flip();
    }

    void Flip()
    {
        if (player.transform.position.x > transform.position.x)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }
    }

    IEnumerator RandomState()
    {
        savePos = transform.position;
        yield return null;

        int randomAction = Random.Range(0, 3);

        switch (randomAction)
        {
            case 0:
                StartCoroutine(Chase());
                break;
            case 1:
                StartCoroutine(HighJump());
                break;
            case 2:
                StartCoroutine(attackReady());
                break;
        }
    }


    IEnumerator Chase()
    {
        if(GetComponent<BossHealth>().hp <= 0)
        {
            transform.position = savePos;
            StartCoroutine(RandomState());
        }

        else
        {
            animaotr.SetTrigger("Chase");

            // 대기동작 시간
            yield return new WaitForSeconds(0.125f);

            collider.enabled = false;
            Vector2 startPos = transform.position;
            Vector2 targetPos = player.transform.position;

            float moveTime = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime < moveTime)
            {
                transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 공중동작 시간
            yield return new WaitForSeconds(0.5f);

            transform.position = targetPos;
            rb.velocity = Vector2.zero;
            collider.enabled = true;

            yield return new WaitForSeconds(0.75f);  // 착지동작 시간
            StartCoroutine(RandomState());
        }       
    }

    IEnumerator HighJump()
    {
        if (GetComponent<BossHealth>().hp <= 0)
        {
            transform.position = savePos;
            StartCoroutine(RandomState());
        }

        else
        {
            animaotr.SetTrigger("HighJump");
            yield return new WaitForSeconds(0.5f); // 준비 동작 대기시간

            collider.enabled = false;
            rb.velocity = new Vector2(0, 70f);
            yield return new WaitForSeconds(0.25f); // 점프 애니메이션 시간

            Vector2 targetPos = player.transform.position;

            while (true)
            {
                if (transform.position.y >= 70)
                {
                    rb.velocity = Vector2.zero;
                    break;
                }
                yield return null;
            }
            yield return new WaitForSeconds(0.5f); // 공중 대기시간

            Vector2 airPos = new Vector2(targetPos.x, transform.position.y);

            while (true)
            {
                float moveTime = 0.5f;
                float elapsedTime = 0f;

                while (elapsedTime < moveTime)
                {
                    transform.position = Vector2.Lerp(airPos, targetPos, elapsedTime / moveTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = new Vector2(targetPos.x, targetPos.y); // 완전한 착지
                Managers.Sound.BossSoundChange("Sound_Enemy_BossDiveAttack");
                // audio Dive

                int spawnBullet = Random.Range(30, 35);

                if (transform.position.y == targetPos.y)
                {
                    collider.enabled = true;
                    rb.velocity = Vector2.zero;
                    transform.position = targetPos;
                    for (int i = 0; i < spawnBullet; i++)
                    {
                        int bulletSpeed = Random.Range(10, 12);
                        var bossBullet = EnemyBulletPoolManager.instance.Pool.Get();
                        GameObject shockWave = transform.GetChild(1).gameObject;
                        shockWave.GetComponent<ShockWave>().CallShockWave();
                        bossBullet.transform.position = bulletPoint.position;
                        Rigidbody2D rb = bossBullet.GetComponent<Rigidbody2D>();
                        Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                        rb.AddForce(ranVec * bulletSpeed, ForceMode2D.Impulse);
                        rb.gravityScale = 0.3f;
                    }
                    break;
                }
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            StartCoroutine(RandomState());
        }      
    }

    IEnumerator attackReady()
    {
        animaotr.SetTrigger("SpitReady");
        Managers.Sound.BossSoundChange("Sound_Enemy_BossIdle");
        //Idle audio
        yield return new WaitForSeconds(1f);

        int randomJump = Random.Range(0, 2);

        if (player != null && bossHp.hp > 0)
        {
            switch (randomJump)
            {
                case 0:
                    StartCoroutine(SpitAttack());
                    break;
                case 1:
                    animaotr.SetTrigger("Idle");
                    StartCoroutine(RandomState());
                    break;
            }
        }

        else
        {
            collider.enabled = false;
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator SpitAttack()
    {
        animaotr.SetTrigger("Spit");
        yield return new WaitForSeconds(1.0f);  // 원거리 공격 전 대기 동작
                                                //공격 사운드

        Managers.Sound.BossSoundChange("Sound_Enemy_BossAttack");
        //audio attack
        int spawnBullet = Random.Range(30, 35);

        while (true)
        {
            for (int i = 0; i < spawnBullet; i++)
            {
                int bulletSpeed = Random.Range(6, 8);
                var bossBullet = EnemyBulletPoolManager.instance.Pool.Get();
                bossBullet.transform.position = bulletPoint.position;
                Rigidbody2D rb = bossBullet.GetComponent<Rigidbody2D>();
                Vector2 dirVec = (player.transform.position - transform.position).normalized;
                Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f));
                dirVec += ranVec;
                rb.AddForce(dirVec * bulletSpeed, ForceMode2D.Impulse);
                rb.gravityScale = 0.5f;
            }
            break;
        }


        yield return new WaitForSeconds(1.5f);
        StartCoroutine(RandomState());
    }
}