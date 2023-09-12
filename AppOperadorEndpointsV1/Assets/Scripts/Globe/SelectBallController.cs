using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBallController : MonoBehaviour
{
    [Header("COMPONENTS SPAWN")]

    [SerializeField] private GridLayoutGroup gridSelectBalls;

    [Header("COMPONENTS")]
    [SerializeField] private SelectBall selectBall;

    [SerializeField] int maxBalls = 60;
    [SerializeField] List<SelectBall> selectBalls;

    [SerializeField] private bool canActiveButtons = true;
    void Start()
    {
        SetGridBalls(maxBalls);
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
    private void ConfigGridBalls(int _cellSizeX, int _cellSizeY, int _spacingX, int _spacingY, int _constraintCount)
    {
        gridSelectBalls.cellSize = new Vector2(_cellSizeX, _cellSizeY);
        gridSelectBalls.spacing = new Vector2(_spacingX, _spacingY);
        gridSelectBalls.constraintCount = _constraintCount;
    }

    private void ResetGrid()
    {
        for (int i = 0; i < selectBalls.Count; i++)
        {
            Destroy(transform.GetChild(i).gameObject, 0.1f);
        }
    }
    public void SetGridBalls(int _maxBalls)
    {
        ResetGrid();
        maxBalls = _maxBalls;
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
