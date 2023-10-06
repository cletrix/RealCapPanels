using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RiptideNetworking;
using System.Collections;
using System.Linq;

public class GlobeManager : MonoBehaviour
{
    [SerializeField] public Button btNextRaffle;
    [Header("TICKET INFOS")]
    [SerializeField] private TicketController ticketController;
    [SerializeField] public Button btTicketVisibility;

    [Header("COMPONENTS")]
    [SerializeField] private DrawGlobeInfos drawGlobeInfos;
    [SerializeField] public SelectBallsController selectBallsController;

    
    [SerializeField] public static bool hasRevoked = false;

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        selectBallsController.StartVariables();
        ticketController = FindObjectOfType<TicketController>();
        btNextRaffle.interactable = false;
        UpdateScreen();
        UpdateStateVisibilityButtonsTicket(false);

        if (GameManager.instance.isBackup)
        {
            selectBallsController.SetDisableAll();
        }
        GameManager.instance.RecoveryGlobeScreen();
    }
    public void UpdateStateVisibilityButtonsTicket(bool isActive)
    {
        if (GameManager.instance.isBackup)
        {
            btTicketVisibility.interactable = false;
        }
        else
        {
            btTicketVisibility.interactable = isActive;
        }
    }

    public void UpdateScreen()
    {
        selectBallsController.DisableHasRevokedAll();
        selectBallsController.indexBalls.Clear();
        for (int i = 0; i < GameManager.instance.globeDrawData.bolasSorteadas.Count; i++)
        {
            selectBallsController.indexBalls.Add(int.Parse(GameManager.instance.globeDrawData.bolasSorteadas[i]) - 1);

            selectBallsController.balls[int.Parse(GameManager.instance.globeDrawData.bolasSorteadas[i]) - 1].SetHasRaffled(true);
        }
        selectBallsController.CheckStateBalls();
        selectBallsController.ValidateBall();
        drawGlobeInfos.CheckWinners();
        UpdateStateVisibilityButtonsTicket(GameManager.instance.isWinner);
        CheckBtNextRaffle();
        if (GameManager.instance.isWinner == true)
            drawGlobeInfos.ticketsList.CheckWinnerButtonState(GameManager.instance.operatorData.ticketsShown, GameManager.instance.operatorData.currentTicketIndex);
    }
    public void SendBallsRaffledToScreen()
    {
        UpdateScreen();
        if (hasRevoked)
        {
            SendMessageBallRevoked();
        }
        else
        {
            SendMessageBallsRaffled();
        }
    }

   

  
    public void CallNextRaffle()
    {
        StartCoroutine(NextRaffle());
    }
    private IEnumerator NextRaffle()
    {
        UIChangeRaffleType uIChangeRaffle = FindObjectOfType<UIChangeRaffleType>();
        UiInfosRaffle uiInfos = FindObjectOfType<UiInfosRaffle>();
        GameManager.instance.globeData.SetOrder(GameManager.instance.globeData.GetOrder() + 1);
        GameManager.instance.ResetScreenGlobe();
        yield return new WaitForSeconds(1);
        SendMesageNextRaffle();
        selectBallsController.DisableHasRevokedAll();
        drawGlobeInfos.ticketsList.ResetGrid();
        RestNetworkManager.instance.GetGlobeInfosDrawn();
        yield return new WaitForSeconds(0.2f);
        UpdateScreen();
        uIChangeRaffle.SendMessageGlobeInfos(
           GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].nome,
           GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].numero,
           GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
           GameManager.instance.globeData.GetOrder(),
           GameManager.instance.globeData.GetDescription(),
           GameManager.instance.globeData.GetValue());

        CheckBtNextRaffle();
        RestNetworkManager.instance.PostBallsRaffled();
    }


    public void ShowTicketGlobe()
    {
        foreach (var item in drawGlobeInfos.ticketsList.btTickets)
        {
            if (item.GetIsSelected() == true)
            {
                GameManager.instance.ticketWinnerIndex = item.GetIndex();
                GameManager.instance.SetIsVisibleTicketList(item.GetIndex());

                item.SetIsFinished(true);
            }
        }
        CheckBtNextRaffle();
        PopulateTicketGlobe();
        ticketController.SetTicketVisibility();
    }

    private void CheckBtNextRaffle()
    {
        if (GameManager.instance.globeData.GetOrder() < GameManager.instance.recoveryData.limit_globo && GameManager.instance.isWinner == true)
            btNextRaffle.interactable = GameManager.instance.GetAllTicketsVisible();
        else
            btNextRaffle.interactable = false;
    }
    public void PopulateTicketGlobe()
    {
        ticketController.PopulateTicketInfos(
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].nome,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].cpf,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].dataNascimento,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].telefone,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].email,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].bairro,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].municipio,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].estado,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].dataSorteio,
           GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].numero,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].valor,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].PDV,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].bairoPDV,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].dataCompra,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].horaCompra,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].numeroTitulo,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].chance,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].numeroCartela,
           GameManager.instance.globeDrawData.ganhadorContemplado[GameManager.instance.ticketWinnerIndex].numeroSorte,
           true,
           2);
    }
    #region MESSAGES
    public void SendMessageBallsRaffled()
    {
        if (!GameManager.instance.isBackup)
            TcpNetworkManager.instance.Server.SendToAll(GetMessageString(Message.Create(MessageSendMode.reliable, ServerToClientId.messageBall), GameManager.instance.GetBallsRaffled().ToArray(), GameManager.instance.globeDrawData.porUmaBolas.Count, GameManager.instance.globeDrawData.ganhadorContemplado.Length, GameManager.instance.globeDrawData.valorPremio));
    }
    public void SendMessageBallRevoked()
    {
        if (!GameManager.instance.isBackup)
            TcpNetworkManager.instance.Server.SendToAll(GetMessageBallRevoked(Message.Create(MessageSendMode.reliable, ServerToClientId.messageBallRevoked), GameManager.instance.GetBallsRaffled().ToArray(), GameManager.instance.globeDrawData.porUmaBolas.Count, GameManager.instance.globeDrawData.ganhadorContemplado.Length, GameManager.instance.globeDrawData.valorPremio));
    }

    public void SendMesageNextRaffle()
    {
        if (!GameManager.instance.isBackup)
            TcpNetworkManager.instance.Server.SendToAll(GetMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.messageNextRaffleGlobe)));
    }

    private Message GetMessageString(Message message, string[] _ballsRaffled, int _forOneBall, int _winnersCount, float _prizeValue)
    {
        message.AddStrings(_ballsRaffled);
        message.AddInt(_forOneBall);
        message.AddInt(_winnersCount);
        message.AddFloat(_prizeValue);
        return message;
    }

    private Message GetMessageBallRevoked(Message message, string[] _ballsRaffled, int _forOneBall, int _winnersCount, float _prizeValue)
    {
        message.AddStrings(_ballsRaffled);
        message.AddInt(_forOneBall);
        message.AddInt(_winnersCount);
        message.AddFloat(_prizeValue);

        return message;
    }
    private Message GetMessage(Message message)
    {
        return message;
    }

    #endregion
}

