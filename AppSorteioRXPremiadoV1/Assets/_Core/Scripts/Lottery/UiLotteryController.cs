using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UiLotteryController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEditionNumber;
    [SerializeField] private TextMeshProUGUI txtCompetitionNumber;
    [SerializeField] private TextMeshProUGUI txtDateRaffle;
    [SerializeField] private TextMeshProUGUI txtDateCompetition;
    [Space]
    [SerializeField] private LotteryTextNumbers lotteryTextNumbers;
    public void PopulateInfosLottery(string _editionNumber, string _competitionNumber, string _dateRaffle, string _dateCompetition)
    {
        txtEditionNumber.text = $"Edição: {_editionNumber}";
        txtCompetitionNumber.text = $"loteria federal - Resultado do concurso: {_competitionNumber}";
        txtDateRaffle.text = $"Data: {_dateRaffle}"; 
        txtDateCompetition.text = _dateCompetition; 
    }
    public void PopulateLotteryNumber(int index, string _number)
    {
        switch (index)
        {
            case 01:
                {
                    for (int i = 0; i < _number.Length; i++)
                    {
                        lotteryTextNumbers.firstLotteryNumber[i].PopulateTxtNumber(_number[i].ToString());
                    }
                    break;
                }
            case 02:
                {
                    for (int i = 0; i < _number.Length; i++)
                    {
                        lotteryTextNumbers.secondLotteryNumber[i].PopulateTxtNumber(_number[i].ToString());
                    }
                    break;
                }
            case 03:
                {
                    for (int i = 0; i < _number.Length; i++)
                    {

                        lotteryTextNumbers.thirdLotteryNumber[i].PopulateTxtNumber(_number[i].ToString());
                    }
                    break;
                }
            case 04:
                {
                    for (int i = 0; i < _number.Length; i++)
                    {

                        lotteryTextNumbers.fourthLotteryNumber[i].PopulateTxtNumber(_number[i].ToString());
                    }
                    break;
                }
            case 05:
                {
                    for (int i = 0; i < _number.Length; i++)
                    {

                        lotteryTextNumbers.fifithLotteryNumber[i].PopulateTxtNumber(_number[i].ToString());
                    }
                    break;
                }
        }

    }
    [System.Serializable]
    public class LotteryTextNumbers
    {
        public List<LotteryTextNumber> firstLotteryNumber = new List<LotteryTextNumber>();
        public List<LotteryTextNumber> secondLotteryNumber = new List<LotteryTextNumber>();
        public List<LotteryTextNumber> thirdLotteryNumber = new List<LotteryTextNumber>();
        public List<LotteryTextNumber> fourthLotteryNumber = new List<LotteryTextNumber>();
        public List<LotteryTextNumber> fifithLotteryNumber = new List<LotteryTextNumber>();
    }
}
