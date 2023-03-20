using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LotteryTextNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtNumber;
    [SerializeField] private Image bgImage;
    [SerializeField] private bool isLuckyNumber;

    [SerializeField] private Color luckyNumberColor;
    [SerializeField] private Color normalColor;
    void Start()
    {
        InitializeVariable();
    }
    void InitializeVariable()
    {
        normalColor = txtNumber.color;
        txtNumber = GetComponentInChildren<TextMeshProUGUI>();
        bgImage = GetComponent<Image>();
    }
    public void PopulateTxtNumber(string _number)
    {
        txtNumber.text = _number;
        //if (isLuckyNumber)
        //{
        //    txtNumber.color = luckyNumberColor;
        //}
        //else
        //{
        //    txtNumber.color = normalColor;
        //}
    }



}
