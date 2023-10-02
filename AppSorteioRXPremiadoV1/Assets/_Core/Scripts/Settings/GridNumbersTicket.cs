using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridNumbersTicket : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] private CellGridTicket cellGrid;

    [SerializeField] List<CellGridTicket> cellsGrid;

    [Header("COMPONENTS SPAWN")]

    [SerializeField] private GridLayoutGroup gridSelectBalls;

    //void Start()
    //{
    //    SetGridBalls(GameManager.instance.GetGridBallsTicket());
    //}
    public void ConfigGridBalls(int _cellSizeX, int _cellSizeY, int _spacingX, int _spacingY, int _constraintCount)
    {
        gridSelectBalls.cellSize = new Vector2(_cellSizeX, _cellSizeY);
        gridSelectBalls.spacing = new Vector2(_spacingX, _spacingY);
        gridSelectBalls.constraintCount = _constraintCount;
    }

    public void ResetGrid(List<object> grid)
    {
        for (int i = 0; i < grid.Count; i++)
        {
            Destroy(transform.GetChild(i).gameObject, 0.1f);
        }
    }
   
    private void SpawnBgBalls(int _amountBalls)
    {
        cellsGrid.Clear();
        for (int i = 0; i < _amountBalls; i++)
        {
            CellGridTicket inst = Instantiate(cellGrid, transform.position, Quaternion.identity);
            inst.transform.SetParent(gameObject.transform);
            inst.transform.localScale = Vector3.one;
            int number = i + 1;
            inst.SetNumberInText(number.ToString("00"));
            cellsGrid.Add(inst);
        }
    }
    public void SetGridBalls(int _maxBalls)
    {
        List<object> grid = new List<object>(cellsGrid.Cast<object>());
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
