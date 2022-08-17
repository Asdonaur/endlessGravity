using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenesScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        int cinem = PlayerPrefs.GetInt("cinemIndex", 0);

        switch (cinem)
        {
            case 0:
                animator.Play("Intro");
                break;

            case 1:
                animator.Play("Ending1");
                break;

            case 2:
                animator.Play("Ending2");
                break;
        }
    }
}
