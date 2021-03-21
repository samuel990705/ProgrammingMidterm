using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    private const float reloadTime = 3.3f;//time it takes to reload 
    private const float spread=22;//degree spread of shotgun fire

    private GameObject player;
    private bool active;//determines if gun is active (picked up)
    public int clip;//remaining clip
    public int maxClip;//ammo capacity
    private int reloadingDelay;//delay of reloading a gun
    private int fireDelay;//used to delay firerate
    private Vector3 mousePos;//World position of the mouse
    private Vector3 tempVector;//temporary Vector3 variable to used throughout code
    private Vector3 scale;//scale of object
    private Camera cam;

    public AudioClip fireSFX;//audio played when gun is fired
    public AudioClip loadSFX;//audio played when gun is reloaded
    public AudioClip cockSFX;//audio played when gun is cocked
    public AudioClip pickUpSFX;//audio played when gun is picked up

    public GameObject bullet;//prefab to spawn when shooting

    private AudioSource audioSource;
    private UI UI;//cache UI

    void Start()
    {
        maxClip = 8;
        clip = maxClip;

        player = GameObject.FindWithTag("Player");
        active = false;
        reloadingDelay = 0;
        fireDelay = 0;
        tempVector = new Vector3(0, 0, 0);

        UI = GameObject.FindWithTag("UI").GetComponent<UI>();
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
    }

    void Update()
    {
        if (active)//if gun is picked up by player
        {
            //Update UI
            UI.ammoCapacity = maxClip;
            UI.ammo = clip;

            //update mouse position
            tempVector.x = Input.mousePosition.x;
            tempVector.y = Input.mousePosition.y;
            tempVector.z = Mathf.Abs(cam.transform.position.z);//depth of camera
            mousePos = cam.ScreenToWorldPoint(tempVector);

            //update gun position
            tempVector = player.transform.position;
            tempVector.z = 0;//gun in front of player
            tempVector.y += -0.4f;//offsets gun higher
            transform.position = tempVector;

            //update gun orientation
            tempVector.x = 0;
            tempVector.y = 0;
            tempVector.z = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;//angle of gun in degrees
            scale = transform.localScale;
            if (Mathf.Abs(tempVector.z) > 90)//if gun is facing other way, flip gun
            {
                scale.y = -1 * Mathf.Abs(scale.y);
            }
            else
            {
                scale.y = Mathf.Abs(scale.y);
            }
            transform.localScale = scale;
            transform.rotation = Quaternion.AngleAxis(tempVector.z, Vector3.forward);//rotate gun

            //decrement reloading and fire delay
            reloadingDelay--;
            fireDelay--;

            //done reloading
            if (reloadingDelay%50==0 && reloadingDelay>0)//every 50 frames, load 1 shot
            {
                clip++;
                audioSource.PlayOneShot(loadSFX, 1F);

            }

            if (reloadingDelay == 0)//done loading all clips
            {
                fireDelay = 65;
                audioSource.PlayOneShot(cockSFX, 1F);
            }

            //fire while reloading: permitted but requires cocking shotgun first
            if (Input.GetMouseButton(0) && reloadingDelay > 0 && clip> 0)
            {
                fireDelay = 65;
                audioSource.PlayOneShot(cockSFX, 1F);
                reloadingDelay = -1;
            }

            //fire
            if (Input.GetMouseButton(0) && reloadingDelay < 0 && fireDelay <= 0)//if left click clicked (can be held) and not reloading
            {
                fireDelay = 65;
                audioSource.PlayOneShot(fireSFX, 0.6F);
                clip--;//decrement clip

                float finalRotation;
                Vector3 finalDirection;
                for(int i = 0; i < 10; i++)
                {
                    finalRotation = tempVector.z + Random.Range(-spread, spread);
                    GameObject g = Instantiate(bullet, transform.position, Quaternion.identity);//instantiate bullet prefab
                    ShotgunBullet p = g.GetComponent<ShotgunBullet>();//cache bullet
                    finalDirection= (mousePos - transform.position).normalized;
                    finalDirection.x += Mathf.Cos(Mathf.Deg2Rad * finalRotation);//change spread angle into directional vector
                    finalDirection.y += Mathf.Sin(Mathf.Deg2Rad * finalRotation);//change spread angle into directional vector
                    p.direction = finalDirection;//direction bullet is travelling
                    p.transform.rotation = Quaternion.AngleAxis(finalRotation, Vector3.forward);//rotate bullet
                }

                if (clip == 0)//if clip is empty, reload automatically
                {
                    fireDelay = 50;
                    reloadingDelay = (maxClip - clip) * 50 + 65;//time it take to fully reload all shells
                }
            }

            //manual reload (if not already reloading)
            if (Input.GetKeyDown("r") && reloadingDelay < 0)
            {
                reloadingDelay = (maxClip - clip) * 50 + 45;//time it take to fully reload all shells
            }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && !active)
        {
            //play pickup audio
            audioSource.PlayOneShot(pickUpSFX, 0.8F);

            //destroy active gun
            GameObject g = GameObject.FindWithTag("ActiveGun");
            Destroy(g);

            //set new gun to active
            transform.gameObject.tag = "ActiveGun";
            active = true;

        }

    }
}
