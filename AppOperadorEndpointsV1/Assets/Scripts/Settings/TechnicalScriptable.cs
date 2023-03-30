using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technical", menuName = "Settings/Technical")]

public class TechnicalScriptable : ScriptableObject
{
    public int currentSceneID;
    public int currentRaffle;
    public string currentGlobeDesc;
    public float currentGlobeValue;
    public int panelActive;
    public bool isVisibleRaffle;
    public int forTwoBalls;
    public List<GlobeRaffleScriptable.porUmaBola> forOneBalls = new List<GlobeRaffleScriptable.porUmaBola>();
    public List<TicketInfos> ticketInfos;

    public List<bool> ticketsShown;
    public int currentTicketIndex;
    public bool isTicketVisible;

    public List<string> spinNumbers;
    public int spinIndex = 1;
    public TicketInfos ticketSpin;

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

        spinIndex = 1;
        spinNumbers.Clear();
    }
    public void UpdateConfig(int sceneId, int _currentRaffle, bool raffleVisibility, int _forTwoBalls, List<GlobeRaffleScriptable.porUmaBola> _forOneBall,
        List<TicketInfos> _tickets, List<bool> _ticketsShown, int _currentTicketIndex, bool _isTicketVisible)
    {
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
        currentRaffle = _currentRaffle;
        currentGlobeDesc = GameManager.instance.globeScriptable.GetGlobeDescription();
        currentGlobeValue = GameManager.instance.globeScriptable.GetGlobeValue();

        RestNetworkManager.instance.CallWriteMemory();
    }
    public void UpdateSpinConfig(string _spinNumber, TicketInfos _ticketSpin)
    {
        ticketSpin = _ticketSpin;
        if (!spinNumbers.Contains(_spinNumber))
            spinNumbers.Add(_spinNumber);
        spinIndex = spinNumbers.Count;
        RestNetworkManager.instance.CallWriteMemory();

    }
    public void PopulateConfig()
    {
        GameManager.instance.sceneId = currentSceneID;
        GameManager.instance.globeScriptable.SetGlobeOrder(currentRaffle);
        GameManager.instance.globeScriptable.SetGlobeDesc(currentGlobeDesc);
        GameManager.instance.globeScriptable.SetGlobeValue(currentGlobeValue);
        GameManager.instance.isVisibleRaffle = isVisibleRaffle;
        GameManager.instance.globeDraw.porDuasBolas = forTwoBalls;
        //GameManager.instance.globeDraw.porUmaBolas.Clear();

        GameManager.instance.globeDraw.IncreseForOneBalls(forOneBalls);

        GameManager.instance.globeDraw.ganhadorContemplado = new TicketInfos[ticketInfos.Count];
        GameManager.instance.globeDraw.ticketListVisible = new bool[ticketInfos.Count];
        GameManager.instance.isTicketVisible = isTicketVisible;
        GameManager.instance.ticketWinnerIndex = currentTicketIndex;


        if (currentSceneID == 2)
        {
            for (int i = 0; i < ticketInfos.Count; i++)
            {
                GameManager.instance.globeDraw.ganhadorContemplado[i] = ticketInfos[i];
                GameManager.instance.globeDraw.ticketListVisible[i] = ticketsShown[i];
            }
            GameManager.instance.RecoveryGlobeScreen();
        }

        if (currentSceneID == 3)
        {
            GameManager.instance.spinScriptable.sorteioOrdem = spinIndex;
            GameManager.instance.spinResultScriptable.ganhadorContemplado = ticketSpin;
            GameManager.instance.RecoverySpinScreen();
        }

    }


}

