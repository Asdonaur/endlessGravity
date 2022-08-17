using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBreBehaviour : MonoBehaviour
{
    //          OBJETOS Y COMPONENTES
    public GameObject breaking;
    Animator animator;
    SpriteRenderer sprRen;
    GameObject obPlayer;
    new BoxCollider collider;

    //          NUMEROS
    float flDist = 1f;

    //          BOOLEANAS
    public bool needsKey = false;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
    }

    private void OnEnable()
    {
        if (GameManager.instance == null)
            return;

        animator = GetComponentInChildren<Animator>();
        sprRen = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<BoxCollider>();
        obPlayer = GameManager.instance.obPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(obPlayer.transform.position, transform.position) <= flDist)
        {
            switch (needsKey)
            {
                case true:
                    if (GameManager.instance.inKeys >= 1)
                    {
                        Destruir(true);
                    }
                    
                    break;

                case false:
                    if ((obPlayer.transform.position.y <= transform.position.y) && (obPlayer.GetComponent<PlayerMovement>().inState == 1))
                    {
                        Destruir(false);
                    }
                    break;
            }
        }
    }

    public void Destruir(bool llave)
    {
        if (llave)
        {
            GameManager.instance.inKeys -= 1;
        }
        Instantiate(breaking, transform.position, new Quaternion());
        Destroy(this.gameObject);
    }
}
