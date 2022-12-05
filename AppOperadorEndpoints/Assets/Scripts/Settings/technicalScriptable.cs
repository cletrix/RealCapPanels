using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technical", menuName = "Settings/Technical")]

public class TechnicalScriptable : ScriptableObject
{
    public int currentSceneID;
    public int currentRaffle;
    public int panelActive;
    public bool isVisibleRaffle;
    public int forTwoBalls;
    public List<GlobeRaffleScriptable.porUmaBola> forOneBalls = new List<GlobeRaffleScriptable.porUmaBola>();
    public List<TicketInfos> ticketInfos;

    public List<bool> ticketsShown;
    public int currentTicketIndex;
    public bool isTicketVisible;

    public List<string> spinNumbers;
    public int spinIndex = 0;

    public void ResetInfos()
    {
        currentSceneID = 0;
        currentSceneID = 1;
        panelActive = 0;
        currentTicketIndex = 0;
        forTwoBalls = 0;
        isVisibleRaffle = false;
        isTicketVisible = false;
        forOneBalls.Clear();
        ticketInfos.Clear();

        spinIndex = 0;
        spinNumbers.Clear();
    }
    public void UpdateConfig(int sceneId, int _currentRaffle, bool raffleVisibility, int _forTwoBalls, List<GlobeRaffleScriptable.porUmaBola> _forOneBall,
        List<TicketInfos> _tickets, List<bool> _ticketsShown, int _currentTicketIndex, bool _isTicketVisible)
    {
        if (!GameManager.instance.isBackup)
        {
            currentRaffle = _currentRaffle;
            currentSceneID = sceneId;
            isVisibleRaffle = raffleVisibility;
            forTwoBalls = _forTwoBalls;
            forOneBalls = _forOneBall;

            ticketInfos.Clear();
            ticketInfos.AddRange(_tickets);
            ticketsShown.Clear();
            ticketsShown.AddRange(_ticketsShown);
            currentTicketIndex = _currentTicketIndex;
            isTicketVisible = _isTicketVisible;

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
        GameManager.instance.globeScriptable.sorteioOrdem = currentRaffle;
        GameManager.instance.isVisibleRaffle = isVisibleRaffle;
        GameManager.instance.globeRaffleScriptable.porDuasBolas = forTwoBalls;
        GameManager.instance.globeRaffleScriptable.porUmaBolas.Clear();
        GameManager.instance.globeRaffleScriptable.porUmaBolas.AddRange(forOneBalls);
        GameManager.instance.globeRaffleScriptable.ganhadorContemplado = new TicketInfos[ticketInfos.Count];
        GameManager.instance.globeRaffleScriptable.ticketListVisible = new bool[ticketInfos.Count];
        GameManager.instance.isTicketVisible = isTicketVisible;
        GameManager.instance.ticketWinnerIndex = currentTicketIndex;

        for (int i = 0; i < ticketInfos.Count; i++)
        {
            GameManager.instance.globeRaffleScriptable.ganhadorContemplado[i] = ticketInfos[i];
            GameManager.instance.globeRaffleScriptable.ticketListVisible[i] = ticketsShown[i];
        }
        GameManager.instance.RecoveryScreen();
    }

    public void UpdateSpinConfig(int _spinIndex, string _spinNumber)
    {
        spinIndex = _spinIndex;
        if (!spinNumbers.Contains(_spinNumber))
            spinNumbers.Add(_spinNumber);

        RestNetworkManager.instance.CallWriteMemory();

    }

    public void PopulateSpinConfig()
    {
        GameManager.instance.spinScriptable.sorteioOrdem = spinIndex;
        GameManager.instance.RecoverySpin();
    }
}

