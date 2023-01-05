using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class BallSlot : MonoBehaviour
{
    public Color selectedColor;
    public Color noSelectedColor;
    public Color finishedColor;
    public Color winnerColor;
    public TextMeshProUGUI textNumber;
    public int numberBall;
    public Image imageBall;
    public GameObject ballBorder;
    public Animator animBorder;
    public bool isActive = false;
    public bool isFinishRaffle = false;
    public bool hasRaffled = false;
    public Transform ballTransform;

    public Image bgLineBall;
    public Color firstColor;
    public Color secondColor;

    void Start()
    {
        imageBall.color = noSelectedColor;
        ballTransform = GetComponent<Transform>();
    }

    public void SetNumberBall(int _number)
    {
        numberBall = _number;
        textNumber.text = _number.ToString();

        ballBorder.SetActive(false);
        SetEnableBall();
        ChangeColorBgLine();
    }
    public void SetSelectedBall()
    {
        ballBorder.SetActive(true);
        imageBall.enabled = true;
        imageBall.color = selectedColor;

    }
    public void DisableBall()
    {
        hasRaffled = false;
        isFinishRaffle = false;
        isActive = false;
        SetNormalColor();
        ballBorder.SetActive(isActive);
    }

    public void SetEnableBallBorder()
    {
        ballTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
        SetEnableBall();
        isActive = true;
        if (UIManager.instance.panelScriptable.winnersCount > 0)
        {
            imageBall.color = winnerColor;
        }
        else
        {
            imageBall.color = selectedColor;
        }
        Invoke("ActiveBlinkAnimBorder", 0.1f);
    }
    private void ActiveBlinkAnimBorder()
    {
        ballBorder.SetActive(isActive);
        animBorder.SetBool("IsBlink", true);
        Invoke("DesactiveBlinkAnimBorder", 3f);
    }
    private void DesactiveBlinkAnimBorder()
    {
        CancelInvoke("ActiveBlinkAnimBorder");
        animBorder.SetBool("IsBlink", false);
        CancelInvoke("DesactiveBlinkAnimBorder");
        ballTransform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
    }

    private void ChangeColorBgLine()
    {
        int number = numberBall;
        if (number <= 30)
        {
            bgLineBall.color = firstColor;
            textNumber.color = firstColor;
        }
        else
        {
            bgLineBall.color = secondColor;
            textNumber.color = secondColor;
        }
    }

    public void SetEnableBall()
    {
        imageBall.enabled = true;

    }
    public void SetNormalColor()
    {
        imageBall.enabled = true;
        imageBall.color = noSelectedColor;
    }
    public void SetSelectedColor()
    {
        imageBall.enabled = true;
        imageBall.color = selectedColor;
    }

    public void SetFinishedColor()
    {
        imageBall.enabled = true;
        imageBall.color = finishedColor;
    }



}
