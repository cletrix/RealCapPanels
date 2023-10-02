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
    private bool isPlayWinnerSound = false;


    void Start()
    {
        InitializeVariables();
    }
    private void InitializeVariables()
    {
        isWinner = false;
        winnersScreen = FindObjectOfType<WinnersScreen>();
        ballController.GetLastIndexBallsRaffle(GameManager.instance.globeData.numberBalls.Count, GameManager.instance.globeData.numberBalls);
        totalBallsCount.SetInfoTotalBall((GameManager.instance.globeData.ballRaffledCount).ToString());
        lastBallRaffle.SetTicketForOneBallInfo(GameManager.instance.globeData.possiblesWinnersCount.ToString());
        if (GameManager.instance.globeData.possiblesWinnersCount > 0)
        {
            lastBallRaffle.PlayAnimationHeart(true);
        }
        else
        {
            lastBallRaffle.PlayAnimationHeart(false);
        }
        GameManager.instance.SetCamActiveInCanvas(Camera.main);
        isPlayWinnerSound = false;
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
    public void PopulateInfosGlobe(string _editionName, string _editionNumber, string _date, int _order, string _description, float _value)
    {
        GameManager.instance.globeData.editionName = _editionName;
        GameManager.instance.globeData.editionNumber = _editionNumber;
        GameManager.instance.globeData.date = _date;
        GameManager.instance.globeData.order = _order;
        GameManager.instance.globeData.description = _description;
        GameManager.instance.globeData.value = _value;
    }
    public void PermissionCallNewBallBall()
    {
        canRafleeBall = true;
        GameManager.instance.globeData.indexBalls++;
    }
    public void UpdateScreenRaffle(string[] _ballsRaffled, int _forOneBall, int _winnersCount, float _prizeValue)
    {
        if (GameManager.instance.globeData.numberBalls.Count < _ballsRaffled.Length)
        {
            VerifyBalls();
            GameManager.instance.globeData.numberBalls.Clear();
            GameManager.instance.globeData.numberBalls.AddRange(_ballsRaffled.ToList());

        }
        else if (GameManager.instance.globeData.numberBalls.Count > _ballsRaffled.Length)
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

        GameManager.instance.globeData.Winners = _winnersCount;
        GameManager.instance.globeData.prizeValue = _prizeValue;

        GameManager.instance.globeData.ballRaffledCount = _ballsRaffled.Length;
        GameManager.instance.globeData.possiblesWinnersCount = _forOneBall;
    }
    public void VerifyBalls()
    {
        if (GameManager.instance.globeData.indexBalls < GameManager.instance.globeData.numberBalls.Count)
            if (timeToSpawn <= 0)
            {
                if (canRafleeBall == true)
                {
                    if (GameManager.instance.globeData.numberBalls.Count <= 60)
                    {
                        StartCoroutine(ballController.ShowBigBall(GameManager.instance.globeData.numberBalls[GameManager.instance.globeData.indexBalls]));
                        timeToSpawn = 1f;
                        canRafleeBall = false;
                    }
                }
            }
        Invoke("VerifyBalls", 0.5f);
    }
    private void VerifyWinner()
    {
        if (GameManager.instance.globeData.Winners > 0)
        {
            winnersScreen.SetInfosWinnerScreen(GameManager.instance.globeData.Winners, GameManager.instance.globeData.prizeValue);
            UiGlobeManager uiGlobeManager = FindObjectOfType<UiGlobeManager>();
            if (isPlayWinnerSound == false)
            {
                AudioManager.instance.PlaySFX("Winner");
                AudioManager.instance.StopSFX("Heart");
                isPlayWinnerSound = true;
            }
        }
    }
    public void SetUpdateInfoScreen()
    {
        UpdateInfosScreen(GameManager.instance.globeData.ballRaffledCount, GameManager.instance.globeData.possiblesWinnersCount);

    }
    private void UpdateInfosScreen(int _totalNumberBalls, int _forOneBalls)
    {
        totalBallsCount.SetInfoTotalBall(_totalNumberBalls.ToString());
        lastBallRaffle.SetTicketForOneBallInfo(_forOneBalls.ToString());
    }
    #region SEND MESSAGES

    private void FixedUpdate()
    {
        if (timeToSpawn > 0)
        {
            timeToSpawn -= Time.deltaTime;
        }
    }
    #endregion
}

