using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeController : MonoBehaviour
{
    public Image img;
    public Animator animator;
    public bool isActive;
    void Start()
    {
        if (isActive)
            SetStateFadeIn();
    }

    private void DesactiveFadeIMG()
    {
        img.enabled = false;
    }
    public void SetStateFadeIn()
    {
        img.enabled = true;
        animator.SetTrigger("FadeIN");
        img.color = new Color(1, 1, 1, 1);
        Invoke("DesactiveFadeIMG", 0.6f);
    }
    public void SetStateFadeOUT()
    {
        img.enabled = true;
        img.color = new Color(0, 0, 0, 0);
        animator.SetTrigger("FadeOUT");
        //Invoke("DesactiveFadeIMG", 1f);
    }
}
