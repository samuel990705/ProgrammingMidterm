using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    private Vector2 screen;
    private float heartTimer;//timer until next heart
    private Vector3 tempVector;//used to radomize spawn location

    public GameObject heart;
    private AudioSource audioSource;
    public AudioClip spawnSFX;//played when gun spawns

    void Start()
    {
        heartTimer = Random.Range(20f, 30f);
        audioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
        tempVector = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        screen = Player.screen;
        heartTimer -= Time.deltaTime;

        if (heartTimer <= 0)
        {
            heartTimer = Random.Range(20f, 30f);
            tempVector.x = Random.Range(-screen.x, screen.x);
            tempVector.y = Random.Range(-screen.y, screen.y);
            Instantiate(heart, tempVector, Quaternion.identity);
            audioSource.PlayOneShot(spawnSFX, 0.8f);
        }
    }
}
