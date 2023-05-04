using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BallController : MonoBehaviour
{
    public GridLayoutGroup groupBalls;
    public List<BallSlot> balls;

    public int indexNumberBall;
    public int countBallsRaffle;
    public int sortCount = 0;
    public string numberSTR;
    void Start()
    {
        SetNumberInBalls();
    }
    private void SetNumberInBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].SetNumberBall(i + 1);
        }
    }

    public void DesactiveAllBorder()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].ballBorder.SetActive(false);
            if (balls[i].isActive)
            {
                balls[i].SetFinishedColor();
            }
        }
    }
    public void ResetBalls()
    {
        UIManager.instance.canRaffleBall = true;
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].ResetBall();
        }
        ResetGrid();
    }

    public void UpdateScreenBalls(List<int> ballsRaffled)
    {
        ResetBalls();
        UIManager.instance.panelScriptable.indexBalls--;
        for (int i = 0; i < ballsRaffled.Count; i++)
        {
            if (i < ballsRaffled.Count - 1)
            {
                balls[ballsRaffled[i] - 1].SetFinishedColor();
            }
            else if (i == ballsRaffled.Count - 1)
            {
                balls[ballsRaffled[i] - 1].SetSelectedBall();
            }
        }

    }

    public void ShowBallRaffled(int index)
    {
        DesactiveAllBorder();
        balls[index - 1].SetEnableBallBorder();
        if (UIManager.instance.panelScriptable.winnersCount > 0)
        {
            Invoke("DisableAllBallsNoDrawn", 4f);
        }
        else
            UIManager.instance.canRaffleBall = true;
    }

    public void UpdateGrid(int _ballsDrawn)
    {
        if (_ballsDrawn > 40 )
        {
            groupBalls.cellSize = new Vector2(300, 300);
            groupBalls.spacing = new Vector2(60, 50);
        }
        else if (_ballsDrawn <= 40 )
        {
            groupBalls.cellSize = new Vector2(300, 300);
            groupBalls.spacing = new Vector2(80, 80);
        }
    }
    public void ResetGrid()
    {
        groupBalls.cellSize = new Vector2(250, 250);
        groupBalls.spacing = new Vector2(120, 80);
    }
    public void DisableAllBallsNoDrawn()
    {
        int enableBalls = 0;
        foreach (var item in balls)
        {
            if (!item.hasRaffled)
            {
                item.SetDisableBall();
            }
            else
            {
                enableBalls++;
            }
        }
        UpdateGrid(enableBalls);
    }
}
