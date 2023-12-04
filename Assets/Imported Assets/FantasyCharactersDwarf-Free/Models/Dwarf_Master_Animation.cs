using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf_Master_Animation : AnimationController
{
    public Animator animator;
    public float DwarfMaterMovePos;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        DwarfMaterMovePos = -1;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("DwarfInput", DwarfMaterMovePos);

    }

    public override void SetAnimationValue(float val)
    {
        DwarfMaterMovePos = val;
    }
}
