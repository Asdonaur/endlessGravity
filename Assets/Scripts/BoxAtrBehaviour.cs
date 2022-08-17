using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAtrBehaviour : MonoBehaviour
{
    //          OBJETOS Y COMPONENTES
    Animator animator;
    SpriteRenderer sprRen;
    GameObject obPlayer;
    new BoxCollider collider;

    //          NUMEROS
    float flDist = 1f;

    //          BOOLEANAS
    bool blSolid = false;

    public Sprite sprSolid;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
    }

    void OnEnable()
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
            Animar();
        }

        if (sprRen.sprite == sprSolid)
        {
            collider.isTrigger = false;
        }
    }

    public void Animar()
    {
        if (blSolid == false)
        {
            animator.Play("S");
            blSolid = true;
        }
    }
}
