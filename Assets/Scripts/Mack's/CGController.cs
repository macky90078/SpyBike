using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGController : MonoBehaviour {

    private float m_moveDist = 0.32f;
    private float m_timeDelta = 0.15f;
    private float m_targetDist;

    public int m_activeCount = 0;

    private bool CR_running = false;
    private bool m_isAxisInUse = false;
    private bool m_pressedSelect = false;
    private bool m_isInput = false;

    [HideInInspector] public bool m_setActive = false;

    private Vector3 m_priorLocation;

    [SerializeField] private GameObject m_spyControllerObj;
    private SpyController m_spycontroller;

    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        m_spycontroller = m_spyControllerObj.GetComponent<SpyController>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_setActive)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            m_activeCount = 0;
            m_isInput = true;
            m_setActive = false;
        }

        if (m_isInput)
        {
            GetInput();
        }
        if(m_activeCount >=3)
        {
            m_isInput = false;
            m_activeCount = -1;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            m_spycontroller.m_setActive = true;
        }

	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_pressedSelect && collision.tag == "LockedDoor")
        {
            collision.GetComponent<LockedDoor>().m_CGHacked = true;
            m_activeCount += 1;
            m_pressedSelect = false;
        }
        if (collision.tag == "LockedDoor")
        {
            Debug.Log("On Locked Door!");
        }
    }

    private void GetInput()
    {

        //if (Input.GetButtonDown("J1AButton") && m_targetDist < 0.97f && !CR_running)
        //{
            
        //}
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_pressedSelect = true;
        }


        if (Input.GetKey(KeyCode.UpArrow) && !CR_running)
        {
            //if (m_isAxisInUse == false)
            //{
                m_priorLocation = transform.position;

                Vector3 relativeLocation = new Vector3(0f, m_moveDist, 0f);
                Vector3 targetLocation = transform.position + relativeLocation;
                float timeDelta = 0.15f;

                StartCoroutine(SmoothMove(targetLocation, timeDelta));

               // RayCastCheck(transform.up);
            //    m_isAxisInUse = true;
            //}

        }
        else if (Input.GetKey(KeyCode.DownArrow) && !CR_running)
        {
            //if (m_isAxisInUse == false)
            //{
                m_priorLocation = transform.position;

                Vector3 relativeLocation = new Vector3(0f, m_moveDist, 0f);
                Vector3 targetLocation = transform.position - relativeLocation;
                float timeDelta = 0.15f;

                StartCoroutine(SmoothMove(targetLocation, timeDelta));

               // RayCastCheck(-transform.up);
            //    m_isAxisInUse = true;
            //}

        }
        else if (Input.GetKey("right") && !CR_running)
        {
            //if (m_isAxisInUse == false)
            //{
                m_priorLocation = transform.position;

                Vector3 relativeLocation = new Vector3(m_moveDist, 0f, 0f);
                Vector3 targetLocation = transform.position + relativeLocation;
                float timeDelta = 0.15f;

                StartCoroutine(SmoothMove(targetLocation, timeDelta));

               // RayCastCheck(transform.right);
            //    m_isAxisInUse = true;
            //}

        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !CR_running)
        {
            //if (m_isAxisInUse == false)
            //{
                m_priorLocation = transform.position;

                Vector3 relativeLocation = new Vector3(m_moveDist, 0f, 0f);
                Vector3 targetLocation = transform.position - relativeLocation;
                float timeDelta = 0.15f;

                StartCoroutine(SmoothMove(targetLocation, timeDelta));

              //  RayCastCheck(-transform.right);
            //    m_isAxisInUse = true;
            //}

        }

        //if (Input.GetAxisRaw("J1LeftVertical") == 0f && Input.GetAxisRaw("J1LeftHorizontal") == 0f)
        //{
        //    m_isAxisInUse = false;
        //}
    }


    public IEnumerator SmoothMove(Vector3 target, float delta)
    {
        CR_running = true;
        float closeEnough = 0.2f;
        float distance = (transform.position - target).magnitude;

        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (distance >= closeEnough)
        {
            transform.position = Vector3.Lerp(transform.position, target, delta);
            yield return wait;

            distance = (transform.position - target).magnitude;
        }

        transform.position = target;

        yield return new WaitForSeconds(0.1f);
        CR_running = false;
    }
}
