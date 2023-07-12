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

    public void PopulateSpinInfos(float _value, string _description , string _edition)
    {
        txtLiquidPrize.text = _description;   //GameManager.instance.FormatMoneyInfo(_value, 2);
       
    }

    private void FixedUpdate()
    {
        DateTime theTime = DateTime.Now;
        txtDateRaffle.text = theTime.ToString(("dd/MM/yyyy - HH:mm:ss"));
        txtLiquidPrize.text = GameManager.instance.luckySpinScriptable.prizeDescription;
        txtEdition.text = $"EDIÇÃO N° {GameManager.instance.luckySpinScriptable.editionID}";
    }

}
