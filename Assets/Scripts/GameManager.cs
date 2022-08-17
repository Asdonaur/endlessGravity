using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region VARIABLES

    //          PRINCIPALES
    public enum kindCnt
    {
        Level,
        Menu
    }
    public kindCnt sceneType;

    //          OBJETOS Y COMPONENTES
    public GameObject obPlayer,
        obGoal;
    public static GameManager instance;
    AudioSource audSrc;

    //          NUMERICAS
    public int inKeys = 0;
    public int inSegundos = 0;

    //          BOOLEANAS
    [HideInInspector] public bool blTaco = false,
        blBread = false;
    bool blResultado = false;

    //          VARIABLES FUNCION
    public bool Passed(bool withFood)
    {
        int val1 = PlayerPrefs.GetInt("level-1", 0),
            val2 = PlayerPrefs.GetInt("level-2", 0),
            val3 = PlayerPrefs.GetInt("level-3", 0),
            val4 = PlayerPrefs.GetInt("level-4", 0),
            val5 = PlayerPrefs.GetInt("level-5", 0),
            val6 = PlayerPrefs.GetInt("level-6", 0),
            val7 = PlayerPrefs.GetInt("level-7", 0),
            val8 = PlayerPrefs.GetInt("level-8", 0),
            val9 = PlayerPrefs.GetInt("level-9", 0),
            val10 = PlayerPrefs.GetInt("level-10", 0);

        int a = (withFood) ? 4 : 1;
        return (val1 >= a) && (val2 >= a) && (val3 >= a) && (val4 >= a) && (val5 >= a) && (val6 >= a) && (val7 >= a) && (val8 >= a) && (val9 >= a) && (val10 >= a);
    }
    public string ConvertToTime(int index)
    {
        int referencia = index,
            minutos = 0,
            segundos = 0;

        while (referencia > 59)
        {
            referencia -= 60;
            minutos += 1;
        }
        segundos = referencia;

        string str1 = ((minutos > 9) ? "" : "0") + minutos;
        string str2 = ((segundos > 9) ? "" : "0") + segundos;
        return (str1 + ":" + str2);
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audSrc = GetComponent<AudioSource>();


        if (sceneType == kindCnt.Level)
        {
            StartCoroutine(ienCountTime());
            obPlayer = GameObject.FindGameObjectWithTag("Player");
            obGoal = GameObject.FindGameObjectWithTag("Finish");
            Coleccionables();

            StartCoroutine(ienObPlayer());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("menuIndex", 0);
        PlayerPrefs.SetInt("cinemIndex", 0);
    }

    public void PlaySE(AudioClip ind, float p = 1)
    {
        audSrc.pitch = p;
        audSrc.PlayOneShot(ind);
    }

    public void vLose()
    {
        blResultado = true;
        
        StartCoroutine(ienLose());
        
    }

    public void vWin()
    {
        blResultado = true;
        obGoal.GetComponent<GoalBehaviour>().AnimGanar();
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel()
    {
        string nivelActual = SceneManager.GetActiveScene().name;
        int numero1 = int.Parse(nivelActual.Substring(6, nivelActual.Length - 6)),
            numero2 = numero1 + 1;
        string nextLevel = "level-" + numero2;
        SceneManager.LoadScene(nextLevel);
    }

    public void GuardarTiempo()
    {
        string nivelActual = SceneManager.GetActiveScene().name;
        int a = int.Parse(nivelActual.Substring(6, nivelActual.Length - 6));
        string nomVar = "tim" + a;

        if ((inSegundos < PlayerPrefs.GetInt(nomVar, 0)) || (PlayerPrefs.GetInt(nomVar, 0) == 0))
        {
            PlayerPrefs.SetInt(nomVar, inSegundos);
        }
        Debug.Log(a + " - " + PlayerPrefs.GetInt(nomVar, 0));
    }

    public void Coleccionables()
    {
        // VERIFICAR LAS VARIABLES
        string nombre = SceneManager.GetActiveScene().name;
        int numero = PlayerPrefs.GetInt(nombre, 0);

        bool verificar(int v1, int v2)
        {
            return (numero == v1) || (numero == v2);
        }


        // BUSCAR LOS OBJECTOS Y ASIGNARLES EL ALPHA
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Collect");
        foreach (var item in objects)
        {
            switch (item.GetComponent<Collectible>().kindOf)
            {
                case Collectible.ColID.Key:
                    break;

                case Collectible.ColID.Taco:
                    if (verificar(2, 4))
                    {
                        Color colorcin = item.GetComponentInChildren<SpriteRenderer>().color;
                        item.GetComponentInChildren<SpriteRenderer>().color = new Color(colorcin.r, colorcin.g, colorcin.b, 0.5f);
                        blTaco = true;
                    }
                    break;

                case Collectible.ColID.Bread:
                    if (verificar(3, 4))
                    {
                        Color colorcin = item.GetComponentInChildren<SpriteRenderer>().color;
                        item.GetComponentInChildren<SpriteRenderer>().color = new Color(colorcin.r, colorcin.g, colorcin.b, 0.5f);
                        blBread = true;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public IEnumerator ienObPlayer()
    {
        do
        {
            yield return null;

        } while (obPlayer == null);

        GameObject[] objetos = GameObject.FindGameObjectsWithTag("Border");
        foreach (GameObject obj in objetos)
        {
            obj.GetComponent<BordersBehaviour>().enabled = true;
        }

        GameObject[] objetos2 = GameObject.FindGameObjectsWithTag("BoxBre");
        foreach (GameObject obj in objetos2)
        {
            obj.GetComponent<BoxBreBehaviour>().enabled = true;
        }

        GameObject[] objetos1 = GameObject.FindGameObjectsWithTag("BoxAtr");
        foreach (GameObject obj in objetos1)
        {
            obj.GetComponent<BoxAtrBehaviour>().enabled = true;
        }

        GameObject[] objetos3 = GameObject.FindGameObjectsWithTag("Candado");
        foreach (GameObject obj in objetos3)
        {
            obj.GetComponent<BoxBreBehaviour>().enabled = true;
        }

    }

    public void MostrarCinematica(int cual)
    {
        PlayerPrefs.SetInt("cinemIndex", cual);
        SceneManager.LoadScene("Cutscenes");
    }

    public IEnumerator ienLose()
    {
        obPlayer.GetComponent<PlayerMovement>().Die();
        
        yield return new WaitForSeconds(3);
        ReloadScene();
    }

    IEnumerator ienCountTime()
    {
        float tim = 0f;
        while (blResultado == false)
        {
            yield return new WaitForSeconds(0.1f);
            tim += 0.1f;

            if (tim >= 1)
            {
                inSegundos += 1;
                tim = 0;
            }
            
        }
    }
}
