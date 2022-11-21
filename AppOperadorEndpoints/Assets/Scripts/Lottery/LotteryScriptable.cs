using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LotterySettings", menuName = "Settings/LotterySettings")]

public class LotteryScriptable : ScriptableObject
{
    public int sorteioOrdem;
    public string sorteioDescricao;
    public string sorteioValor;
    public string resultadoLoteriaFederalNumeroConcurso;
    public string resultadoLoteriaFederalDataConcurso;
    public string resultadoLoteriaFederalPrimeiroSorteio;
    public string resultadoLoteriaFederalSegundoSorteio;
    public string resultadoLoteriaFederalTerceiroSorteio;
    public string resultadoLoteriaFederalQuartoSorteio;
    public string resultadoLoteriaFederalQuintoSorteio;
    public string resultadoLoteriaFederalResultado;
}
