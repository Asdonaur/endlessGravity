using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //          OBJETOS Y COMPONENTES
    public GameObject obTarget;
    GameObject obBorders;
    [Range(0, 5)]public float flLerp = 1;
    public float flRange = 1;

    public Vector3 v3Pos;

    // Start is called before the first frame update
    void Start()
    {
        obBorders = GameObject.FindGameObjectWithTag("Border");
        obTarget = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        int in1 = (obTarget.transform.position.x > 0) ? 1 : -1,
            in2 = (obTarget.transform.position.y > 0) ? 1 : -1;

        float val1 = obBorders.transform.localScale.x / 2,
            val2 = obBorders.transform.localScale.y / 2,
            val3 = (Mathf.Abs(obTarget.transform.position.x) + flRange > val1) ? (val1 - flRange) * in1 : obTarget.transform.position.x,
            val4 = (Mathf.Abs(obTarget.transform.position.y) + flRange > val2) ? (val2 - flRange) * in2 : obTarget.transform.position.y;

        v3Pos = new Vector3(val3, val4, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, v3Pos, flLerp);
        //Vector3.MoveTowards(transform.position, v3Pos, flLerp * (()));
    }
}
