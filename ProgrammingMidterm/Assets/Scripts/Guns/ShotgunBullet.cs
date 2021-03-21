using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public Vector3 direction;//direction being fired
    private const float speed = 10f;
    private Vector2 screen;

    private float lifespan;

    void Start()
    {
        lifespan = Random.Range(0.2f, 0.5f);
    }

    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
        {
            Destroy(this.gameObject);
        }

        //reference screen in world space
        screen = Player.screen;

        //move bullet
        transform.position += direction * speed * Time.deltaTime;

        //destroy if no longer on screen
        if (Mathf.Abs(transform.position.x) > screen.x+10 || Mathf.Abs(transform.position.y) > screen.y+10)//+10 for leeway so player can shoot hugging screen
        {
            Destroy(this.gameObject);
        }
    }
}
