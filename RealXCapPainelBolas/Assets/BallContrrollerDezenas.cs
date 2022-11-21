using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BallContrrollerDezenas : MonoBehaviour
{
    public GroupBalls ballsList;

    [Space]
    public int indexNumberBall;
    public int countBallsRaffle;
    public int sortCount = 0;
    public string numberSTR;
    public int currentRaffle;
    void Start()
    {
        SetNumberInBalls();
        SetNormalCurrentRaffle();
        Invoke("CallSelectCurrentRaffle", 1f);
    }

    public void CallSelectCurrentRaffle()
    {
        SetSelectCurrentRaffle(currentRaffle);
    }
    private void SetSelectCurrentRaffle(int index)
    {
        ballsList.groupBalls[index].groupBg.color = ballsList.selectedColor;
        ballsList.groupBalls[index].txtPrize.color = Color.black;

    }
    private void SetNumberInBalls()
    {
        foreach (var item in ballsList.groupBalls)
        {
            item.txtPrize.text = item.prizeName;
            for (int i = 0; i < item.balls.Count; i++)
            {
                item.balls[i].SetNumberBall(i);
            }
        }
    }

    private void SetNormalCurrentRaffle()
    {
        foreach (var item in ballsList.groupBalls)
        {
            item.groupBg.color = ballsList.normalColor;
            item.txtPrize.color = Color.white;

        }

    }
    private void SetFinishCurrentRaffle(int index)
    {

        ballsList.groupBalls[index].groupBg.color = ballsList.finishColor;
        ballsList.groupBalls[index].txtPrize.color = Color.white;

    }

    public void DesactiveAllBorder()
    {
        for (int i = 0; i < ballsList.groupBalls[currentRaffle].balls.Count; i++)
        {
            if (ballsList.groupBalls[currentRaffle].balls[i].isActive)
                ballsList.groupBalls[currentRaffle].balls[i].ballBorder.SetActive(false);
        }
    }
    private void ActiveNewBallInCurrentRaffle(int index)
    {
        DesactiveAllBorder();
        ballsList.groupBalls[currentRaffle].balls[index].SetEnableBallBorder();

    }

    //  private void SetEnableBallsTest(int index)
    // {
    //     foreach (var item in ballsList.groupBalls)
    //     {
    //         for (int i = 0; i < item.balls.Count; i++)
    //         {
    //             item.balls[i].SetRestoredBalls();
    //         }
    //     }

    // }

    // public void DesactiveAllBorder()
    // {
    //     for (int i = 0; i < balls.Count; i++)
    //     {
    //         if (balls[i].isActive)
    //             balls[i].ballBorder.SetActive(false);
    //     }
    // }

    // public void ShowBallsRestored(List<int> BallsIndex)
    // {
    //     DesactiveAllBorder();
    //     for (int i = 0; i < BallsIndex.Count; i++)
    //     {
    //         balls[BallsIndex[i] - 1].SetRestoredBalls();
    //         if (i == BallsIndex.Count - 1)
    //             balls[BallsIndex[i] - 1].SetEnableBallBorder();
    //     }
    // }

    // public void ShowBallRaffled(int index)
    // {
    //     DesactiveAllBorder();
    //     balls[index - 1].SetEnableBallBorder();
    // }

    void Update()
    {
        if (numberSTR.Length < 2)
        {
            var input = Input.inputString;
            switch (input)
            {
                case "0":
                    {
                        numberSTR += input;
                        break;
                    }
                case "1":
                    {
                        numberSTR += input;

                        break;
                    }
                case "2":
                    {
                        numberSTR += input;

                        break;
                    }
                case "3":
                    {
                        numberSTR += input;

                        break;
                    }
                case "4":
                    {
                        numberSTR += input;

                        break;
                    }
                case "5":
                    {
                        numberSTR += input;

                        break;
                    }
                case "6":
                    {
                        numberSTR += input;

                        break;
                    }
                case "7":
                    {
                        numberSTR += input;

                        break;
                    }
                case "8":
                    {
                        numberSTR += input;

                        break;
                    }
                case "9":
                    {
                        numberSTR += input;

                        break;
                    }

            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && numberSTR.Length > 0)
        {
            //  ShowBallRaffled(System.Convert.ToInt32(numberSTR));
            ActiveNewBallInCurrentRaffle(System.Convert.ToInt32(numberSTR));
            numberSTR = string.Empty;

        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            SetFinishCurrentRaffle(currentRaffle);
            currentRaffle++;
            SetSelectCurrentRaffle(currentRaffle);

        }

    }
    [System.Serializable]
    public class GroupBalls
    {
        public Color normalColor;
        public Color selectedColor;
        public Color finishColor;
        public List<BallsDezenas> groupBalls;
    }

    [System.Serializable]
    public class BallsDezenas
    {
        public string prizeName;
        public TextMeshProUGUI txtPrize;
        public Image groupBg;
        public List<BallSlot> balls;
    }
}
