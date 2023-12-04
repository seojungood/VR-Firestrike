using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 tentPos = new Vector3(-58.09f, 11.256f, 70.94f);
    Vector3 startPos;
    public Camera myCam;
    Vector3 lastPos;

    bool inTent = false;


    void Start()
    {
        lastPos = startPos = myCam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (inTent)
            {
                //put player into the startPos
                this.transform.position -= (myCam.transform.position - startPos);
            }
            //lastPos = myCam.transform.position;
            else
            {
                this.transform.position -= (myCam.transform.position - tentPos);
            }

            inTent = !inTent;
        }
    }
}
