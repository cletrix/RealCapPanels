using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class TotalBallRaffle : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textInfo;
    
    public void Start()
    {
        transform.DOLocalMoveY(-1000, 0.1f);
        ActiveMovement();
    }
    public void SetInfoTotalBall(string _info)
    {
        textInfo.text = _info;
    }

    public void ActiveMovement()
    {
        transform.DOLocalMoveY(-540, 1f).SetDelay(2f);
    }
}
