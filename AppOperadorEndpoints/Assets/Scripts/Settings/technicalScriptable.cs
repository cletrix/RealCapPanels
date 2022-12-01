using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technical", menuName = "Settings/Technical")]

public class TechnicalScriptable : ScriptableObject
{
    public int currentSceneID;
    public int panelActive;
    public bool isVisibleRaffle;
    public int forTwoBalls;
    public List<GlobeRaffleScriptable.porUmaBola> forOneBalls = new List<GlobeRaffleScriptable.porUmaBola>();
    public List<TicketInfos> ticketInfos;

    public void ResetInfos()
    {
        currentSceneID = 0;
        panelActive = 0;
        isVisibleRaffle = false;
        forTwoBalls = 0;
        forOneBalls.Clear();
        ticketInfos.Clear();
    }
    public void UpdateConfig(int sceneId, bool raffleVisibility, int _forTwoBalls, List<GlobeRaffleScriptable.porUmaBola> _forOneBall)
    {
        if (!GameManager.instance.isBackup)
        {
            currentSceneID = sceneId;
            isVisibleRaffle = raffleVisibility;
            forTwoBalls = _forTwoBalls;
            forOneBalls = _forOneBall;

            RestNetworkManager.instance.CallWriteMemory();
        }
    }
    public void UpdateTicketInfo(List<TicketInfos> _tickets)
    {
        if (!GameManager.instance.isBackup)
        {
            ticketInfos.Clear();
            ticketInfos.AddRange(_tickets);
            RestNetworkManager.instance.CallWriteMemory();
        }
    }
    public void PopulateConfig()
    {
        for (int i = 0; i < forOneBalls.Count; i++)
        {
            forOneBalls[i].numeroChance = forOneBalls[i].numeroChance.Insert(1, "°");
        }
        GameManager.instance.sceneId = currentSceneID;
        GameManager.instance.isVisibleRaffle = isVisibleRaffle;
        GameManager.instance.globeRaffleScriptable.porDuasBolas = forTwoBalls;
        GameManager.instance.globeRaffleScriptable.porUmaBolas.Clear();
        GameManager.instance.globeRaffleScriptable.porUmaBolas.AddRange(forOneBalls);
        GameManager.instance.globeRaffleScriptable.ganhadorContemplado = new TicketInfos[ticketInfos.Count];
        for (int i = 0; i < ticketInfos.Count; i++)
        {
            GameManager.instance.globeRaffleScriptable.ganhadorContemplado[i] = ticketInfos[i];
        }

        GameManager.instance.RecoveryScreen();



    }

}

