using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigGridController : MonoBehaviour
{
    [Header("COMPONENTS SPAWN")]

    [SerializeField] private GridLayoutGroup gridSelectBalls;
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
   
}
