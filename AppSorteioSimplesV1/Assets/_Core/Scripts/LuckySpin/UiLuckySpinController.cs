using UnityEngine;
using TMPro;
public class UiLuckySpinController : MonoBehaviour
{

    public TextMeshProUGUI txtRoundRaffle;
    public TextMeshProUGUI txtLiquidPrize;


    public void SetRoundIDLuckySpin(int _roundID)
    {
        txtRoundRaffle.text = $"{_roundID}° SORTEIO";
    }

    public void PopulateSpinInfos(float _value, string _edition)
    {
        txtLiquidPrize.text = GameManager.instance.FormatMoneyInfo(_value, 2);
    }

   

}
