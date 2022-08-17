using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersBehaviour : MonoBehaviour
{
    #region VARIABLES

    //          OBJETOS Y COMPONENTES
    SpriteRenderer sprRen;
    GameObject obPlayer;

    //          NUMÉRICAS
    Vector3 size;

    //          BOOLEANAS
    bool blTP = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
    }

    void OnEnable()
    {
        if (GameManager.instance == null)
            return;

        sprRen = GetComponentInChildren<SpriteRenderer>();
        obPlayer = GameManager.instance.obPlayer;
        transform.position = new Vector3();
        sprRen.color = new Color(1, 1, 1, 0f);
        size = transform.localScale / 2;
    }

    // Update is called once per frame
    void Update()
    {
        float val0 = Mathf.Abs(obPlayer.transform.position.x),
            val1 = Mathf.Abs(obPlayer.transform.position.y),
            val2 = Mathf.Abs(size.x),
            val3 = Mathf.Abs(size.y);

        if (((val0 > val2) || (val1 > val3)) && (blTP == false))
        {
            StartCoroutine(Transport(obPlayer, (val0 > val2), (val1 > val3)));
        }
    }

    public IEnumerator Transport(GameObject who, bool b0, bool b2)
    {
        if (blTP == true)
        {
            StopCoroutine(Transport(who, b0, b2));
        }

        blTP = true;
        obPlayer.GetComponent<PlayerMovement>().blCanMove = false;
        obPlayer.GetComponent<PlayerMovement>().chrCntrl.enabled = false;
        //float tim = 0, timm = 0.05f;

        who.transform.position = new Vector3(who.transform.position.x * ((b0) ? -1 : 1), who.transform.position.y * ((b2) ? -1 : 1), 0);
        yield return new WaitForSecondsRealtime(0.05f);

        obPlayer.GetComponent<PlayerMovement>().blCanMove = true;
        obPlayer.GetComponent<PlayerMovement>().chrCntrl.enabled = true;
        yield return new WaitForSecondsRealtime(0.05f);

        blTP = false;
    }
}
