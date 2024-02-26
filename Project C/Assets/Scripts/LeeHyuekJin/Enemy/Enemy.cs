using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject blood;
    public GameObject[] heart;
    public static event Action OnEnemySpawned;
    public static event Action OnEnemyDestroyed;
    private void Start()
    {
        OnEnemySpawned?.Invoke();
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            int randomNum = UnityEngine.Random.Range(0, 100);
            if (blood != null)
            {
                Instantiate(blood, transform.position, Quaternion.identity);
            }
            if(heart !=null && randomNum < 3)
            {
                Instantiate(heart[0], transform.position, Quaternion.identity);
            }
            else if(heart != null && randomNum < 10)
            {
                Instantiate(heart[1], transform.position, Quaternion.identity);
            }
            OnEnemyDestroyed?.Invoke();
        }
    }

}
