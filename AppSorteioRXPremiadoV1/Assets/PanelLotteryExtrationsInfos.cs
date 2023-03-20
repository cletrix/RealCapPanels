using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PanelLotteryExtrationsInfos : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEditionNumber;
    [SerializeField] private TextMeshProUGUI txtCompetitionNumber;
    [SerializeField] private TextMeshProUGUI txtDateRaffle;
    [SerializeField] private TextMeshProUGUI txtDateCompetition;
   
    public void PopulateInfosLottery(string _editionNumber, string _competitionNumber, string _dateRaffle, string _dateCompetition)
    {
        txtEditionNumber.text = _editionNumber;
        txtCompetitionNumber.text = _competitionNumber;
        txtDateRaffle.text = _dateRaffle;
        txtDateCompetition.text = _dateCompetition;
    }
  
}
