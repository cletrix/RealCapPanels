using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;
using System;

public class BallController : MonoBehaviour
{
    public Transform parentBack;
    public Transform parentFront;
    public Transform spawnerPointBallsFinal;
    [Space]
    public List<Transform> positionBallsGrid;
    public List<Transform> spawnerPointBallsStart;
    [Space]
    public ballAnimGlobeController animeGlobe;
    public BigBall bigBall;
    public Ball ball;
    public List<Ball> BallsSelected;
    [Space]
    public string numberBall;
    public int ballCountSpawn;
    public bool isLoop = false;
    public float timeBetweenBalls = 2f;

    [Header("Restore Balls")]
    public int ballCount = 5;
    public List<int> lastIndexBallsRaffle;

    public static event Action OnBallRaffled;

    private void Start()
    {
        animeGlobe = FindObjectOfType<ballAnimGlobeController>();
    }
    public IEnumerator ShowBigBall(string _number)
    {
        numberBall = _number;

        animeGlobe.SetCurrentBall(int.Parse(numberBall));
        while (animeGlobe.isFinishMovement == false)
        {
            yield return null;
        }
        bigBall.SetInfoInBigBall(numberBall);
        OnBallRaffled?.Invoke();
        if (BallsSelected.Count < 5 && !isLoop)
        {
            StartCoroutine(SpawnerBall());
        }
        else
        {
            isLoop = true;
        }
        if (isLoop)
        {
            StartCoroutine(SpawnerBallLoop());
        }
    }
    public IEnumerator SpawnerBall()
    {
        Ball inst = Instantiate(ball, spawnerPointBallsStart[0].position, Quaternion.identity);
        BallsSelected.Add(inst);
        inst.transform.SetParent(parentBack);
        inst.transform.localScale = Vector3.one;
        inst.SetInfoBall(numberBall);
        SetBallWinner();
        yield return new WaitForSeconds(0.5f);
        MovementOnSpawner(inst);
    }
    //Após mostrar as 5 primeira bolas sorteadas, essa função cria um loop para exibir seguinte bolas sorteadas.
    public IEnumerator SpawnerBallLoop()
    {
        Ball inst = Instantiate(ball, spawnerPointBallsStart[0].position, Quaternion.identity);
        BallsSelected.Add(inst);
        inst.transform.SetParent(parentBack);
        inst.transform.localScale = Vector3.one;
        inst.SetInfoBall(numberBall);
        BallsSelected[0].ExitBall(spawnerPointBallsFinal, spawnerPointBallsStart[0], parentBack);
        BallsSelected[0].RotateLoop(360);
        yield return new WaitForSeconds(0.2f);
        BallsSelected.Remove(BallsSelected[0]);
        SetBallWinner();
        yield return new WaitForSeconds(0.3f);
        MovementOnSpawnerLoop(inst);

    }
    private void MovementOnSpawner(Ball instance)
    {
        timeBetweenBalls = 2f;
        instance.RotateLoop(-360);
        Sequence seq = DOTween.Sequence();
        seq.Insert(0, instance.transform.DOMove(spawnerPointBallsStart[1].position, 0.3f).OnComplete(() =>
        {
            instance.SetSize(_delay: 0f);
            instance.transform.SetParent(parentFront);
            instance.transform.DOMove(instance.transform.position, 0.2f);
        }));

        seq.Insert(1, instance.transform.DOMove(spawnerPointBallsStart[2].position, 0.2f).OnComplete(() =>
        {
            instance.MoveBallAtFinalPos(positionBallsGrid[ballCountSpawn].position);
            //GlobeController globe = new GlobeController();
            //globe.SendMessageConfirmBallRaffled();

            ballCountSpawn++;
            if (ballCountSpawn < 5)
            {
                instance.RotateLoop(360);
            }

        }));
    }
    private void MovementOnSpawnerLoop(Ball instance)
    {
        timeBetweenBalls = 1.5f;
        instance.RotateLoop(-360);
        Sequence seq = DOTween.Sequence();
        seq.Insert(0, instance.transform.DOMove(spawnerPointBallsStart[1].position, 0.3f).OnComplete(() =>
        {
            instance.SetSize();
            instance.transform.SetParent(parentFront);

        }));
        seq.Insert(1, instance.transform.DOMove(spawnerPointBallsStart[2].position, 0.2f).SetDelay(0.2f).OnComplete(() =>
        {
            instance.MoveBallAtFinalPos(positionBallsGrid[ballCountSpawn - 1].position);
            Invoke("CallWinnerScreen", 2f);

        }));
        for (int i = 0; i < positionBallsGrid.Count - 1; i++)
        {
            BallsSelected[i].MoveBallAtFinalPos(positionBallsGrid[i].position);
            BallsSelected[i].RotateLoop(rotZ: 360, rotSpeed: 1f, loop: 1);
        }

    }
    public void CallWinnerScreen()
    {
        if (GameManager.instance.globeScriptable.Winners > 0)
        {
            WinnersScreen.instance.SetWinnersScreenVisibility(true);

        }
    }
    public void SetRevokedBall(string[] _numbers)
    {
        GetLastIndexBallsRaffleAfterRevoked(_numbers.Length, _numbers.ToList());
    }
    private void GetLastIndexBallsRaffleAfterRevoked(int count, List<string> _numbers)
    {
        lastIndexBallsRaffle.Clear();
        ballCount = 5;
        for (int i = ballCount; i > 0; i--)
        {
            if ((count - ballCount) >= 0)
            {
                int index = count - ballCount;
                lastIndexBallsRaffle.Add(index);
            }
            ballCount--;
        }
        RestoreBallsAfterRevoked(_numbers);
        GameManager.instance.globeScriptable.indexBalls--;

    }
    ////Essa função mostra na tela em até um máximo das 5 ultimas bolas sorteadas.
    public void RestoreBallsAfterRevoked(List<string> _numbers)
    {
        List<string> _ballsRaflled = new List<string>();
        _ballsRaflled.AddRange(GameManager.instance.globeScriptable.numberBalls);
        animeGlobe.ResetBall(int.Parse(_ballsRaflled[_ballsRaflled.Count - 1]));

        if (_ballsRaflled.Count <= 5)
        {
            for (int i = 0; i < BallsSelected.Count; i++)
            {
                if (BallsSelected[i].numberBall == _ballsRaflled[_ballsRaflled.Count - 1])
                {
                    Destroy(BallsSelected[i].gameObject, 0.3f);
                    BallsSelected.Remove(BallsSelected[i]);
                    ballCountSpawn--;
                }
            }
            isLoop = false;
        }
        else
        {
            for (int i = 0; i < lastIndexBallsRaffle.Count; i++)
            {
                BallsSelected[i].SetInfoBall(_ballsRaflled[lastIndexBallsRaffle[i]]);
            }
        }

        if (_ballsRaflled.Count > 0)
        {
            bigBall.SetInfoInBigBall(_ballsRaflled[lastIndexBallsRaffle[lastIndexBallsRaffle.Count - 1]], false);
        }
        else
        {
            bigBall.SetBgBallWithLogo();
        }
        GameManager.instance.globeScriptable.numberBalls.Clear();
        GameManager.instance.globeScriptable.numberBalls.AddRange(_numbers.ToList());
    }
    public void GetLastIndexBallsRaffle(int count, List<string> _numbers)
    {
        lastIndexBallsRaffle.Clear();
        ballCount = 5;
        for (int i = ballCount; i > 0; i--)
        {
            if ((count - ballCount) >= 0)
            {
                int index = count - ballCount;
                lastIndexBallsRaffle.Add(index);
            }
            ballCount--;
        }
        StartCoroutine(RestoreBalls(lastIndexBallsRaffle, _numbers));
    }
    ////Essa função mostra na tela em até um máximo das 5 ultimas bolas sorteadas.
    public IEnumerator RestoreBalls(List<int> lastFiveBalls, List<string> _numbers)
    {

        for (int i = 0; i < lastFiveBalls.Count; i++)
        {
            StartCoroutine(ShowBigBall(_numbers[lastFiveBalls[i]].ToString()));
            yield return new WaitForSeconds(2f);
        }
    }

    public void SetBallWinner()
    {
        if (GameManager.instance.globeScriptable.Winners > 0)
        {
            bigBall.SetBallWinner();
            BallsSelected[BallsSelected.Count - 1].SetBallWinner();
        }

    }

}
