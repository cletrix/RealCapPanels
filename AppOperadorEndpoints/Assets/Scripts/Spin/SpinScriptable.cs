using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinSettings", menuName = "Settings/SpinSettings")]
public class SpinScriptable : ScriptableObject
{
    public int sorteioOrdem;
    public string sorteioDescricao;
    public string sorteioValor;

    public void SetNewOrder(int order)
    {
        sorteioOrdem = order;
    }
}
