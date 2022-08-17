using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    public int inLevel = 1;
    string strNameLevel;
    GameObject selector;
    Image imgTaco, imgBread;
    Text txtLvl, txtTim;
    public Sprite[] sprites;
    int inValue;

    bool blGotData;

    bool Collected(int valP, int val1, int val2)
    {
        return ((valP == val1) || (valP == val2));
    }

    bool Selected()
    {
        return (Vector3.Distance(selector.GetComponent<RectTransform>().localPosition, gameObject.GetComponent<RectTransform>().localPosition) < 5);
    }

    bool BorrarDatos()
    {
        return ((Input.GetKey(KeyCode.LeftShift)) && (Input.GetKey(KeyCode.A)));
    }
    bool CompletarDatos()
    {
        return ((Input.GetKey(KeyCode.LeftShift)) && (Input.GetKey(KeyCode.Q)));
    }

    // Start is called before the first frame update
    void Start()
    {
        strNameLevel = "level-" + inLevel;
        selector = GameObject.Find("imgSelectorN");
        imgBread = GameObject.Find("imgBREAD").GetComponent<Image>();
        imgTaco = GameObject.Find("imgTACO").GetComponent<Image>();
        txtLvl = GetComponentInChildren<Text>();
        txtTim = GameObject.Find("TextTime").GetComponent<Text>();

        inValue = PlayerPrefs.GetInt(strNameLevel, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (inValue >= 1)
        {
            txtLvl.color = new Color(0.4f, 0.69f, 0.77f);
        }
        else
        {
            txtLvl.color = new Color(0.8f, 0.85f, 0.92f);
        }

        if (Selected())
        {
            GetData();
        }
        else
        {
            blGotData = false;
        }

        if (BorrarDatos())
        {
            PlayerPrefs.SetInt(strNameLevel, 0);
            PlayerPrefs.SetInt("stJump", 0);
            PlayerPrefs.SetInt("stDead", 0);
            PlayerPrefs.SetInt("tim" + inLevel, 0);
            txtLvl.color = new Color(0.8f, 0.85f, 0.92f);
            PlayerPrefs.SetInt("saw", 0);
        }

        if (CompletarDatos())
        {
            int q = (inLevel == 10) ? 0 : 4;
            PlayerPrefs.SetInt(strNameLevel, q);
            txtLvl.color = new Color(0.8f, 0.85f, 0.92f);
            PlayerPrefs.SetInt("saw", 0);
        }
    }

    void GetData()
    {
        if (!blGotData)
        {
            int inValue = PlayerPrefs.GetInt(strNameLevel, 0);
            if (Collected(inValue, 2, 4))
            {
                imgTaco.sprite = sprites[1]; 
            }
            else
            {
                imgTaco.sprite = sprites[0];
            }

            if (Collected(inValue, 3, 4))
            {
                imgBread.sprite = sprites[3];
            }
            else
            {
                imgBread.sprite = sprites[2];
            }

            string strTime = GameManager.instance.ConvertToTime(PlayerPrefs.GetInt("tim" + inLevel, 0));
            txtTim.text = strTime;

            blGotData = true;
        }
    }
}
