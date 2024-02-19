using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 방향키 입력을 받아 캐릭터 움직임과는 별개로 공격을 실행하는 스크립트
public class Player_Attack : MonoBehaviour
{
    bool isAttack;
    // 스탯
    float bulletSpeed = 8.0f;
    public static float cooltime = 0.4f;
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Shoot(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Shoot(Vector2.down);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Shoot(Vector2.right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Shoot(Vector2.left);
        }
    }
    void Shoot(Vector2 direction)
    {
        if (!isAttack && Player_Move.gameState == "playing")
        {
            GameObject bullet = Player_ObjectPooling.instance.GetBulletPool();

            Player_Bullet bulletScript = bullet.GetComponent<Player_Bullet>();

            bullet.transform.position = transform.position;
            Rigidbody2D bulletRbody = bullet.GetComponent<Rigidbody2D>();
            bulletRbody.velocity = direction * bulletSpeed;

            isAttack = true;
            Invoke("AttackDelay", cooltime);
        }
    }
    void AttackDelay()
    {
        isAttack = false;
    }
}