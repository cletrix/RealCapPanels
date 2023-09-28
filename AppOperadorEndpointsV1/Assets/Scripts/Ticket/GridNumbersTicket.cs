using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridNumbersTicket : ConfigGridController
{
    [Header("COMPONENTS")]
    [SerializeField] private SelectBall selectBall;

    [SerializeField] List<SelectBall> selectBalls;

    void Start()
    {
        SetGridBalls(GameManager.instance.GetCountBallsCard());
    }

    private void SpawnBgBalls(int _amountBalls)
    {
        selectBalls.Clear();
        for (int i = 0; i < _amountBalls; i++)
        {
            SelectBall inst = Instantiate(selectBall, transform.position, Quaternion.identity);
            inst.transform.SetParent(gameObject.transform);
            inst.transform.localScale = Vector3.one;
            inst.InitializeVariables();
            inst.DisableButton();
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
            case 20:
                {
                    ConfigGridBalls(65, 65, 5, 5, 4);
                    break;
                }
            case 15:
                {
                    ConfigGridBalls(65, 70, 5, 25, 3);
                    break;
                }
            
        }
        SpawnBgBalls(_maxBalls);
    }
}
