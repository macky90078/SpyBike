using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobAI : MonoBehaviour {

    public Transform target;
    public Transform target2;
    public Transform target3;
    public Transform target4;
    public float speed;
    private bool move1 = true;
    private bool move2 = false;
    private bool move3 = false;
    private bool move4 = false;



    void start()
    {

    }

    void Update()
    {
        move();
    }

    private void move()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow)))

        {
            float step = speed * Time.deltaTime;
            if (move1 == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            }
            if (move1 == false)
            {
                print("off");
            }

            if (move2 == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, target2.position, step);
            }
            if (move1 == false)
            {
                print("off");
            }
            if (move3 == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, target3.position, step);
            }
            if (move1 == false)
            {
                print("off");
            }
            if (move4 == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, target4.position, step);
            }
            if (move1 == false)
            {
                print("off");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "node1")
        {
            move1 = false;
            move2 = true;
            Debug.Log("Something has entered this zone.");
        }


        if (other.gameObject.tag == "node2")
        {
            move2 = false;
            move3 = true;
        }


        if (other.gameObject.tag == "node3")
        {
            move3 = false;
            move4 = true;
        }

        if (other.gameObject.tag == "node4")
        {
            move4 = false;
            move1 = true;
        }


    }
}
