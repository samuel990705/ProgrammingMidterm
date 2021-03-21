using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    private Vector2 screen;
    private float gunTimer;//timer until next gun
    private float randInt;//used to determine next gun to spawn
    private Vector3 tempVector;//used to radomize spawn location

    public GameObject pistol;
    public GameObject AR;
    public GameObject shotgun;

    private AudioSource audioSource;
    public AudioClip dropSFX;//played when gun spawns

    void Start()
    {
        gunTimer = Random.Range(4f, 8f);
        audioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
        tempVector = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        screen = Player.screen;
        gunTimer -= Time.deltaTime;

        if (gunTimer <= 0)
        {
            gunTimer = Random.Range(5f, 10f);
            tempVector.x = Random.Range(-screen.x, screen.x);
            tempVector.y = Random.Range(-screen.y, screen.y);
            randInt = Random.Range(0, 3);
            switch (randInt)
            {
                case 0:
                    Instantiate(pistol, tempVector, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(AR, tempVector, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(shotgun, tempVector, Quaternion.identity);
                    break;
            }
            audioSource.PlayOneShot(dropSFX, 0.8f);
        }
    }
}
