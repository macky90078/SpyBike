using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour {

     public bool m_CGHacked = false;
     public bool m_spyHacked = false;

    // Update is called once per frame
    void Update ()
    {
	    if(m_CGHacked && m_spyHacked)
        {
            gameObject.SetActive(false);
        }
	}
}
