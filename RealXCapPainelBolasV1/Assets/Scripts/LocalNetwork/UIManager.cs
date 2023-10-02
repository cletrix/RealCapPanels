using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RiptideNetworking;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    public static UIManager instance
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    public BallController ballController;
    public GridBallController gridBallController;
    public Resoucers.PanelData panelData;

    [SerializeField] private float timeToSpawn;
    public float maxTimetoSpawn = 1.5f;
    public bool canRaffleBall = true;


    private void Awake()
    {
        instance = this;
        gridBallController = FindObjectOfType<GridBallController>();
    }
    public void SetMaxBallGrid(int _maxBall)
    {
        panelData.maxBallsGrid = _maxBall;
        gridBallController.SetGridBalls(panelData.maxBallsGrid);
        ballController.PopulateListBalls();
    }
    public int GetMaxGridBalls()
    {
        return panelData.maxBallsGrid;
    }
    private void Start()
    {
        Invoke("Connect", 1f);
        panelData.ResetVariables();
        timeToSpawn = maxTimetoSpawn;
    }
    private void FakeDraw()
    {
        List<int> balls = new List<int>();
        balls.AddRange(panelData.Balls);
        RecieveBalls(balls, 0);
    }
    public void Connect()
    {
        NetworkManager.Singleton.Connect();
    }

    public void ShowBall()
    {
        ballController.DesactiveAllBorder();
        ballController.ShowBallRaffled(panelData.Balls[panelData.indexBalls]);
        panelData.indexBalls++;
    }
    public void ResetRaffle()
    {
        ballController.ResetBalls();
        panelData.ResetVariables();
    }
    public void RecieveBalls(List<int> ballsRaffled, int _winnersCount)
    {
        panelData.winnersCount = _winnersCount;
        if (ballsRaffled.Count > panelData.Balls.Count)
        {
            panelData.Balls.Clear();
            panelData.Balls.AddRange(ballsRaffled);
            VerifyBalls();
        }
        else if (ballsRaffled.Count < panelData.Balls.Count)
        {

            panelData.Balls.Clear();
            panelData.Balls.AddRange(ballsRaffled);
            ballController.UpdateScreenBalls(ballsRaffled);
        }
    }

    public void VerifyBalls()
    {
        if (panelData.indexBalls < panelData.Balls.Count)
            if (timeToSpawn <= 0)
            {
                if (canRaffleBall == true)
                {
                    if (panelData.Balls.Count <= 60)
                    {
                        Invoke("ShowBall", 0.5f);
                        timeToSpawn = maxTimetoSpawn;
                        canRaffleBall = false;
                    }
                }
            }
        Invoke("VerifyBalls", 0.5f);
    }

    private void FixedUpdate()
    {
        if (timeToSpawn > 0)
        {
            timeToSpawn -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Application.Quit();
            }
        }
    }

}

