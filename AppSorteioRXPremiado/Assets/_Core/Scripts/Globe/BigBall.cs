using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BigBall : MonoBehaviour
{
    public TextMeshProUGUI textNumber;

    public Image imageBall;
    public Sprite bgBall;
    public Sprite bgBallLogo;
    public Animator animBall;
    public string numberBall;
    public GameObject trackBall;

    public void SetInfoInBigBall(string _numberBall, bool isAnim = true)
    {
        if (isAnim)
        {
            animBall.SetTrigger("isShow");
        }
        imageBall.sprite = bgBall;
        numberBall = _numberBall;
        textNumber.text = _numberBall;
        imageBall.enabled = true;
        if (numberBall == "9" || numberBall == "6")
        {
            trackBall.SetActive(true);
        }
        else
        {
            trackBall.SetActive(false);
        }
    }

    public void SetBgBallWithLogo()
    {
        imageBall.sprite = bgBallLogo;
        textNumber.text = string.Empty;
        imageBall.enabled = true;
    }

    public void SetBallWinner()
    {
        imageBall.color = Color.yellow;
    }

}
