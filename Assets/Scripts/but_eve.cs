using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class but_eve : MonoBehaviour
{
    public Color32 col;
    public Image img;
    public Image img2;
    public GameObject but;
    public Animator anima;

    private void Start()
    {
        but.SetActive(false);
    }

    public void go1()
    {
        img.color = col;
        img2.color = col;
        but.SetActive(true);
        anima.SetTrigger("go");
    }

    public void exit1()
    {
        img.color = Color.white;
        img2.color = Color.white;

        but.SetActive(false);
        anima.SetTrigger("stop");

    }
}
