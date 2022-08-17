using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUILevel : MonoBehaviour
{
    [Header("HUD NIVEL")]

    public Text txtKey;
    public Text txtTime;
    public Image imgTaco;
    public Image imgBread;
    public Sprite sprTacoGet,
        sprBreadGet;

    [Header("HUD PAUSA")]
    public GameObject obPausa;
    public AudioClip sePause;

    //          OTROS
    bool blPaused = false;

    //          VARIABLES FUNCION
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //      IMAGENES EN LA HUD
        if (GameManager.instance.blTaco)
        {
            imgTaco.sprite = sprTacoGet;
        }
        if (GameManager.instance.blBread)
        {
            imgBread.sprite = sprBreadGet;
        }
        txtKey.text = GameManager.instance.inKeys + "";
        txtTime.text = GameManager.instance.ConvertToTime(GameManager.instance.inSegundos);

        //      CONTROL DE LA PAUSA
        if (Input.GetKeyDown("return"))
        {
            Pausar();
        }

        if ((blPaused) && (Input.GetKey(KeyCode.X)))
        {
            Time.timeScale = 1;
            GameManager.instance.Menu();
        }
    }

    void Pausar(int a = 0)
    {
        if (a == 0)
        {
            blPaused = !blPaused;
            Time.timeScale = (blPaused) ? 0f : 1f;
            obPausa.SetActive(blPaused);
        }
        else
        {
            blPaused = (a == 1) ? true : false;
            Time.timeScale = (a == 1) ? 0f : 1f;
            obPausa.SetActive((a == 1) ? true : false);
        }
        GameManager.instance.PlaySE(sePause, 0.75f);
    }
}
