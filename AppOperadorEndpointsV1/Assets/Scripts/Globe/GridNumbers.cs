using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridNumbers : ConfigGridController
{
   
    [Header("COMPONENTS")]
    [SerializeField] private SelectBall selectBall;

    [SerializeField] List<SelectBall> selectBalls;

    void Start()
    {
        SetGridBalls(GameManager.instance.GetCountBallsGrid());
    }

    private void SpawnBgBalls(int _amountBalls)
    {
        selectBalls.Clear();
        for (int i = 0; i < _amountBalls; i++)
        {
            SelectBall inst = Instantiate(selectBall, transform.position, Quaternion.identity);
            inst.transform.SetParent(gameObject.transform);
            inst.transform.localScale = Vector3.one;
            int number = i + 1;
            inst.SetNumberInText(number.ToString("00"));
            selectBalls.Add(inst);
        }
    }
    public void SetGridBalls(int _maxBalls)
    {
        List<object> grid = new List<object>(selectBalls.Cast<object>());
        ResetGrid(grid); ;
        switch (_maxBalls)
        {
            case 30:
                {
                    ConfigGridBalls(100, 100, 30, 20, 10);
                    break;
                }
            case 50:
                {
                    ConfigGridBalls(85, 60, 50, 20, 10);
                    break;
                }
            case 60:
                {
                    ConfigGridBalls(70, 70, 18, 25, 15);
                    break;
                }
            case 75:
                {
                    ConfigGridBalls(66, 66, 20, 25, 15);
                    break;
                }
            case 90:
                {
                    ConfigGridBalls(66, 52, 20, 24, 15);
                    break;
                }
        }
        SpawnBgBalls(_maxBalls);
    }
}
