using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centering : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0f);
    }
}
