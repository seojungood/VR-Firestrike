using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 tentPos = new Vector3(-58.09f, 11.256f, 70.94f);

    public GameObject screen;
    public GameObject board;

    Vector3 startPos;
    public Camera myCam;
    Vector3 lastPos;

    bool inTent = false;

    Vector3 boardPos;


    void Start()
    {
        lastPos = startPos = myCam.transform.position;
        boardPos = new Vector3(board.transform.position.x, tentPos.y, board.transform.position.z);
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
                this.transform.RotateAround(myCam.transform.position, Vector3.up, 180);
                //this.transform.LookAt(boardPos, Vector3.up);
            }
            //lastPos = myCam.transform.position;
            else
            {
                this.transform.position -= (myCam.transform.position - tentPos);
                this.transform.RotateAround(myCam.transform.position, Vector3.up, 180);

                //this.transform.LookAt(screen.transform.position, Vector3.up);

            }

            inTent = !inTent;
        }
    }
}
