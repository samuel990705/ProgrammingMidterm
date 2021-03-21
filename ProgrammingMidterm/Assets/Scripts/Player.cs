using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Player : MonoBehaviour
{
    public const float speed=0.1f;//speed of chacaracter when moving
    private float xSpeed;//to be added to x-coord
    private float ySpeed;//to be added to y-coord
    private Vector3 mousePos;//World position of the mouse
    private Vector3 tempVector;//temporary Vector3 variable to used throughout code
    public int health;//health of player
    public int score;//total score

    public Animator animator;//animator for player
    private Camera cam;

    public static Vector2 screen;

    private UI UI;//cache UI
    private AudioSource audioSource;
    public AudioClip hurtSFX;//audio played when player takes damage
    public AudioClip startSFX;//audio played at the very start
    public AudioClip heartSFX;//audio played when a heart is picked up

    private void Awake()
    {
        health = 3;//health needs to be set before UI.Start();
    }
    void Start()
    {

        xSpeed = 0;
        ySpeed = 0;
        tempVector = new Vector3(0, 0, 0);

        UI= GameObject.FindWithTag("UI").GetComponent<UI>();
        audioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();//used to play death SFX
        audioSource.PlayOneShot(startSFX, 0.7F);//play round-start SFX

        cam = Camera.main;
        screen = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Mathf.Abs(cam.transform.position.z)));
    }


    void Update()
    {
        //update UI
        UI.score = this.score;
        UI.health = this.health;

        //variable updates
        xSpeed = 0;
        ySpeed = 0;
        screen = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Mathf.Abs(cam.transform.position.z)));//in case screen is resized

        //update mouse position
        tempVector.x = Input.mousePosition.x;
        tempVector.y = Input.mousePosition.y;
        tempVector.z = Mathf.Abs(cam.transform.position.z);//depth of camera
        mousePos = cam.ScreenToWorldPoint(tempVector);

        //2D movement
        if (Input.GetKey("w"))
        {
            ySpeed += speed;
        }
        if (Input.GetKey("s"))
        {
            ySpeed += -speed;
        }
        if (Input.GetKey("a"))
        {
            xSpeed += -speed;
        }
        if (Input.GetKey("d"))
        {
            xSpeed += speed;
        }
        tempVector.x = Mathf.Clamp(xSpeed + transform.position.x, -1 * screen.x, screen.x);
        tempVector.y = Mathf.Clamp(ySpeed + transform.position.y, -1 * screen.y, screen.y);
        tempVector.z = 0;
        transform.position = tempVector;

        //flip character depending on mouse position
        tempVector = transform.localScale;
        if (transform.position.x < mousePos.x)//when mouse is to the right of player
        {
            tempVector.x = Mathf.Abs(tempVector.x);
        }
        else//when mouse is to the left of player
        {
            tempVector.x = -1*Mathf.Abs(tempVector.x);//flip character sprite
        }
        transform.localScale = tempVector;

        //if deead
        if (health <= 0)
        {
            //write new score into text file
            string s = "\n"+System.DateTime.Now.ToString("yyyy/MM/dd HH:mm") + "," + score;
            string path = Application.dataPath + "/scoreboard.txt";
            File.AppendAllText(path, s);

            //switch to scoreboard scene
            SceneManager.LoadScene("Scoreboard");
        }

        //sets animation to moving when speed!=0
        if (xSpeed!=0 || ySpeed != 0)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Mob")
        {
            Destroy(collision.gameObject);//destroy Mob
            health--;//decrement health
            audioSource.PlayOneShot(hurtSFX, 0.7F);//play hurt SFX

        }

        if(collision.collider.tag == "Heart")
        {
            health++;
            audioSource.PlayOneShot(heartSFX, 0.7F);//play hurt SFX
            Destroy(collision.gameObject);
        }

    }
}
