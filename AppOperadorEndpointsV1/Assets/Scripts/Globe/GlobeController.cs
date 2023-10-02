using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RiptideNetworking;
using System.Collections;
using System.Linq;

public class GlobeController : MonoBehaviour
{
    [SerializeField] public Button btNextRaffle;
    [Header("TICKET INFOS")]
    [SerializeField] private TicketController ticketController;
    [SerializeField] public Button btTicketVisibility;
    [Header("GRID BALLS")]
    [SerializeField] private List<SelectBall> balls;
    [SerializeField] private GameObject panelGridBalls;
    [SerializeField] private int indexBallSelected;

    [Header("GRID RAFFLED BALLS")]
    [SerializeField] private SelectBall[] ballsRaffled;
    [SerializeField] private SelectBall ballRaffledPrefab;
    [SerializeField] private GameObject panelGridBallsRaffled;
    [SerializeField] private SelectBall lastBallRaffled;
    [SerializeField] private List<int> indexBalls;


    [Header("STATUS INFOS")]
    [SerializeField] private TextMeshProUGUI txtInfosTitle;
    [SerializeField] private TextMeshProUGUI txtForOneBall;
    [SerializeField] private TextMeshProUGUI txtWinners;
    [SerializeField] private TextMeshProUGUI txtForTwoBalls;
    [SerializeField] private TicketsListController ticketsListController;

    [Header("COMPONENTS")]
    [SerializeField] private TextMeshProUGUI orderBalls;
    [Header("CONFIRM BALL")]
    [SerializeField] private Button btConfirm;
    [SerializeField] private GameObject panelConfirmBall;
    [SerializeField] private TextMeshProUGUI txtViewNumberBallConfirm;
    [Header("REVOKE BALL")]
    [SerializeField] private Button btRevoke;
    [SerializeField] private GameObject panelRevokeBall;
    [SerializeField] private TextMeshProUGUI txtViewNumberBallRevoke;

    [SerializeField] private bool hasRevoked = false;


    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        balls = new List<SelectBall>();
        balls.AddRange(panelGridBalls.GetComponentsInChildren<SelectBall>());
        ticketController = FindObjectOfType<TicketController>();
        PopulateBalls();
        btNextRaffle.interactable = false;
        UpdateScreen();
        UpdateStateVisibilityButtonsTicket(false);

