using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCinem : MonoBehaviour
{
    public int inIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        int inUnlock = PlayerPrefs.GetInt("saw", 0);
        if (inUnlock >= inIndex)
        {

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
