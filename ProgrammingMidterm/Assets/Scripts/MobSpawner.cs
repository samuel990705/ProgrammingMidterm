using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    private float timer;
    private float randFloat;

    public GameObject mob1;
    public GameObject mob2;
    public GameObject mob3;

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
            randFloat = Random.Range(0f, 1f);
            if (randFloat < 0.15)//15% chance to spawn big ogre
            {
                Instantiate(mob3, transform.position, Quaternion.identity);
            }
            else if (randFloat < 0.4)//25% chance of spawning medium zombie
            {
                Instantiate(mob2, transform.position, Quaternion.identity);
            }
            else//60% chance of small slime
            {
                Instantiate(mob1, transform.position, Quaternion.identity);
            }
        }
    }
}
