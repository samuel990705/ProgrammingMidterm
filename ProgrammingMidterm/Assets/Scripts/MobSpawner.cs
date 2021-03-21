using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    private float timer;
    private int randInt;

    public GameObject mob1;

    void Start()
    {
        timer = Random.Range(3f, 8f);//random spawn interval
    }


    void Update()
    {
        timer -= Time.deltaTime;//decrement timer

        if (timer < 0)
        {
            timer = Random.Range(3f, 8f);//reset timer
            randInt = Random.Range(0, 1);
            switch (randInt)
            {
                case 0:
                    Instantiate(mob1, transform.position, Quaternion.identity);
                    break;
            }
        }
    }
}
