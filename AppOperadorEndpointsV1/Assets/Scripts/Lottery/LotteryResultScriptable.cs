using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LotteryResult", menuName = "Settings/LotteryResult")]
public class LotteryResultScriptable : ScriptableObject
{
    public string combinacaoResultado;

    [Header("Ticket Winner")]
    public TicketInfos ganhadorContemplado;
}
