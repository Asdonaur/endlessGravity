using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIMenu : MonoBehaviour
{
    //          OBJETOS, COMPONENTES Y COSAS IMPORTANTES
    public GameObject[] opcInicio;
    public GameObject[] menus;
    public GameObject selector, obMenuSec, obStats;
    Animator animator;

    public int inMenu;

    //          SONIDOS
    public AudioClip seMove,
        seSelect,
        seReturn;

    //          ENTEROS
    public int inOpc = 0,
        inOpcMa;

    //          FLOTANTES
    [Range(0, 1)]public float flSpeedCursor = 0.1f,
        flSpeedActing = 0.1f;

    //          BOOLEANAS
    bool pressL,
        pressR,
        pressZ,
        pressX;

    bool blActing = false;
    bool blCanMove = false;
    bool blCanReact = true;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();

        CambiarMenu(0);
    }

    // Update is called once per frame
    void Update()
    {
        pressL = Input.GetButtonUp("L");
        pressR = Input.GetButtonUp("R");

        pressZ = Input.GetKeyDown("z");
        pressX = Input.GetKeyDown("x");

        if ((inOpcMa <= 0) || (animator.GetBool("credits")))
        {
            blCanMove = false;
        }
        else
        {
            blCanMove = true;
        }

        if (pressL)
        {
            CambiarOpcion(false);
        }
        if (pressR)
        {
            CambiarOpcion(true);
        }

        if (pressZ)
        {
            ButtonAction();
        }
        if (pressX)
        {
            ButtonAction(false);
        }

        selector.GetComponent<RectTransform>().position = Vector3.Lerp(selector.GetComponent<RectTransform>().position, opcInicio[inOpc].GetComponent<RectTransform>().position, flSpeedCursor);
        
    }

    void CambiarMenu(int men)
    {
        GameObject menu = menus[men];
        RectTransform[] rctTr = menu.GetComponentsInChildren<RectTransform>();
        List<GameObject> lista = new List<GameObject>();

        foreach (RectTransform rect in rctTr)
        {
            switch (rect.gameObject.tag)
            {
                case "MenuSelector":
                    selector = rect.gameObject;
                    break;

                case "MenuOption":
                    lista.Add(rect.gameObject);
                    break;
            }
        }
        opcInicio = lista.ToArray();
        inOpc = 0;
        inOpcMa = opcInicio.Length - 1;
        inMenu = men;

        if (men == 1)
        {
            MenuSecundarioQuitar();
        }
        if (men > 1)
        {
            MenuSecundario(men);
        }
        PlayerPrefs.SetInt("menuIndex", men);
        Debug.Log(PlayerPrefs.GetInt("menuIndex", 0));
    }

    void CambiarOpcion(bool sumar)
    {
        if (blCanMove)
        {
            StartCoroutine(ienMoveOption(sumar));
            GameManager.instance.PlaySE(seMove);
        }
        
    }

    void ButtonAction(bool avance = true)
    {
        if (blCanReact)
        {
            if (avance)
            {
                GameManager.instance.PlaySE(seSelect);

                switch (inMenu)
                {
                    case 0: // PRESIONAR Z PARA EMPEZAR
                        CambiarMenu(1);
                        animator.SetInteger("menua", 1);
                        blCanMove = true;
                        break;

                    case 1: // MENU INICIO
                        switch (inOpc)
                        {
                            case 0:
                                CambiarMenu(2);
                                MenuSecundario(2);
                                break;

                            case 1:
                                CambiarMenu(3);
                                MenuSecundario(3);
                                break;

                            case 2:
                                Application.Quit();
                                break;

                        }
                        break;

                    case 2: // SELECCIONAR NIVELES
                        int lvl = inOpc + 1;
                        string lvlSTR = "level-" + lvl;
                        SceneManager.LoadScene(lvlSTR);
                        break;

                    case 3: //  CONFIG
                        switch (inOpc)
                        {
                            case 0:
                                VerCreditos(!animator.GetBool("credits"));
                                break;

                            case 1:
                                CambiarMenu(4);
                                break;

                            case 2:
                                VerStats(!animator.GetBool("stats"));
                                break;

                        }
                        break;

                    case 4: //  CINEMATICAS
                        GameManager.instance.MostrarCinematica(inOpc);
                        break;
                }
            }
            else
            {
                if ((inMenu != 0) && (inMenu != 1))
                {
                    GameManager.instance.PlaySE(seReturn);
                }

                switch (inMenu)
                {
                    case 2:
                        CambiarMenu(1);
                        break;
                    case 3:
                        if ((animator.GetBool("credits")) || (animator.GetBool("stats")))
                        {
                            VerCreditos(false);
                            VerStats(false);
                        }
                        else
                        {
                            CambiarMenu(1);
                        }

                        break;

                    case 4: //  CINEMATICAS
                        CambiarMenu(3);
                        menus[4].GetComponent<RectTransform>().position = new Vector3(0, -500, 0);
                        break;
                }
            }
        }
        else
        {
            // N A D A
        }
    }

    void MenuSecundario(int menu)
    {
        MenuSecundarioQuitar(false);
        RectTransform rect = menus[menu].GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        animator.SetInteger("menua", 2);
    }

    public void MenuSecundarioQuitar(bool r = true)
    {
        RectTransform[] rects = obMenuSec.GetComponentsInChildren<RectTransform>();
        foreach (var item in rects)
        {
            if (item.gameObject.tag == "MenuMenu")
            {
                item.localPosition = new Vector3(0, -2000, 0);
            }
        }

        if (r)
        {
            animator.SetInteger("menua", 1);
        }
    }

    void VerCreditos(bool poner)
    {
        animator.SetBool("credits", poner);
        blCanMove = !poner;
    }

    void VerStats(bool poner)
    {
        if (poner)
        {
            Text[] textos = obStats.GetComponentsInChildren<Text>();
            foreach (Text item in textos)
            {
                switch (item.gameObject.transform.parent.gameObject.name)
                {
                    case "stat1":
                        item.text = PlayerPrefs.GetInt("stJump", 0) + "";
                        break;

                    case "stat2":
                        item.text = PlayerPrefs.GetInt("stDead", 0) + "";
                        break;

                    case "stat3":
                        item.text = GetBoolValue(true) + " / 10";
                        break;

                    case "stat4":
                        item.text = GetBoolValue(false) + " / 10";
                        break;

                    case "stat5":
                        int[] variables =
                        {
                            PlayerPrefs.GetInt("tim1", 0),
                            PlayerPrefs.GetInt("tim2", 0),
                            PlayerPrefs.GetInt("tim3", 0),
                            PlayerPrefs.GetInt("tim4", 0),
                            PlayerPrefs.GetInt("tim5", 0),
                            PlayerPrefs.GetInt("tim6", 0),
                            PlayerPrefs.GetInt("tim7", 0),
                            PlayerPrefs.GetInt("tim8", 0),
                            PlayerPrefs.GetInt("tim9", 0),
                            PlayerPrefs.GetInt("tim10", 0)
                        };

                        int valor = 0;

                        foreach (var itema in variables)
                        {
                            valor += itema;
                        }

                        item.text = GameManager.instance.ConvertToTime(valor);
                        break;
                }
            }
        }

        animator.SetBool("stats", poner);
        blCanMove = !poner;
    }

    int GetBoolValue(bool isTaco)
    {
        int[] variables =
        {
            PlayerPrefs.GetInt("level-1", 0),
            PlayerPrefs.GetInt("level-2", 0),
            PlayerPrefs.GetInt("level-3", 0),
            PlayerPrefs.GetInt("level-4", 0),
            PlayerPrefs.GetInt("level-5", 0),
            PlayerPrefs.GetInt("level-6", 0),
            PlayerPrefs.GetInt("level-7", 0),
            PlayerPrefs.GetInt("level-8", 0),
            PlayerPrefs.GetInt("level-9", 0),
            PlayerPrefs.GetInt("level-10", 0)
        };

        int valor = 0;
        int valor2 = (isTaco) ? 2 : 3;

        foreach (var item in variables)
        {
            valor += ((item == valor2) || (item == 4)) ? 1 : 0;
        }

        return valor;
    }

    IEnumerator ienMoveOption(bool suma)
    {
        if (blActing)
        {
            StopCoroutine(ienMoveOption(suma));
        }
        blActing = true;

        inOpc += (suma) ? 1 : -1;
        inOpc += (inOpc < 0) ? (inOpcMa + 1) : 0;
        inOpc -= (inOpc > inOpcMa) ? (inOpcMa + 1) : 0;

        while (Vector3.Distance(selector.GetComponent<RectTransform>().position, opcInicio[inOpc].GetComponent<RectTransform>().position) > flSpeedActing)
        {
            yield return new WaitForSecondsRealtime(0.05f);
        }
        blActing = false;
    }
}
