using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region VARIABLES

    //          OBJETOS Y COMPONENTES
    public CharacterController chrCntrl;
    Animator animator;
    SpriteRenderer sprRen;

    //          SONIDOS
    public AudioClip seJump,
        seFlip;

    //          DETECTAR BOTONES;
    bool pressUp;
    bool pressDown;
    bool pressL;
    bool pressR;

    //          FISICAS
    [HideInInspector] public int inState;
    public float flSpeed = 1;
    public float flJumpForce = 1;
    public float flGravity = 1;
    Vector3 v3MovePlayer;
    float flFallVelocity;
    int inGravityDir = 1;

    //          BOOLEANAS
    [HideInInspector] public bool blCanMove = true;
    bool piso = false;
    bool blDead = false;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //          ASIGNAR VARIABLES
        chrCntrl = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        sprRen = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //          VARIABLES
        inState = animator.GetInteger("state");
        pressUp = Input.GetButton("UP");
        pressDown = Input.GetButton("DN");
        pressL = Input.GetButton("L");
        pressR = Input.GetButton("R");

        v3MovePlayer = new Vector3(flSpeed, 0f, 0f);
        SetGravity();

        MaquinaDeEstados();

        //          RAYCAST
        RaycastHit hit;
        int layerMask = 1;
        float flFactor = 1.65f;
        Vector3 v3Dir = new Vector3(((flSpeed < 0) ? -1 : 1), 0, 0);

        // Does the ray intersect any objects excluding the player layer
        if ((Physics.Raycast(transform.position, v3Dir, out hit, chrCntrl.radius * flFactor, layerMask)) && (!hit.collider.isTrigger))
        {
            VerifyCollider(hit.collider.gameObject);
        }
    }

    private void LateUpdate()
    {
        //          CONTROL DEL PERSONAJE CORRIENDO
        v3MovePlayer.z = 0;

        switch (animator.GetInteger("state"))
        {
            case 0:
            case 1:
            case 2:
                chrCntrl.Move(v3MovePlayer * Time.deltaTime);
                break;

            default:
                break;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit == null)
        {
            return;
        }

        VerifyCollider(hit.gameObject);
    }

    void VerifyCollider(GameObject hiter)
    {
        switch(hiter.tag)
        {
            case "Player":
            case "Ledge":
                break;

            case "Collect":
                hiter.GetComponent<Collider>().isTrigger = true;
                StartCoroutine(hiter.GetComponent<Collider>().gameObject.GetComponent<Collectible>().Accion());
                break;

            case "BoxHurt":
                GameManager.instance.vLose();
                break;

            case "Gravitor":
                StartCoroutine(ienTrigger(hiter.gameObject));
                //DirGravedad();
                break;

            case "Finish":
                GameManager.instance.vWin();
                break;

            default:
                float val1 = hiter.transform.position.y,    // EL VAL3 SERA EL QUE TIENE QUE VALER MAS PARA QUE EL VOLTEO SE EJECUTE
                    val2 = transform.position.y,
                    val3 = ((inGravityDir == 1) ? val1 : val2),
                    val4 = ((inGravityDir == 1) ? val2 : val1);
                float rang = 0.65f;

                bool TestRange(float numberToCheck, float bottom, float top)
                {
                    return (numberToCheck >= bottom && numberToCheck <= top);
                }

                if ((TestRange(val4, val3 - rang, val3 + rang)) && (!hiter.GetComponent<Collider>().isTrigger))
                {
                    Flip();
                }
                
                break;
        }
    }

    void Estado(int ani)
    {
        if (animator.GetInteger("state") != ani)
        {
            animator.SetInteger("state", ani);
        }
    }

    void MaquinaDeEstados()
    {
        if (blCanMove)
        {
            chrCntrl.enabled = true;
            switch (animator.GetInteger("state"))
            {
                case 0: //  CORRER
                    if ((pressUp) && (piso))
                    {
                        Saltar();
                    }

                    if (pressDown)
                    {
                        Estado(1);
                    }
                    break;

                case 1: //  DESLIZAR
                    if (!pressDown)
                    {
                        Estado(0);
                    }
                    break;

                case 2: //  AIRE
                    break;
            }
        }
        else
        {
            chrCntrl.enabled = false;
        }
    }

    void SetGravity()
    {
        //          PRIMERO VERIFICAR SI ESTÁ EN EL PISO O NO
        if (inGravityDir == 1)
        {
            piso = chrCntrl.isGrounded;
        }
        else
        {
            //          RAYCAST
            RaycastHit hit;
            int layerMask = 1;
            float flFactor = 2.4f;

            if ((Physics.Raycast(transform.position, Vector3.up, out hit, chrCntrl.radius * flFactor, layerMask)) && (!hit.collider.isTrigger))
            {
                Debug.DrawRay(transform.position, Vector3.up * hit.distance, Color.yellow);
                piso = true;
            }
            else
            {
                Debug.DrawRay(transform.position, Vector3.up * (chrCntrl.radius * flFactor), Color.white);
                piso = false;
            }
        }

        //          DESPUES CONTROLAR QUE HACER SI ESTA EN EL PISO O NO
        if (piso)
        {
            Estado((pressDown) ? 1 : 0);
            flFallVelocity = -(flGravity * inGravityDir) * Time.deltaTime;
            v3MovePlayer.y = flFallVelocity;
        }
        else
        {
            flFallVelocity -= ((flGravity * inGravityDir) * ((pressDown) ? 5f : 1f)) * Time.deltaTime;
            v3MovePlayer.y = flFallVelocity;
            Estado(2);
        }

        RaycastHit hit2;
        int layerMask2 = 1;
        float flFactor2 = 2.4f;
        if ((Physics.Raycast(transform.position, Vector3.down * inGravityDir, out hit2, chrCntrl.radius * flFactor2, layerMask2)) && (!hit2.collider.isTrigger))
        {
            VerifyCollider(hit2.collider.gameObject);
        }

        RaycastHit hit3;
        float flFactor3 = 2.4f;
        if ((Physics.Raycast(transform.position, Vector3.up * inGravityDir, out hit3, chrCntrl.radius * flFactor3, layerMask2)) && (!hit3.collider.isTrigger))
        {
            Debug.DrawLine(transform.position, hit3.transform.position, Color.red);
            VerifyCollider(hit3.collider.gameObject);
            flFallVelocity = (Mathf.Abs(flFallVelocity) > 1) ? (-0.1f * inGravityDir) : flFallVelocity;
        }
    }

    void DirGravedad()
    {
        Saltar(2.75f);
        sprRen.flipY = !sprRen.flipY;
        inGravityDir = -inGravityDir;
    }

    void Saltar(float force = 1)
    {
        GameManager.instance.PlaySE(seJump, (force * 0.5f) + 0.5f);
        flFallVelocity = ((flJumpForce) * force) * inGravityDir;
        v3MovePlayer.y = flFallVelocity;
        if (force == 1)
        {
            PlayerPrefs.SetInt("stJump", PlayerPrefs.GetInt("stJump", 0) + 1);
        }
    }

    void Flip() //  TRUE = IZQUIERDA
    {
        sprRen.flipX = !sprRen.flipX;
        flSpeed = -flSpeed;
        chrCntrl.Move(new Vector3(((flSpeed < 0) ? -0.1f : 0.1f), 0, 0));
        
    }

    public void Die()
    {
        if (blDead == false)
        {
            blDead = true;
            blCanMove = false;
            Estado(3);
            PlayerPrefs.SetInt("stDead", PlayerPrefs.GetInt("stDead", 0) + 1);
            animator.Play("Lose");
        }
    }

    IEnumerator ienTrigger(GameObject ind)
    {
        Collider collider = ind.GetComponent<Collider>();
        collider.isTrigger = true;
        yield return new WaitForSeconds(0.01f);
        DirGravedad();
        yield return new WaitForSeconds(0.3f);
        collider.isTrigger = false;
    }
}
