using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1 : MonoBehaviour
{
    private const float speed = 2f;//speed of mob
    private const int maxHealth = 1;//max health

    private GameObject player;//cache player object
    private Vector3 direction;//direction mob will be moving
    private int health;//curent health


    public GameObject healthPrefab;//cache health text
    private GameObject healthObject;
    private TextMesh healthText;

    public AudioClip deathSFX;//audio to be played when dead
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();//used to play death SFX

        player = GameObject.FindWithTag("Player");
        direction = new Vector3();
        health = maxHealth;

        healthObject = Instantiate(healthPrefab, transform.position, Quaternion.identity);
        healthText = healthObject.GetComponent<TextMesh>();
        healthText.text = health + "/" + maxHealth;


    }

    void Update()
    {
        //update displayed health
        healthText.text = health + "/" + maxHealth; ;
        Vector3 pos = transform.position;
        pos.y += 0.6f;
        healthObject.transform.position = pos;

        //follow player
        direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        //update localScale
        Vector3 temp = transform.localScale;
        if (direction.x < 0)//if facing other way, flip mob
        {
            temp.x = -1 * Mathf.Abs(transform.localScale.x);
        }
        else
        {
            temp.x = Mathf.Abs(transform.localScale.x);
        }
        transform.localScale = temp;

        //if dead
        if (health <= 0)
        {
            audioSource.PlayOneShot(deathSFX, 1F);//play mob death effect
            GameObject.FindWithTag("Player").GetComponent<Player>().score += 1;//increment score
            Destroy(this.gameObject);//destroys self
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            Destroy(collision.gameObject);//destroy bullet
            health--;//decrement health

        }
    }

    private void OnDestroy()
    {
        Destroy(healthObject);//remove health label
    }
}
