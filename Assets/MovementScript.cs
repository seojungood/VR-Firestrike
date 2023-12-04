using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    public AnimationController animationScript;

    /// <summary>
    ///  Animation Value:
    ///   Death : -2
    ///   Idle :  -1 
    ///   Walking : 0
    ///   Attacking: 1
    /// </summary>
    float currentAnimationValue;

    float targetAnimationValue;

    Vector3 walkingDest;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        animationScript.setAnimationValue(currentAnimationValue);

        if (currentAnimationValue >= targetAnimationValue + 0.1 || currentAnimationValue <= targetAnimationValue - 0.1)
        {
            currentAnimationValue += 0.05f;
        }
        else
        {
            currentAnimationValue = targetAnimationValue;
        }

        if (walkingDest != null)
        {
            float dis = Vector3.Distance(transform.position, walkingDest); // Calculating Distance
            if (targetAnimationValue == 1 && dis > 0.5)
            {
                transform.position = transform.position + new Vector3(0.5f, 0.0f, 0.5f);
            }
        }
    }

    public void Walk(Vector3 destination)
    {
        walkingDest = destination;
    }

    public void setAnimationValue(float newVal)
    {
        targetAnimationValue = newVal;
    }
}
