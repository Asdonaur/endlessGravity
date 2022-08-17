using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    Vector3 v3origin;
    GameObject parental;

    private void Awake()
    {
        v3origin = transform.position;
        parental = this.gameObject;
    }

    void Print(string valor)
    {
        Debug.Log(valor);
    }

    void PlaySound(AudioClip index)
    {
        GameManager.instance.PlaySE(index);
    }

    void SpawnGameObject(GameObject index)
    {
        GameObject product = Instantiate(index);
        product.transform.parent = parental.transform;
        product.transform.localPosition = new Vector3();
    }

    void Disappear()
    {
        Destroy(transform.parent.gameObject);
    }

    void GoToMenu()
    {
        GameManager.instance.Menu();
    }

    void CambiarParental(string parentalNew)
    {
        GameObject parentalobj = GameObject.Find(parentalNew);
        parental = parentalobj;
    }
}
