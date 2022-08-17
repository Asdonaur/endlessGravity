using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalBehaviour : MonoBehaviour
{
    Animator animator;
    bool blWon = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void AnimGanar()
    {
        blWon = true;
        GameObject jugador = GameManager.instance.obPlayer;
        jugador.GetComponent<PlayerMovement>().enabled = false;

        CharacterController chrCtr = jugador.GetComponent<CharacterController>();
        chrCtr.stepOffset = 0.01f;
        chrCtr.enabled = false;
        jugador.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        animator.Play("anGoalW");
    }

    void OnDestroy()
    {
        if (blWon)
        {
            GuardarProgresos();
            GameManager.instance.GuardarTiempo();

            int inSaw = PlayerPrefs.GetInt("saw", 0);
            bool blSaw1 = (inSaw >= 1) ? true : false,
                blSaw2 = (inSaw == 2) ? true : false;

            if (GameManager.instance.Passed(false)) // Si paso f
            {
                if (GameManager.instance.Passed(true)) // Si paso t
                {
                    if (blSaw2) //Si vio 2
                    {
                        Debug.Log("t 2");
                        AccionesPorDefecto();
                        
                    }
                    else //Si no vio 2
                    {
                        Debug.Log("t n2");
                        showCinem(2);
                        
                    }
                }
                else // Solamente paso f
                {
                    if (blSaw1) //Si vio 1
                    {
                        Debug.Log("f 1");
                        AccionesPorDefecto();
                        
                    }
                    else //Si no vio 1
                    {
                        Debug.Log("f n1");
                        showCinem(1);
                        
                    }
                }
            }
            else // No pasaste nada weon
            {
                Debug.Log("-----");
                AccionesPorDefecto();
            }
            

        }
    }

    void GuardarProgresos()
    {
        /*
         0 = NO PASO
         1 = PASO SIN COLECCIONABLES
         2 = PASO CON TACO
         3 = PASO CON BAGUETE
         4 = PASO CON TACO Y BAGUETE
         */
        int num0 = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name, 0),
            num1 = ((GameManager.instance.blTaco) ? 1 : 0),
            num2 = ((GameManager.instance.blBread) ? 2 : 0);

        int numero = 1 + (((num0 == 2) || (num0 == 4)) ? 1 : num1) + (((num0 == 3) || (num0 == 4)) ? 2 : num2);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, numero);
    }

    void AccionesPorDefecto()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "level-10":
                GameManager.instance.Menu();

                break;

            default:
                GameManager.instance.NextLevel();
                break;
        }
    }

    void showCinem(int e)
    {
        PlayerPrefs.SetInt("saw", e);
        GameManager.instance.MostrarCinematica(e);
    }
}
