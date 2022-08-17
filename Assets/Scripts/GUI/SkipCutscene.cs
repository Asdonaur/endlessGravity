using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCutscene : MonoBehaviour
{
    Animator animator;
    public bool blSkip = false;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("show", false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown)
        {
            if (blSkip)
            {
                GameManager.instance.Menu();
            }
            else
            {
                blSkip = true;
                animator.SetBool("show", true);
            }
        }
    }
}
