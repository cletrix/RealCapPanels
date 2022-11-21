using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RiptideNetworking;

public class GlobeController : MonoBehaviour
{

    [Header("Infos Screen")]
    [SerializeField] private TotalBallRaffle totalBallsCount;
    [SerializeField] private PossiblesWinners lastBallRaffle;

    [Header("Variables")]

    [SerializeField] private BallController ballController;
    [SerializeField] private WinnersScreen winnersScreen;

    [Space]
    public bool isLoop = false;
    public bool isWinner = false;
    public bool hasShowHeart = true;
    [Space]
    public string numberSTR;
    public float timeToSpawn = 3f;
    public bool canRafleeBall = true;

    void Start()
    {
        InitializeVariables();
    }
    private void InitializeVariables()
    {
        isWinner = false;
        winnersScreen = FindObjectOfType<WinnersScreen>();
        ballController.GetLastIndexBallsRaffle(GameManager.instance.globeScriptable.numberBalls.Count, GameManager.instance.globeScriptable.numberBalls);
        totalBallsCount.SetInfoTotalBall((GameManager.instance.globeScriptable.ballRaffledCount).ToString());
        lastBallRaffle.SetTicketForOneBallInfo(GameManager.instance.globeScriptable.possiblesWinnersCount.ToString());
        if (GameManager.instance.globeScriptable.possiblesWinnersCount > 0)
        {
            lastBallRaffle.PlayAnimationHeart(true);
        }
        else
        {
            lastBallRaffle.PlayAnimationHeart(false);
        }
    }
    private void OnEnable()
    {
        BallController.OnBallRaffled += VerifyWinner;
        BallController.OnBallRaffled += SetUpdateInfoScreen;
        BallController.OnBallRaffled += PermissionCallNewBallBall;
    }
    private void OnDisable()
    {
        BallController.OnBallRaffled -= VerifyWinner;
        BallController.OnBallRaffled -= SetUpdateInfoScreen;
        BallController.OnBallRaffled -= PermissionCallNewBallBall;
    }
    public void PopulateInfosGlobe(string _editionName, string _editionNumber, string _date, int _order, string _description, string _value)
    {
        GameManager.instance.globeScriptable.editionName = _editionName;
        GameManager.instance.globeScriptable.editionNumber = _editionNumber;
        GameManager.instance.globeScriptable.date = _date;
        GameManager.instance.globeScriptable.order = _order;
        GameManager.instance.globeScriptable.description = _description;
        GameManager.instance.globeScriptable.value = _value;
    }

    public void PermissionCallNewBallBall()
    {
        canRafleeBall = true;
        GameManager.instance.globeScriptable.indexBalls++;

    }
    public void UpdateScreenRaffle(string[] _ballsRaffled, int _forOneBall, int _winnersCount, string _prizeValue)
    {
       
        if (GameManager.instance.globeScriptable.numberBalls.Count < _ballsRaffled.Length)
        {
            //CancelInvoke("VerifyBalls");
            VerifyBalls();
            GameManager.instance.globeScriptable.numberBalls.Clear();
            GameManager.instance.globeScriptable.numberBalls.AddRange(_ballsRaffled.ToList());

        }
        else if (GameManager.instance.globeScriptable.numberBalls.Count > _ballsRaffled.Length)
        {

            ballController.SetRevokedBall(_ballsRaffled);
            UpdateInfosScreen(_ballsRaffled.Length, _forOneBall);

        }
        
        if (_forOneBall > 0)
        {
            lastBallRaffle.PlayAnimationHeart(true);
        }
        else
        {
            lastBallRaffle.PlayAnimationHeart(false);
        }
        TicketScreen.instance.SetLastBallGlobeRaffle(_ballsRaffled[_ballsRaffled.Length - 1]);

        GameManager.instance.globeScriptable.Winners = _winnersCount;
        GameManager.instance.globeScriptable.prizeValue = int.Parse(_prizeValue);

        GameManager.instance.globeScriptable.ballRaffledCount = _ballsRaffled.Length;
        GameManager.instance.globeScriptable.possiblesWinnersCount = _forOneBall;

    }
    public void VerifyBalls()
    {
        if (GameManager.instance.globeScriptable.indexBalls < GameManager.instance.globeScriptable.numberBalls.Count)
            if (timeToSpawn <= 0)
            {
                if (canRafleeBall == true)
                {
                    if (GameManager.instance.globeScriptable.numberBalls.Count <= 60)
                    {
                        StartCoroutine(ballController.ShowBigBall(GameManager.instance.globeScriptable.numberBalls[GameManager.instance.globeScriptable.indexBalls]));
                        timeToSpawn = 3f;
                        canRafleeBall = false;
                    }
                }
            }
        Invoke("VerifyBalls", 0.5f);
    }
    private void VerifyWinner()
    {
        if (GameManager.instance.globeScriptable.Winners > 0)
        {
            //winnersScreen.SetWinnersScreenVisibility(true);
            winnersScreen.SetInfosWinnerScreen(GameManager.instance.globeScriptable.Winners, GameManager.instance.globeScriptable.prizeValue);
            UiGlobeManager uiGlobeManager = FindObjectOfType<UiGlobeManager>();
           
        }
    }
    public void SetUpdateInfoScreen()
    {
        UpdateInfosScreen(GameManager.instance.globeScriptable.ballRaffledCount, GameManager.instance.globeScriptable.possiblesWinnersCount);

    }
    private void UpdateInfosScreen(int _totalNumberBalls, int _forOneBalls)
    {
        totalBallsCount.SetInfoTotalBall(_totalNumberBalls.ToString());
        lastBallRaffle.SetTicketForOneBallInfo(_forOneBalls.ToString());
    }
    #region SEND MESSAGES

    //public void SendMessageConfirmBallRaffled()
    //{
    //    Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.messageConfirmBall);
    //    NetworkManager.Client.Send(message);
    //    //  NetworkManager.Client2.Send(message);
    //}


    private void FixedUpdate()
    {
        if (timeToSpawn > 0)
        {
            timeToSpawn -= Time.deltaTime;
        }



    }
    #endregion

}

