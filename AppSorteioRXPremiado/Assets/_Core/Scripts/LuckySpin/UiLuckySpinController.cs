using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class UiLuckySpinController : MonoBehaviour
{

    public TextMeshProUGUI txtRoundRaffle;
    public TextMeshProUGUI txtLiquidPrize;
    public TextMeshProUGUI txtDateRaffle;
    public TextMeshProUGUI txtEdition;


    public void SetRoundIDLuckySpin(int _roundID)
    {
        txtRoundRaffle.text = $"{_roundID}° SORTEIO";
    }

    public void PopulateSpinInfos(string _value, string _edition)
    {
        txtLiquidPrize.text = GameManager.instance.FormatMoneyInfo(int.Parse(_value), 2);
        txtEdition.text = $"EDIÇÃO N° {_edition}";
    }

    private void FixedUpdate()
    {
        DateTime theTime = DateTime.Now;
        txtDateRaffle.text = theTime.ToString(("dd/MM/yyyy - HH:mm:ss"));
    }

}
