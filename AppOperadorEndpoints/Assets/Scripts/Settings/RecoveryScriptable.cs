using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Recovery", menuName = "Settings/Recovery")]
public class RecoveryScriptable : ScriptableObject
{
    public int limit_globo;
    public int limit_spin;
    public string sorteio_tipo;
    public int sorteio_globo_numero;
    public int sorteio_spin_numero;
    public List<int> globe_balls;
    public List<int> globe_partial_result;
    public List<List<int>> titulo_winner_globe;
    public List<List<int>> card_winner_globe;
    public List<List<string>> titulo_winner_giro;
    public List<List<string>> number_winner_giro;
}
