using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyController : MonoBehaviour {

    private float m_moveDist = 0.32f;
    private float m_timeDelta = 0.15f;
    private float m_targetDist;

    private bool CR_running = false;
    private bool m_isAxisInUse = false;
    private bool m_isInput = true;
    private bool m_overLockedDoor = false;
    public bool m_overWall = false;

    [HideInInspector] public bool m_setActive = false;

    [HideInInspector] public int m_activeCount = 0;

    private Vector3 m_priorLocation;

    public GameObject m_selectedObj = null;

    [SerializeField] private GameObject m_spyObj;
    private SpyMovement m_spyMovement;

    [SerializeField] private GameObject m_CGControllerObj;
    private CGController m_CGController;

    [SerializeField] private GameObject m_spyCamObj;
    private CameraControl m_camController;

    private void Awake()
    {
        m_spyMovement = m_spyObj.GetComponent<SpyMovement>();
        m_CGController = m_CGControllerObj.GetComponent<CGController>();
        m_camController = m_spyCamObj.GetComponent<CameraControl>();
    }

    void Update()
    {
        if(m_setActive)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            m_activeCount = 0;
            m_isInput = true;
            m_setActive = false;
        }

        if (m_isInput)
        {
            GetInput();
            m_targetDist = Vector2.Distance(m_spyObj.transform.position, transform.position);
        }
        if(m_activeCount >= 3)
        {
            m_isInput = false;
            m_activeCount = -1;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            m_camController.ShowCGCam();
            m_CGController.m_setActive = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            m_overWall = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            m_overWall = true;
        }
        else if (collision.tag == "LockedDoor")
        {
            m_selectedObj = collision.gameObject;
            m_overLockedDoor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            m_overWall = false;
        }
        else if (collision.tag == "LockedDoor")
        {
            m_overLockedDoor = false;
        }
    }

    private void GetInput()
    {

        if(Input.GetButtonDown("J1AButton") && m_targetDist < 0.97f && !m_overWall && !m_overLockedDoor && !m_spyMovement.CR_running)
        {
            StartCoroutine(m_spyMovement.SmoothMove(transform.position, m_timeDelta));
        }
        else if(Input.GetButtonDown("J1AButton") && m_targetDist < 0.97f && m_overLockedDoor && !m_spyMovement.CR_running)
        {
            m_selectedObj.GetComponent<LockedDoor>().m_spyHacked = true;
            m_activeCount += 1;
        }

        if (Input.GetAxisRaw("J1LeftVertical") < 0f && !CR_running)
        {
            if (m_isAxisInUse == false)
            {
                m_priorLocation = transform.position;

                Vector3 relativeLocation = new Vector3(0f, m_moveDist, 0f);
                Vector3 targetLocation = transform.position + relativeLocation;
                float timeDelta = 0.15f;

                StartCoroutine(SmoothMove(targetLocation, timeDelta));

                RayCastCheck(transform.up);
                m_isAxisInUse = true;
            }

        }
        else if (Input.GetAxisRaw("J1LeftVertical") > 0f && !CR_running)
        {
            if (m_isAxisInUse == false)
            {
                m_priorLocation = transform.position;

                Vector3 relativeLocation = new Vector3(0f, m_moveDist, 0f);
                Vector3 targetLocation = transform.position - relativeLocation;
                float timeDelta = 0.15f;

                StartCoroutine(SmoothMove(targetLocation, timeDelta));

                RayCastCheck(-transform.up);
                m_isAxisInUse = true;
            }

        }
        else if (Input.GetAxisRaw("J1LeftHorizontal") > 0f && !CR_running)
        {
            if (m_isAxisInUse == false)
            {
                m_priorLocation = transform.position;

                Vector3 relativeLocation = new Vector3(m_moveDist, 0f, 0f);
                Vector3 targetLocation = transform.position + relativeLocation;
                float timeDelta = 0.15f;

                StartCoroutine(SmoothMove(targetLocation, timeDelta));

                RayCastCheck(transform.right);
                m_isAxisInUse = true;
            }

        }
        else if (Input.GetAxisRaw("J1LeftHorizontal") < 0f && !CR_running)
        {
            if (m_isAxisInUse == false)
            {
                m_priorLocation = transform.position;

                Vector3 relativeLocation = new Vector3(m_moveDist, 0f, 0f);
                Vector3 targetLocation = transform.position - relativeLocation;
                float timeDelta = 0.15f;

                StartCoroutine(SmoothMove(targetLocation, timeDelta));

                RayCastCheck(-transform.right);
                m_isAxisInUse = true;
            }

        }

        if(Input.GetAxisRaw("J1LeftVertical") == 0f && Input.GetAxisRaw("J1LeftHorizontal") == 0f)
        {
            m_isAxisInUse = false;
        }
    }

    IEnumerator SmoothMove(Vector3 target, float delta)
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

        CR_running = false;
    }

    private void RayCastCheck(Vector3 rayDirection)
    {
        Ray ray = new Ray(transform.position, rayDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_moveDist))
        {
            if (hit.transform.tag == "Wall")
            {
                StopAllCoroutines();
                StartCoroutine(SmoothMove(m_priorLocation, m_timeDelta));
            }
        }
    }
}
