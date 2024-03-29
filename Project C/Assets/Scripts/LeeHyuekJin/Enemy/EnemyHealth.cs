using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float hp;
    private Animator _animator;
    private GameObject _player;
    private PlayerStats _playerStats;
    public GameObject bloodPop;
    public float bloodPopoffset;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerStats = _player.GetComponent<PlayerStats>();
        _animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            TakeDamage(_playerStats.attackDamage);
            _animator.SetTrigger("OnHit");
        }
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Managers.Sound.BossSoundChange("Boss_Death");
            Collider2D collider = GetComponent<Collider2D>();
            if(bloodPop != null)
            {
                Instantiate(bloodPop, transform.position + Vector3.up * bloodPopoffset, transform.rotation);
            }
            Destroy(gameObject);
            //collider.enabled = false;
            Invoke("Dead", 0.6f);
            
        }
    }


    private void Dead()
    {
        Destroy(gameObject);
    }
}
