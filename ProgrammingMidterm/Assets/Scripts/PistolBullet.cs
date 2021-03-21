using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    public Vector3 direction;//direction being fired
    private const float speed=10f;
    private Vector2 screen;


    void Start()
    {

    }

    void Update()
    {
        //reference screen in world space
        screen = Player.screen;

        //move bullet
        transform.position += direction * speed * Time.deltaTime;

        //destroy if no longer on screen
        if(Mathf.Abs(transform.position.x)>screen.x || Mathf.Abs(transform.position.y) > screen.y)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


    }
}
