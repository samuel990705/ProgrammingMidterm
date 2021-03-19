using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const float speed=0.1f;//speed of chacaracter
    private float xSpeed;//to be added to x-coord
    private float ySpeed;//to be added to y-coord
    private Vector3 tempAdd;//temporary variable to add to transform.position
    public Animator animator;//animator for player
    // Start is called before the first frame update
    void Start()
    {
        xSpeed = 0;
        ySpeed = 0;

        tempAdd = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //reset speeds
        xSpeed = 0;
        ySpeed = 0;

        //2D movement
        if (Input.GetKey("w"))
        {
            Debug.Log("pressed");
            ySpeed = speed;
        }
        if (Input.GetKey("s"))
        {
            ySpeed = -speed;
        }
        if (Input.GetKey("a"))
        {
            xSpeed = -speed;
        }
        if (Input.GetKey("d"))
        {
            xSpeed = speed;
        }
        tempAdd.x = xSpeed;
        tempAdd.y = ySpeed;
        Debug.Log(tempAdd);
        transform.position +=tempAdd;

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
}
