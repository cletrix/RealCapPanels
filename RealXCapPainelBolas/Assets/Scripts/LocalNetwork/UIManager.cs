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
    public PanelScriptable panelScriptable;

    [SerializeField] private float timeToSpawn ;
    public float maxTimetoSpawn = 1.5f;
    public bool canRaffleBall = true;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Invoke("Connect", 1f);
        panelScriptable.ResetVariables();
        timeToSpawn = maxTimetoSpawn;
    }
    public void Connect()
    {
        NetworkManager.Singleton.Connect();
    }

    public void ShowBall()
    {
        ballController.DesactiveAllBorder();
        ballController.ShowBallRaffled(panelScriptable.Balls[panelScriptable.indexBalls]);
        panelScriptable.indexBalls++;
    }
    public void ResetRaffle()
    {
        ballController.DisableAll();
        panelScriptable.ResetVariables();
    }
    public void RecieveBalls(List<int> ballsRaffled, int _winnersCount)
    {
        panelScriptable.winnersCount = _winnersCount;
        if (ballsRaffled.Count > panelScriptable.Balls.Count)
        {
            panelScriptable.Balls.Clear();
            panelScriptable.Balls.AddRange(ballsRaffled);
            VerifyBalls();
        }
        else if (ballsRaffled.Count < panelScriptable.Balls.Count)
        {

            panelScriptable.Balls.Clear();
            panelScriptable.Balls.AddRange(ballsRaffled);
            ballController.UpdateScreenBalls(ballsRaffled);
        }
    }

    public void VerifyBalls()
    {
        if (panelScriptable.indexBalls < panelScriptable.Balls.Count)
            if (timeToSpawn <= 0)
            {
                if (canRaffleBall == true)
                {
                    if (panelScriptable.Balls.Count <= 60)
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

