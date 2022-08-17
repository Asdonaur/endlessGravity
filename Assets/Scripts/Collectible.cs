using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum ColID
    {
        Key,
        Taco,
        Bread
    }
    public ColID kindOf;
    Animator animator;
    bool blIsActing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Accion()
    {
        if (blIsActing)
        {
            StopCoroutine(Accion());
        }
        blIsActing = true;

        switch (kindOf)
        {
            case ColID.Key:
                GameManager.instance.inKeys += 1;
                break;

            case ColID.Taco:
                GameManager.instance.blTaco = true;
                break;

            case ColID.Bread:
                GameManager.instance.blBread = true;
                break;
        }
        animator.Play("anCollD");
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