        if (GameManager.instance.isBackup)
        {
            SetDisableAll();
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
    #region BALLS 
    private void PopulateBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            int number = i + 1;
            balls[i].InitializeVariables();
            balls[i].SetNumberInText(number.ToString());
        }
    }
    public void SetTxtViewBall(string _number)
    {
        if (panelConfirmBall.activeSelf)
        {
            txtViewNumberBallConfirm.text = _number;
        }
        if (panelRevokeBall.activeSelf)
        {
            txtViewNumberBallRevoke.text = _number;
        }
    }
    private void CheckStateBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].CheckState();
        }

        if (GameManager.instance.globeDrawData.ganhadorContemplado.Length > 0)
        {
            SetDisableAllNotRevoke();
        }
    }
    public void DisableHasRevokedAll()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].SetHasCanRevoked(false);
            balls[i].SetHasSelected(false);
            balls[i].SetHasRaffled(false);
        }
    }
    public void DisableAllConfirmed()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].SetHasRaffled(false);
        }
    }
    private void SetDisableAllNotRevoke()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (!balls[i].GetHasCanRevoked())
            {
                balls[i].DisableInteractable();
            }
        }
    }
    public void SetDisableAll()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].DisableInteractable();
        }
    }
    public void SetEnableAll()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].EnableInteractable();
        }
        CheckStateBalls();
    }
    public void OpenPanelBall(int index)
    {
        indexBallSelected = index - 1;

        if (balls[indexBallSelected].GetHasRaffled() == true)
        {
            panelRevokeBall.SetActive(true);
        }
        else
        {
            balls[indexBallSelected].SetHasSelected(true);
            if (!GameManager.instance.isWinner)
            {
                panelConfirmBall.SetActive(true);
            }
        }
        SetTxtViewBall(balls[indexBallSelected].GetNumberBall());
        CheckStateBalls();
    }
    public void ConfirmBallSelected()
    {
        GameManager.instance.SetNewBall(balls[indexBallSelected].GetNumberBall());
        hasRevoked = false;

    }
    public void RevokeBallSelected()
    {
        indexBalls.Remove(indexBalls[indexBalls.Count - 1]);
        GameManager.instance.SetRemoveBall(balls[indexBallSelected].GetNumberBall());
        lastBallRaffled.SetHasSelected(true);
        lastBallRaffled.SetHasRaffled(false);
        lastBallRaffled.SetHasCanRevoked(false);

        hasRevoked = true;

    }
    public void ClosePanelConfirmBall()
    {
        panelConfirmBall.SetActive(false);
        balls[indexBallSelected].SetHasSelected(false);
        CheckStateBalls();
    }
    public void ClosePanelRevokeBall()
    {
        panelRevokeBall.SetActive(false);
        balls[indexBallSelected].SetHasSelected(false);
        CheckStateBalls();
    }
    #endregion

    public void UpdateScreen()
    {
        DisableHasRevokedAll();
        indexBalls.Clear();
        for (int i = 0; i < GameManager.instance.globeDrawData.bolasSorteadas.Count; i++)
        {
            indexBalls.Add(int.Parse(GameManager.instance.globeDrawData.bolasSorteadas[i]) - 1);

            balls[int.Parse(GameManager.instance.globeDrawData.bolasSorteadas[i]) - 1].SetHasRaffled(true);
        }
        CheckStateBalls();
        ValidateBall();
        CheckBtNextRaffle();
        if (GameManager.instance.isWinner == true)
            ticketsListController.CheckWinnerButtonState(GameManager.instance.operatorData.ticketsShown, GameManager.instance.operatorData.currentTicketIndex);
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
    #region BALLS RAFFLED

    private void SpawnBallRaffled(List<string> _balls)
    {
        foreach (var item in ballsRaffled)
        {
            Destroy(item.gameObject);
        }
        ballsRaffled = new SelectBall[_balls.Count];
        for (int i = 0; i < _balls.Count; i++)
        {
            SelectBall inst = Instantiate(ballRaffledPrefab, transform.position, Quaternion.identity);
            inst.transform.SetParent(panelGridBallsRaffled.transform);
            inst.SetNumberInText(_balls[i]);
            inst.transform.localScale = new Vector3(1, 1, 1);
            if (i == _balls.Count - 1)
            {
                inst.SetCanRevokebleColor();
            }
            else
            {
                inst.SetConfirmedColor();
            }
            ballsRaffled[i] = inst;
        }
        orderBalls.text = $"Dezenas Sorteadas: {ballsRaffled.Length}";
    }
    #endregion
   
    public void CheckWinners()
    {
        ticketsListController.ResetGrid();

        if (GameManager.instance.globeDrawData.ganhadorContemplado.Length > 0)
        {
            txtInfosTitle.text = "GANHADORES";
            GameManager.instance.isWinner = true;
            ticketsListController.PopulateListTickets(GameManager.instance.GetWinners(), GameManager.instance.isWinner);
            ticketsListController.btTickets[0].SelectWinner();
        }
        else
        {
            if (GameManager.instance.GetForOneBalls().Count > 0)
            {
                txtInfosTitle.text = "POR UMA BOLA";
            }
            else
            {
                txtInfosTitle.text = "INFORMAÇÕES";
            }
            GameManager.instance.isWinner = false;
            ticketsListController.PopulateListTickets(GameManager.instance.GetForOneBalls(), GameManager.instance.isWinner);
        }
        txtForOneBall.text = GameManager.instance.GetForOneBalls().Count.ToString();
        txtForTwoBalls.text = GameManager.instance.GetForTwoBalls();
        txtWinners.text = GameManager.instance.GetWinners().Count.ToString();
        ticketsListController.SetInteractableBtTicketsList(GameManager.instance.isWinner);
        UpdateStateVisibilityButtonsTicket(GameManager.instance.isWinner);
        GameManager.instance.WriteInfosGlobe();
    }

    public void ValidateBall()
    {
        if (indexBalls.Count > 0)
        {
            lastBallRaffled = balls[indexBalls[indexBalls.Count - 1]];
            lastBallRaffled.SetHasCanRevoked(true);
        }

        CheckStateBalls();

        SpawnBallRaffled(GameManager.instance.GetBallsRaffled());
        CheckWinners();

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
        DisableHasRevokedAll();
        ticketsListController.ResetGrid();
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
        RestNetworkManager.instance.SendBallsRaffledFromServer();
    }


    public void ShowTicketGlobe()
    {
        foreach (var item in ticketsListController.btTickets)
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            if (panelConfirmBall.activeSelf)
            {
                ConfirmBallSelected();
                panelConfirmBall.SetActive(false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (panelConfirmBall.activeSelf)
            {
                ClosePanelConfirmBall();
            }
        }
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

