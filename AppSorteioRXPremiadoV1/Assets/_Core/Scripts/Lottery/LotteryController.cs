using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LotteryController : MonoBehaviour
{
    [Header("PANELS")]
    [SerializeField] private CanvasGroup panelLotteryNumber;
    [SerializeField] private CanvasGroup panelResult;
    [SerializeField] private CanvasGroup panelTicket;
    [SerializeField] private LotteryNumbers lotteryNumbers;
    [Space]
    [SerializeField] private UiLotteryController uiLotteryController;

    void Start()
    {
        InitializeVariables();
        //ShowLotteryNumberRaffle("000345");
    }

    void InitializeVariables()
    {
        panelLotteryNumber.gameObject.SetActive(true);
        panelResult.gameObject.SetActive(false);
        panelTicket.gameObject.SetActive(false);

        if (GameManager.instance.isLotteryOpenedScreen == false)
        {
            ResetLotteryNumbers();
            GameManager.instance.isLotteryOpenedScreen = true;
        }else
        {
            RecoveryLotteryNumbers();
        }
    }
    public void SetPopulateInfosLottery(string _editionNumber, string _competitionNumber, string _dateRaffle, string _dateCompetition)
    {   
        uiLotteryController.PopulateInfosLottery(_editionNumber, _competitionNumber, _dateRaffle, _dateCompetition);
    }
    private void ResetLotteryNumbers()
    {
        lotteryNumbers.lotteryScriptable.Reset();
    }

    private void RecoveryLotteryNumbers()
    {
        if (lotteryNumbers.lotteryScriptable.firstRaffle.Length > 0)
        {
            SetLotteryNumber(1, lotteryNumbers.lotteryScriptable.firstRaffle);
        }
        if (lotteryNumbers.lotteryScriptable.secondRaffle.Length > 0)
        {
            SetLotteryNumber(2, lotteryNumbers.lotteryScriptable.secondRaffle);
        }
        if (lotteryNumbers.lotteryScriptable.thirdRaffle.Length > 0)
        {
            SetLotteryNumber(3, lotteryNumbers.lotteryScriptable.thirdRaffle);
        }
        if (lotteryNumbers.lotteryScriptable.fourthRaffle.Length > 0)
        {
            SetLotteryNumber(4, lotteryNumbers.lotteryScriptable.fourthRaffle);
        }
        if (lotteryNumbers.lotteryScriptable.fifithRaffle.Length > 0)
        {
            SetLotteryNumber(5, lotteryNumbers.lotteryScriptable.fifithRaffle);
        }
    }
    public void SetLotteryNumber(int index, string _number)
    {
        switch (index)
        {
            case 01:
                {
                    lotteryNumbers.lotteryScriptable.firstRaffle = _number;
                    uiLotteryController.PopulateLotteryNumber(index, lotteryNumbers.lotteryScriptable.firstRaffle);
                    break;
                }
            case 02:
                {
                    lotteryNumbers.lotteryScriptable.secondRaffle = _number;
                    uiLotteryController.PopulateLotteryNumber(index, lotteryNumbers.lotteryScriptable.secondRaffle);
                    break;
                }
            case 03:
                {
                    lotteryNumbers.lotteryScriptable.thirdRaffle = _number;
                    uiLotteryController.PopulateLotteryNumber(index, lotteryNumbers.lotteryScriptable.thirdRaffle);
                    break;
                }
            case 04:
                {
                    lotteryNumbers.lotteryScriptable.fourthRaffle = _number;
                    uiLotteryController.PopulateLotteryNumber(index, lotteryNumbers.lotteryScriptable.fourthRaffle);
                    break;
                }
            case 05:
                {
                    lotteryNumbers.lotteryScriptable.fifithRaffle = _number;
                    uiLotteryController.PopulateLotteryNumber(index, lotteryNumbers.lotteryScriptable.fifithRaffle);
                    
                    break;
                }
        }
    }
    public void SetVisibilityLotteryRaffle()
    {

        Sequence seq = DOTween.Sequence();
        seq.Insert(0, panelResult.DOFade(0, 1f).OnComplete(() =>
         {
             panelResult.gameObject.SetActive(false);
             panelLotteryNumber.gameObject.SetActive(true);
             panelLotteryNumber.alpha = 0;
         }));
        seq.Insert(1, panelLotteryNumber.DOFade(1, 1f));
    }
    public void ShowLotteryNumberRaffle(string _lotteryNumberRaffle)
    {

        lotteryNumbers.lotteryScriptable.lotteryRaffleResult = _lotteryNumberRaffle;
        Sequence seq = DOTween.Sequence();
        seq.Append(panelLotteryNumber.DOFade(1, 0.01f));
        seq.Insert(0, panelLotteryNumber.DOFade(0, 1f).OnComplete(() =>
         {
             panelLotteryNumber.gameObject.SetActive(false);
             panelResult.gameObject.SetActive(true);
             StartCoroutine(ShowLotteryNumbers());
             panelResult.alpha = 0;
         }));
        seq.Insert(1, panelResult.DOFade(0, 0.01f));

        seq.Insert(2, panelResult.DOFade(1, 1f));
    }
    private IEnumerator ShowLotteryNumbers()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < lotteryNumbers.LotteryTextResults.Count; i++)
        {
            lotteryNumbers.LotteryTextResults[i].PopulateTxtNumber(lotteryNumbers.lotteryScriptable.lotteryRaffleResult[i].ToString());
        }
    }
    [System.Serializable]
    public class LotteryNumbers
    {
        public LotteryScriptable lotteryScriptable;

        public List<LotteryTextResult> LotteryTextResults = new List<LotteryTextResult>();

    }
}
