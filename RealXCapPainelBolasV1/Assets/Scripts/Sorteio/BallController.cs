using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
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
    public void ResetGrid()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].ResetBall();
        }
    }

    public void UpdateScreenBalls(List<int> ballsRaffled)
    {
        ResetGrid();
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
            Invoke("DisableAllBallsNoDrawn",3f);
        }
        else
            UIManager.instance.canRaffleBall = true;
    }

    public void DisableAllBallsNoDrawn()
    {
        foreach (var item in balls)
        {
            if (!item.hasRaffled)
            {
                item.SetDisableBall();
            }
        }
    }
}
