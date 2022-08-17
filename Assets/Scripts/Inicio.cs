using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inicio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(act());
    }

    IEnumerator act()
    {
        yield return null;
        GameManager.instance.MostrarCinematica(0);
    }
}
