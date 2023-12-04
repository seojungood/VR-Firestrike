using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusaderAnimation : MonoBehaviour
{
    
    public Animator animator;
    public float CrusaderMovePos;
    // Start is called before the first frame update
    void Start()
    {
        animator= this.gameObject.GetComponent<Animator>();
        CrusaderMovePos = -1;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("CrusaderMove", CrusaderMovePos);
        
    }
}
