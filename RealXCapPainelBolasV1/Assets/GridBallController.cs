using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridBallController : MonoBehaviour
{

    [Header("COMPONENTS SPAWN")]

    [SerializeField] private GridLayoutGroup gridSelectBalls;
    [Header("COMPONENTS")]
    [SerializeField] private BallSlot selectBall;

    [SerializeField] List<BallSlot> selectBalls;


    private void SpawnBgBalls(int _amountBalls)
    {
        selectBalls.Clear();
        for (int i = 0; i < _amountBalls; i++)
        {
            BallSlot inst = Instantiate(selectBall, transform.position, Quaternion.identity);
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
        ClearGrid(grid);
        switch (_maxBalls)
        {
            case 30:
                {
                    ConfigGridBalls(330, 330, 280, 80, 6);
                    break;
                }
            case 50:
                {
                    ConfigGridBalls(85, 60, 50, 20, 10);
                    break;
                }
            case 60:
                {
                    ConfigGridBalls(250, 250, 120, 80, 10);
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
    public void ConfigGridBalls(int _cellSizeX, int _cellSizeY, int _spacingX, int _spacingY, int _constraintCount)
    {
        gridSelectBalls.cellSize = new Vector2(_cellSizeX, _cellSizeY);
        gridSelectBalls.spacing = new Vector2(_spacingX, _spacingY);
        gridSelectBalls.constraintCount = _constraintCount;
    }

    public void ClearGrid(List<object> grid)
    {
        for (int i = 0; i < grid.Count; i++)
        {
            Destroy(transform.GetChild(i).gameObject, 0.1f);
        }
    }


}
