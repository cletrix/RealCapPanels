using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class LotteryTextResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtNumber;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image bgTamp;
    [SerializeField] private float delay=2f;

    void Start()
    {
        InitializeVariable();
    }
    void InitializeVariable()
    {   

        txtNumber = GetComponentInChildren<TextMeshProUGUI>();
        txtNumber.transform.DOLocalMoveZ(-500, 0.1f);
        bgImage = GetComponent<Image>();
    }

    public void ShowNumber()
    {
        bgTamp.transform.DOLocalMoveY(270, 1f).SetDelay(delay);
    }
    public void PopulateTxtNumber(string _number)
    {
        txtNumber.text = _number;
        ShowNumber();

       // txtNumber.transform.DOLocalMoveZ(0, 2f).SetDelay(1f);

    }
}
