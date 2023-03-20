using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinResult", menuName = "Settings/SpinResult")]
public class SpinResultScriptable : ScriptableObject
{
    public string numeroSorteado;
    public int sorteioOrdem=1;
    [Header("Ticket Winner")]
    public TicketInfos ganhadorContemplado;

    public void SetNewRaffleNumber()
    {
        numeroSorteado = string.Empty;
        for (int i = 0; i < 6; i++)
        {
            int random = Random.Range(0, 9);
            numeroSorteado += random.ToString();
        }
        ganhadorContemplado.numeroSorte = numeroSorteado;
        ganhadorContemplado.numeroTitulo = $"0{numeroSorteado}";

    }


}
