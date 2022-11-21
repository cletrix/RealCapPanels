using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PanelSettings", menuName = "Settings/PanelSettings")]
public class PanelScriptable : ScriptableObject
{
    public List<int> Balls;
    public int indexBalls = 0;

    public void ResetVariables()
    {
        Balls.Clear();
        indexBalls = 0;
    }
}
