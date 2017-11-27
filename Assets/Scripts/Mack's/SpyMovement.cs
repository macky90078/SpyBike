using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyMovement : MonoBehaviour {

    [HideInInspector] public bool CR_running = false;

    [SerializeField] private GameObject m_spyControllerObj;

    private bool m_isAction = false;

    private SpyController m_spyController;

    private void Awake()
    {
        m_spyController = m_spyControllerObj.GetComponent<SpyController>();
    }

    // Update is called once per frame
    void Update ()
    {
		if(m_isAction)
        {
            m_spyController.m_activeCount += 1;
            m_isAction = false;
        }
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

        m_isAction = true;

        CR_running = false;
    }
}
