using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private BoxCollider2D m_wallCollider;
    private Rigidbody2D m_body2D;

    private void Awake()
    {
        m_wallCollider = GetComponent<BoxCollider2D>();
        m_body2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            Destroy(m_wallCollider);
            Destroy(m_body2D);
        }
    }
}
