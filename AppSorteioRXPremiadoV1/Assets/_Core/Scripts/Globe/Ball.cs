﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class Ball : MonoBehaviour
{
    private Transform ballTransform;
    public TextMeshProUGUI textNumber;
    public Image imageBall;

    public string numberBall;

    public Vector3 nextPos;
    private bool canMove = false;
    public GameObject trackBall;

    [SerializeField] private Color firstColor;
    [SerializeField] private Color secondColor;
    [SerializeField] private Image bgLineBall;

    private void Start()
    {
        ballTransform = GetComponent<Transform>();
    }
    public void SetInfoBall(string _numberBall)
    {
        numberBall = _numberBall;
        textNumber.text = _numberBall;
        imageBall.enabled = true;
        ChangeColorBgLine();
    
        SetMarkedNormal();
    }
    public void SetMarkedRevokedBall()
    {
        imageBall.color = Color.red;
        textNumber.color = Color.white;
    }
    public void SetMarkedNormal()
    {
        imageBall.color = Color.white;
    }

    private void ChangeColorBgLine()
    {
        int number = int.Parse(numberBall);
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

    public void HideBall()
    {
        Destroy(this.gameObject, 0.1f);
    }
    public void MoveBallAtFinalPos(Vector3 pos)
    {
        canMove = true;
        nextPos = pos;
        ballTransform.DOMove(nextPos, 1f);
    }
    public void ExitBall(Transform pos1, Transform pos2, Transform parent)
    {
        Sequence seq = DOTween.Sequence();
        seq.Insert(0, ballTransform.DOScale(1, 1f).OnComplete(() =>
         {
             ballTransform.SetParent(parent);
             RotateLoop(-360);

         }));
        seq.Insert(0, ballTransform.DOMove(pos1.position, 0.7f));
        seq.Insert(1, ballTransform.DOMove(pos2.position, 1f).SetDelay(0.2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        }));
    }
    public void SetSize(float size = 1.4f, float _delay = 0.5f)
    {
        //RotateLoop(360, 0.5f, delay: _delay, 1);
        ballTransform.DOScale(size, 0.5f).SetDelay(_delay);
    }
    public void RotateLoop(float rotZ, float rotSpeed = 0.5f, float delay = 0f, int loop = 2)
    {
        ballTransform.DORotate(new Vector3(0, 0, rotZ), rotSpeed).SetLoops(loop).SetRelative(true).SetEase(Ease.Linear).SetDelay(delay);
    }

    public void SetBallWinner()
    {
        imageBall.color = Color.yellow;
    }
}
