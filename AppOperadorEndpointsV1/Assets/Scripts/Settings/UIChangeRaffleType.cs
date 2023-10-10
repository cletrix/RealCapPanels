using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RiptideNetworking;

public class UIChangeRaffleType : MonoBehaviour
{
    [Header("CONTROLLERS")]
    [SerializeField] GlobeManager globeManager;
    [SerializeField] SpinController spinController;

    [Header("GERAL")]
    [SerializeField] private Button btRecovery;
    [SerializeField] private UiInfosRaffle infosRaffle;
    [Header("RAFFLES PANELS")]
    public GameObject panelGlobeDraw;
    public GameObject panelSpinDraw;

    [Header("BUTTONS RAFFLES")]
    [SerializeField] private Button btLotteryDraw;
    [SerializeField] private Button btGlobeDraw;
    [SerializeField] private Button bSpinDraw;

    [Header("BUTTON COLORS")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;

    [Header("HIDE RAFFLE SYSTEM")]
    [SerializeField] private Button btVisibilityRaffle;

    [SerializeField] private bool hasActiveGlobe = true;
    [SerializeField] private bool hasActiveSpin = true;

    void Start()
    {
        InitializeVariables();
    }
    private void InitializeVariables()
    {
        SetModality();
        SetButtonsEvent();
        GameManager.instance.isVisibleRaffle = GameManager.instance.operatorData.isVisibleRaffle;
        GameManager.instance.RecoveryGlobeScreen();
        SetStateSelectBackupButton();
        CheckStateVisibilityRaffle();
        RestNetworkManager.instance.PostWriteMemory();
    }

    private void SetModality()
    {
        switch (GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].modalidades)
        {
            case 1:
                {
                    hasActiveGlobe = false;
                    hasActiveSpin = false;
                    break;
                }
            case 2:
                {
                    hasActiveGlobe = true;
                    hasActiveSpin = true;
                    SetGlobeDraw();
                    break;
                }
            case 3:
                {
                    hasActiveGlobe = true;
                    hasActiveSpin = true;
                    SetGlobeDraw();
                    break;
                }
            case 4:
                {
                    hasActiveGlobe = false;
                    hasActiveSpin = true;
                    SetSpinDraw();
                    break;
                }
        }
        btGlobeDraw.interactable = hasActiveGlobe;
        bSpinDraw.interactable = hasActiveSpin;
    }
    private void SetButtonsEvent()
    {
        btLotteryDraw.onClick.AddListener(GameManager.instance.WriteInfosGlobe);
        btGlobeDraw.onClick.AddListener(SetGlobeDraw);
        btGlobeDraw.onClick.AddListener(GameManager.instance.WriteInfosGlobe);
        bSpinDraw.onClick.AddListener(SetSpinDraw);
        bSpinDraw.onClick.AddListener(GameManager.instance.WriteInfosGlobe);
        btVisibilityRaffle.onClick.AddListener(SetStateHasVisibilityDraw);
        btVisibilityRaffle.onClick.AddListener(SendInfosDraw);
        btVisibilityRaffle.onClick.AddListener(GameManager.instance.WriteInfosGlobe);
    }

    #region RAFFLES PANELS

    private void ResetColorButtons(Color newColor)
    {
        btLotteryDraw.image.color = newColor;
        btGlobeDraw.image.color = newColor;
        bSpinDraw.image.color = newColor;
    }


    public void SetGlobeDraw()
    {
        SelectPanelForActivate(1);
        SendMessageToClientChangeRaffle("SceneGlobe", GameManager.instance.GetCountBallsGrid(), GameManager.instance.GetCountBallsCard());

    }
    public void SetSpinDraw()
    {
        SelectPanelForActivate(2);
        SendMessageToClientChangeRaffle("SceneSpin", GameManager.instance.GetCountBallsGrid(), GameManager.instance.GetCountBallsCard());
    }

