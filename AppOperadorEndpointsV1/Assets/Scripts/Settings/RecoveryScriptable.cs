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
    public List<int> globe_balls = new List<int>();
    public List<int> globe_partial_result = new List<int>();
    public List<string> titulo_winner_giro = new List<string>();
    public List<string> number_winner_giro = new List<string>();

    public List<List<int>> titulo_winner_globe = new List<List<int>>();
    public List<List<int>> card_winner_globe = new List<List<int>>();
    public List<List<int>> winner_globo_balls = new List<List<int>>();


    public void ResetInfos()
    {
        limit_globo = 0;
        limit_spin = 0;
        sorteio_tipo = string.Empty;
        sorteio_globo_numero = 0;
        sorteio_spin_numero = 0;
        globe_balls.Clear();
        globe_partial_result.Clear();
        titulo_winner_giro.Clear();
        number_winner_giro.Clear();
        titulo_winner_globe.Clear();
        card_winner_globe.Clear();
        winner_globo_balls.Clear();
    }
    public void UpdateInfos()
    {
        //GLOBE RECOVERY
        GameManager.instance.globeRaffleScriptable.bolasSorteadas.Clear();
        GameManager.instance.technicalScriptable.spinNumbers.Clear();
        foreach (var item in globe_balls)
        {
            GameManager.instance.globeRaffleScriptable.bolasSorteadas.Add(item.ToString());
        }

        for (int i = 0; i < number_winner_giro.Count; i++)
        {
            if (number_winner_giro[i].Length > 0)
            {
                GameManager.instance.technicalScriptable.spinNumbers.Add(number_winner_giro[i]);
            }
        }

        ////SPIN RECOVERY
        //GameManager.instance.spinScriptable.sorteioOrdem = GameManager.instance.technicalScriptable.spinIndex+1;
    }


}
