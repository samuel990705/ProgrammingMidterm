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
        if(Mathf.Abs(transform.position.x)>screen.x+10 || Mathf.Abs(transform.position.y) > screen.y+10)//+10 for leeway so player can shoot hugging screen
        {
            Destroy(this.gameObject);
        }
    }
}
