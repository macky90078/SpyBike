using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit2 : MonoBehaviour {
    public Transform target1;
    public Transform target2;
    public Transform target3;
    public Transform target4;


    public bool goTarget1 = true;
    public bool goTarget2 = false;
    public bool goTarget3 = false;
    public bool goTarget4 = false;


    public Transform[] targets;
    float speed = 2;
    Vector3[] path;
    int targetIndex;
    int targetsIndex;
    private bool hitTarget = false;

    void Start()
    {
        // PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void Update()
    {

        if (goTarget1)
        {
            PathRequestManager.RequestPath(transform.position, target1.position, OnPathFound);
        }
        else if (goTarget2)
        {
            PathRequestManager.RequestPath(transform.position, target2.position, OnPathFound);
        }
        else if (goTarget3)
        {
            PathRequestManager.RequestPath(transform.position, target3.position, OnPathFound);
        }
        else if (goTarget4)
        {
            PathRequestManager.RequestPath(transform.position, target4.position, OnPathFound);
        }


        //if (hitTarget)
        //{
        //    Patrol();
        //}
    }

    //private void Patrol()
    //{

    //    for (int targetsIndex = 0; targetsIndex < targets.Length; targetsIndex++)
    //    {
    //        int index = System.Array.IndexOf(targets, targets.Length - 1);
    //        if (index == targets.Length - 1)
    //        {
    //            targetsIndex = 0;
    //        }
    //        hitTarget = false;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Target")
        {
            goTarget1 = false;
            goTarget2 = true;
        }
        else if (collision.tag == "Target2")
        {
            goTarget2 = false;
            goTarget3 = true;
        }
        else if (collision.tag == "Target3")
        {
            goTarget3 = false;
            goTarget4 = true;
        }
        else if (collision.tag == "Target4")
        {
            goTarget4 = false;
            goTarget1 = true;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(.15f, .15f, .01f));

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
