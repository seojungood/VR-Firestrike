using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    public AnimationController animationScript;

    public GameObject thisO;

    /// <summary>
    ///  Animation Value:
    ///   Death : -2
    ///   Idle :  -1 
    ///   Walking : 0
    ///   Attack : 1
    /// </summary>
    float currentAnimationValue;
    float targetAnimationValue;
    float timer;

    Vector3 walkingDest;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("current value " + currentAnimationValue);
        animationScript.SetAnimationValue(currentAnimationValue);
        timer -= Time.deltaTime;

        if (currentAnimationValue == -2 && timer < 0.0f)
        {
            thisO.SetActive(false);
        }

        if (timer < 0.0f && timer > -1.0f && targetAnimationValue != 0)
        {
            targetAnimationValue = -1;
        }

        if (currentAnimationValue <= targetAnimationValue - 0.2)
        {
            currentAnimationValue += 0.1f;
            timer = 3.0f;
        }
        else if (currentAnimationValue >= targetAnimationValue + 0.2)
        {
            currentAnimationValue -= 0.1f;
            timer = 3.0f;
        }
        else
        {
            currentAnimationValue = targetAnimationValue;
        }

        if (walkingDest != null && targetAnimationValue == 0)
        {
            float dis = Vector3.Distance(transform.position, walkingDest); // Calculating Distance
            if (dis > 0.25)
            {
                float xDiff = Mathf.Clamp(walkingDest.x - transform.position.x, -0.1f, 0.1f);
                float zDiff = Mathf.Clamp(walkingDest.z - transform.position.z, -0.1f, 0.1f);
                transform.position = transform.position + new Vector3(xDiff, 0.0f, zDiff);

                transform.LookAt(walkingDest);
            }
            else
            {
                targetAnimationValue = -1;
            }
        }
    }

    public void Walk(Vector3 destination)
    {
        targetAnimationValue = 0;
        walkingDest = destination;
    }

    public void Attack()
    {
        targetAnimationValue = 1;
        timer = 3.0f;
    }

    public void setAnimationValue(float newVal)
    {
        Debug.Log("setting animation value: " + newVal);
        targetAnimationValue = newVal;
        timer = 3.0f;
    }
}
