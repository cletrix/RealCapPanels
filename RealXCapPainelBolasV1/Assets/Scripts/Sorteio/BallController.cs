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
  
    public void PopulateListBalls()
    {
        balls.Clear();
        BallSlot[] childrens = groupBalls.GetComponentsInChildren<BallSlot>();
        balls.AddRange(childrens);
        SetNumberInBalls();
    }
    private void SetNumberInBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            int number = i + 1;
            balls[i].SetNumberInText(number.ToString());
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
    }

    public void UpdateScreenBalls(List<int> ballsRaffled)
    {
        ResetBalls();
        UIManager.instance.panelData.indexBalls--;
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
        if (UIManager.instance.panelData.winnersCount > 0)
        {
            Invoke("DisableAllBallsNoDrawn", 4f);
        }
        else
            UIManager.instance.canRaffleBall = true;
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
    }
}
