using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsemanAnimation : AnimationController
{
    public Animator animator;
    public float HorsemanMovePos;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        HorsemanMovePos = -1;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("HorseInput", HorsemanMovePos);

    }
}