    public void SelectPanelForActivate(int index)
    {
        switch (index)
        {
            case 1:
                {
                    ResetColorButtons(normalColor);
                    panelGlobeDraw.SetActive(true);
                    panelSpinDraw.SetActive(false);
                    btGlobeDraw.image.color = selectedColor;
                    infosRaffle.PopulateRaffleInfos(GameManager.instance.globeData.GetOrder().ToString(), GameManager.instance.globeData.GetDescription(), GameManager.instance.globeData.GetValue());
                    break;
                }
            case 2:
                {
                    ResetColorButtons(normalColor);
                    panelGlobeDraw.SetActive(false);
                    panelSpinDraw.SetActive(true);
                    bSpinDraw.image.color = selectedColor;
                    infosRaffle.PopulateRaffleInfos(GameManager.instance.spinData.sorteioOrdem.ToString(), GameManager.instance.spinData.sorteioDescricao, GameManager.instance.spinData.sorteioValor);
                    break;
                }
            default:
                {
                    ResetColorButtons(normalColor);
                    panelGlobeDraw.SetActive(false);
                    panelSpinDraw.SetActive(false);
                    break;
                }
        }
        GameManager.instance.sceneId = index;
        GameManager.instance.operatorData.panelActive = index;
        //RestNetworkManager.instance.PostWriteMemory();
    }
    public void RaffleTypeScene(int type)
    {
        switch (type)
        {
            case 1:
                {
                    SetGlobeDraw();
                    break;
                }
            case 2:
                {
                    SetSpinDraw();
                    break;
                }
        }
    }

    #endregion

    #region RECOVERY

    public void SetRecoveryConfig()
    {
        if (GameManager.instance.isBackup)
        {
            GameManager.instance.isBackup = false;
        }
        else
        {
            GameManager.instance.isBackup = true;
            RestNetworkManager.instance.GetRecoveryInfosDrawn();
        }
        SetStateSelectBackupButton();
    }
    private void SetStateSelectBackupButton()
    {
        if (GameManager.instance.isBackup)
        {
            btRecovery.GetComponentInChildren<TextMeshProUGUI>().text = "Backup";
            btRecovery.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            btRecovery.image.color = Color.red;
            SetStateButtonsRaffle(false);
            globeManager.UpdateStateVisibilityButtonsTicket(false);
            globeManager.drawGlobeInfos.ticketsList.SetInteractableBtTicketsList(false);
        }
        else
        {
            btRecovery.GetComponentInChildren<TextMeshProUGUI>().text = "Principal";
            btRecovery.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            btRecovery.image.color = Color.green;
            SetStateButtonsRaffle(true);
            if (panelGlobeDraw.activeSelf == true)
                globeManager.UpdateScreen();
        }
        CheckStateVisibilityRaffle();
    }
    #endregion

    #region VISIBILITY RAFFLE

    private void SetStateButtonsRaffle(bool isActive)
    {
        if (GameManager.instance.isBackup)
        {
            btLotteryDraw.interactable = false;
            btGlobeDraw.interactable = false;
            bSpinDraw.interactable = false;
            btVisibilityRaffle.interactable = false;
        }
        else
        {
            btVisibilityRaffle.interactable = true;

            if (hasActiveGlobe)
            {
                btGlobeDraw.interactable = isActive;
            }
            else
            {
                btGlobeDraw.interactable = false;
            }

            if (hasActiveSpin)
            {
                bSpinDraw.interactable = isActive;
            }
            else
            {
                bSpinDraw.interactable = false;
            }
        }
    }
    public void SetStateHasVisibilityDraw()
    {
        if (GameManager.instance.isVisibleRaffle)
        {
            GameManager.instance.isVisibleRaffle = false;
        }
        else
        {
            GameManager.instance.isVisibleRaffle = true;
        }
        Invoke("CheckStateVisibilityRaffle", 0.1f);
    }
    public void CheckStateVisibilityRaffle()
    {
        if (GameManager.instance.isVisibleRaffle)
        {
            btVisibilityRaffle.GetComponentInChildren<TextMeshProUGUI>().text = "OCULTAR SORTEIO";
            btVisibilityRaffle.image.color = selectedColor;
            SetStateButtonsRaffle(false);
            globeManager.selectBallsController.SetEnableAll();
            spinController.SetActiveBtGenerateSpin(true);
            globeManager.UpdateStateVisibilityButtonsTicket(GameManager.instance.isWinner);
            globeManager.drawGlobeInfos.ticketsList.SetInteractableBtTicketsList(GameManager.instance.isWinner);

        }
        else
        {
            btVisibilityRaffle.GetComponentInChildren<TextMeshProUGUI>().text = "MOSTRAR SORTEIO";
            btVisibilityRaffle.image.color = normalColor;
            SetStateButtonsRaffle(true);
            globeManager.selectBallsController.SetDisableAll();

            spinController.SetActiveBtGenerateSpin(false);
            globeManager.UpdateStateVisibilityButtonsTicket(false);
            globeManager.drawGlobeInfos.ticketsList.SetInteractableBtTicketsList(false);


        }
    }
    #endregion

