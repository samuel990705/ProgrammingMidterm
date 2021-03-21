using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{

    private const float reloadTime = 2f;//time it takes to reload 

    private GameObject player;
    private bool active;//determines if gun is active (picked up)
    public int clip;//remaining clip
    public int maxClip;//ammo capacity
    private float reloadingDelay;//delay of reloading a gun
    private Vector3 mousePos;//World position of the mouse
    private Vector3 tempVector;//temporary Vector3 variable to used throughout code
    private Vector3 scale;//scale of object
    private Camera cam;

    public AudioClip fireSFX;//audio played when gun is fired
    public AudioClip reloadSFX;//audio played when gun is reloaded
    public AudioClip pickUpSFX;//audio played when gun is picked up

    public GameObject bullet;//prefab to spawn when shooting

    private AudioSource audioSource;
    private UI UI;//cache UI

    private void Awake()//needs to be initialized before Canvas.Start();
    {
        maxClip = 12;
        clip = maxClip;
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        active = false;
        reloadingDelay = 0.1f;
        tempVector = new Vector3(0, 0, 0);

        UI = GameObject.FindWithTag("UI").GetComponent<UI>();
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;

        if(transform.gameObject.tag == "ActiveGun")//game starts with 1 active gun
        {
            active = true;
        }
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
            tempVector.z = Mathf.Atan2(mousePos.y-transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;//angle of gun in degrees
            scale = transform.localScale;
            if (Mathf.Abs(tempVector.z) > 90)//if gun is facing other way, flip gun
            {
                scale.y = -1*Mathf.Abs(scale.y);  
            }
            else
            {
                scale.y = Mathf.Abs(scale.y);
            }
            transform.localScale = scale;
            transform.rotation = Quaternion.AngleAxis(tempVector.z, Vector3.forward);//rotate gun

            //decrement reloading delay
            reloadingDelay -= Time.deltaTime;

            //done reloading
            if (reloadingDelay < 0 && clip==0)
            {
                clip = maxClip;
                
            }

            //fire
            if (Input.GetMouseButtonDown(0) && reloadingDelay<0)//if left click clicked (only triggered once if held) and not reloading
            {
                audioSource.PlayOneShot(fireSFX, 0.6F);
                clip--;//decrement clip
                GameObject g= Instantiate(bullet, transform.position, Quaternion.identity);//instantiate bullet prefab
                PistolBullet p = g.GetComponent<PistolBullet>();//cache bullet
                p.direction = (mousePos - transform.position).normalized;//direction bullet is travelling
                p.transform.rotation= Quaternion.AngleAxis(tempVector.z, Vector3.forward);//rotate bullet

                if (clip == 0)//if clip is empty, reload automatically
                {
                    reload();
                }
            }

            //manual reload (if not already reloading)
            if (Input.GetKeyDown("r") && reloadingDelay<0)
            {
                reload();
            }

        }

    }

    //reloads gun
    void reload()
    {
        audioSource.PlayOneShot(reloadSFX, 0.6F);
        reloadingDelay = reloadTime;
        clip = 0;
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