    #region MESSAGES
    public void SendMessageVisibilityRaffle()
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessageVisibilityRaffle(Message.Create(MessageSendMode.unreliable, ServerToClientId.messageVisibilityRaffle), GameManager.instance.isVisibleRaffle, GameManager.instance.sceneId));
    }
    public void SendInfosDraw()
    {
        if (panelGlobeDraw.activeSelf == true)
        {
            SendMessageGlobeInfos(
       GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].nome,
       GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].numero,
       GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
       GameManager.instance.globeData.GetOrder(),
       GameManager.instance.globeData.GetDescription(),
       GameManager.instance.globeData.GetValue());
        }
        else if (panelSpinDraw.activeSelf == true)
        {
            SendMessageSpinInfos(
       GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].nome,
       GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].numero,
       GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
       GameManager.instance.spinData.sorteioOrdem,
       GameManager.instance.spinData.sorteioDescricao,
       GameManager.instance.spinData.sorteioValor);
        }
        SendMessageVisibilityRaffle();
    }
    private Message GetMessageVisibilityRaffle(Message message, bool isActive, int typeRaffle)
    {
        message.AddBool(isActive);
        message.AddInt(typeRaffle);
        return message;
    }
    public void SendMessageToClientGetActiveScene()
    {
        if (!GameManager.instance.isBackup)
        {
            TcpNetworkManager.instance.Server.SendToAll(GetMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.messageCheckSceneActive)));
        }
    }
    public void SendMessageToClientChangeRaffle(string _messageString, int maxBalls,int gridNumbersTicket)
    {
        if (!GameManager.instance.isBackup)
        {
            TcpNetworkManager.instance.Server.SendToAll(GetMessageTypeRaffle(Message.Create(MessageSendMode.reliable, ServerToClientId.messageTypeRaffle), _messageString, maxBalls, gridNumbersTicket));
        }
    }
    public void SendMessageGlobeInfos(string _editionName, string _editionNumber, string _date, int _ordem, string _description, float _value)
    {
        if (!GameManager.instance.isBackup)
            TcpNetworkManager.instance.Server.SendToAll(GetMessageGlobeInfos(Message.Create(MessageSendMode.reliable, ServerToClientId.messageInfosGlobe), _editionName, _editionNumber, _date, _ordem, _description, _value));
    }
    public void SendMessageSpinInfos(string _editionName, string _editionNumber, string _date, int _ordem, string _description, float _value)
    {
        if (!GameManager.instance.isBackup)
            TcpNetworkManager.instance.Server.SendToAll(GetMessageSpinInfos(Message.Create(MessageSendMode.reliable, ServerToClientId.messageInfosSpin), _editionName, _editionNumber, _date, _ordem, _description, _value));
    }
    //private Message GetMessageString(Message message, string _textMessage)
    //{
    //    message.AddString(_textMessage);

    //    return message;
    //}
    private Message GetMessageTypeRaffle(Message message, string _textMessage, int maxBalls,int gridNumbersTicket)
    {
        message.AddString(_textMessage);
        message.Add(maxBalls);
        message.Add(gridNumbersTicket);
        return message;
    }
    private Message GetMessage(Message message)
    {
        return message;
    }

    private Message GetMessageGlobeInfos(Message message, string _editionName, string _editionNumber, string _date, int _ordem, string _description, float _value)
    {
        message.AddString(_editionName);
        message.AddString(_editionNumber);
        message.AddString(_date);
        message.AddInt(_ordem);
        message.AddString(_description);
        message.AddFloat(_value);

        return message;
    }

    private Message GetMessageSpinInfos(Message message, string _editionName, string _editionNumber, string _date, int _ordem, string _description, float _value)
    {
        message.AddString(_editionName);
        message.AddString(_editionNumber);
        message.AddString(_date);
        message.AddInt(_ordem);
        message.AddString(_description);
        message.AddFloat(_value);

        return message;
    }
    #endregion
}
