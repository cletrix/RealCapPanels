using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RiptideNetworking;

public class UIChangeRaffleType : MonoBehaviour
{
    [Header("CONTROLLERS")]
    [SerializeField] GlobeController globeController;

    [Header("GERAL")]
    [SerializeField] private Button btRecovery;
    [SerializeField] private UiInfosRaffle infosRaffle;
    [Header("RAFFLES PANELS")]
    [SerializeField] private GameObject panelRaffleLottery;
    public GameObject panelRaffleGlobe;
    [SerializeField] private GameObject panelRaffleSpin;

    [Header("BUTTONS RAFFLES")]
    [SerializeField] private Button btRaffleLottery;
    [SerializeField] private Button btRaffleGlobe;
    [SerializeField] private Button btRaffleSpin;

    [Header("BUTTON COLORS")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;

    [Header("HIDE RAFFLE SYSTEM")]
    [SerializeField] private Button btVisibilityRaffle;

    [SerializeField] private bool hasActiveLottery = true;
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
        GameManager.instance.isVisibleRaffle = GameManager.instance.technicalScriptable.isVisibleRaffle;
        GameManager.instance.RecoveryGlobeScreen();
        SetStateSelectBackupButton();
        CheckStateVisibilityRaffle();
        RestNetworkManager.instance.CallWriteMemory();
    }

    private void SetModality()
    {
        switch (GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].modalidades)
        {
            case 1:
                {
                    hasActiveLottery = true;
                    hasActiveGlobe = false;
                    hasActiveSpin = false;
                    SetRaffleLottery();
                    break;
                }
            case 2:
                {
                    hasActiveLottery = true;
                    hasActiveGlobe = true;
                    hasActiveSpin = true;
                    SetRaffleGlobe();
                    break;
                }
            case 3:
                {
                    hasActiveLottery = false;
                    hasActiveGlobe = true;
                    hasActiveSpin = true;
                    SetRaffleGlobe();
                    break;
                }
        }
        btRaffleLottery.interactable = hasActiveLottery;
        btRaffleGlobe.interactable = hasActiveGlobe;
        btRaffleSpin.interactable = hasActiveSpin;
    }
    private void SetButtonsEvent()
    {
        btRaffleLottery.onClick.AddListener(SetRaffleLottery);
        btRaffleLottery.onClick.AddListener(GameManager.instance.WriteInfosGlobe);
        btRaffleGlobe.onClick.AddListener(SetRaffleGlobe);
        btRaffleGlobe.onClick.AddListener(GameManager.instance.WriteInfosGlobe);
        btRaffleSpin.onClick.AddListener(SetRaffleSpin);
        btRaffleSpin.onClick.AddListener(GameManager.instance.WriteInfosGlobe);
        btVisibilityRaffle.onClick.AddListener(SetStateHasRaffleVisibility);
        btVisibilityRaffle.onClick.AddListener(SendInfosRaffle);
        btVisibilityRaffle.onClick.AddListener(GameManager.instance.WriteInfosGlobe);
    }

    #region RAFFLES PANELS

    private void ResetColorButtons(Color newColor)
    {
        btRaffleLottery.image.color = newColor;
        btRaffleGlobe.image.color = newColor;
        btRaffleSpin.image.color = newColor;
    }
    public void SetRaffleLottery()
    {
        SelectPanelForActivate(1);
        LotteryController lotteryController = FindObjectOfType<LotteryController>();
        lotteryController.ResetNumberRaffle();
        SendMessageToClientChangeRaffle("SceneLottery");
    }

    public void SetRaffleGlobe()
    {
        SelectPanelForActivate(2);
        SendMessageToClientChangeRaffle("SceneGlobe");

    }
    public void SetRaffleSpin()
    {
        SelectPanelForActivate(3);
        SendMessageToClientChangeRaffle("SceneSpin");
    }

    public void SelectPanelForActivate(int index)
    {
        switch (index)
        {
            case 1:
                {
                    panelRaffleLottery.SetActive(true);
                    ResetColorButtons(normalColor);
                    panelRaffleGlobe.SetActive(false);
                    panelRaffleSpin.SetActive(false);
                    btRaffleLottery.image.color = selectedColor;
                    infosRaffle.PopulateRaffleInfos(GameManager.instance.lotteryScriptable.sorteioOrdem.ToString(), GameManager.instance.lotteryScriptable.sorteioDescricao, GameManager.instance.lotteryScriptable.sorteioValor);

                    break;
                }
            case 2:
                {
                    ResetColorButtons(normalColor);
                    panelRaffleLottery.SetActive(false);
                    panelRaffleGlobe.SetActive(true);
                    panelRaffleSpin.SetActive(false);
                    btRaffleGlobe.image.color = selectedColor;
                    infosRaffle.PopulateRaffleInfos(GameManager.instance.globeScriptable.GetGlobeOrder().ToString(), GameManager.instance.globeScriptable.GetGlobeDescription(), GameManager.instance.globeScriptable.GetGlobeValue());
                    break;
                }
            case 3:
                {
                    ResetColorButtons(normalColor);
                    panelRaffleLottery.SetActive(false);
                    panelRaffleGlobe.SetActive(false);
                    panelRaffleSpin.SetActive(true);
                    btRaffleSpin.image.color = selectedColor;
                    infosRaffle.PopulateRaffleInfos(GameManager.instance.spinScriptable.sorteioOrdem.ToString(), GameManager.instance.spinScriptable.sorteioDescricao, GameManager.instance.spinScriptable.sorteioValor);
                    SpinController spinController = FindObjectOfType<SpinController>();
                    spinController.ActiveButtonNewRaffleSpin();
                    break;
                }
            default:
                {
                    ResetColorButtons(normalColor);
                    panelRaffleLottery.SetActive(false);
                    panelRaffleGlobe.SetActive(false);
                    panelRaffleSpin.SetActive(false);
                    break;
                }
        }
        GameManager.instance.sceneId = index;
        GameManager.instance.technicalScriptable.panelActive = index;
    }
    public void RaffleTypeScene(int type)
    {
        switch (type)
        {
            case 1:
                {
                    SetRaffleLottery();
                    break;
                }
            case 2:
                {
                    SetRaffleGlobe();
                    break;
                }
            case 3:
                {
                    SetRaffleSpin();
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
            RestNetworkManager.instance.DisableInvokInfosServer();
            GameManager.instance.isBackup = false;
        }
        else
        {
            RestNetworkManager.instance.CallGetInfoServer();
            GameManager.instance.isBackup = true;
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
        }
        else
        {
            btRecovery.GetComponentInChildren<TextMeshProUGUI>().text = "Principal";
            btRecovery.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            btRecovery.image.color = Color.green;
            globeController.UpdateScreen();

            SpinController spinController = FindObjectOfType<SpinController>();
            if (spinController != null)
            {
                //spinController.ActiveLastSpin();
                spinController.ActiveButtonNewRaffleSpin();
            }
        }
    }
    #endregion

    #region VISIBILITY RAFFLE

    private void SetStateButtonsRaffle(bool isActive)
    {
        if (GameManager.instance.isBackup)
        {
            btRaffleLottery.interactable = false;
            btRaffleGlobe.interactable = false;
            btRaffleSpin.interactable = false;
        }
        else
        {
            if (hasActiveLottery)
            {
                btRaffleLottery.interactable = isActive;
            }
            else
            {
                btRaffleLottery.interactable = false;
            }

            if (hasActiveGlobe)
            {
                btRaffleGlobe.interactable = isActive;
            }
            else
            {
                btRaffleGlobe.interactable = false;
            }

            if (hasActiveSpin)
            {
                btRaffleSpin.interactable = isActive;
            }
            else
            {
                btRaffleSpin.interactable = false;
            }
        }
    }
    public void SetStateHasRaffleVisibility()
    {
        if (GameManager.instance.isVisibleRaffle)
        {
            GameManager.instance.isVisibleRaffle = false;
        }
        else
        {
            GameManager.instance.isVisibleRaffle = true;
        }
        CheckStateVisibilityRaffle();
    }
    public void CheckStateVisibilityRaffle()
    {
        if (GameManager.instance.isVisibleRaffle)
        {
            btVisibilityRaffle.GetComponentInChildren<TextMeshProUGUI>().text = "OCULTAR SORTEIO";
            btVisibilityRaffle.image.color = selectedColor;
            SetStateButtonsRaffle(false);
            globeController.SetEnableAll();
        }
        else
        {
            btVisibilityRaffle.GetComponentInChildren<TextMeshProUGUI>().text = "MOSTRAR SORTEIO";
            btVisibilityRaffle.image.color = normalColor;
            SetStateButtonsRaffle(true);
            globeController.SetDisableAll();
        }
    }
    #endregion

    #region MESSAGES
    public void SendMessageVisibilityRaffle()
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessageVisibilityRaffle(Message.Create(MessageSendMode.unreliable, ServerToClientId.messageVisibilityRaffle), GameManager.instance.isVisibleRaffle, GameManager.instance.sceneId));

    }
    public void SendInfosRaffle()
    {

        if (panelRaffleLottery.activeSelf == true)
        {
            SendMessageLotteryInfos(
       GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].numero,
       GameManager.instance.lotteryScriptable.resultadoLoteriaFederalNumeroConcurso,
       GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
       GameManager.instance.lotteryScriptable.resultadoLoteriaFederalDataConcurso,
       GameManager.instance.lotteryScriptable.sorteioOrdem,
       GameManager.instance.lotteryScriptable.sorteioDescricao,
       GameManager.instance.lotteryScriptable.sorteioValor);
        }
        else if (panelRaffleGlobe.activeSelf == true)
        {
            SendMessageGlobeInfos(
       GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].nome,
       GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].numero,
       GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
       GameManager.instance.globeScriptable.GetGlobeOrder(),
       GameManager.instance.globeScriptable.GetGlobeDescription(),
       GameManager.instance.globeScriptable.GetGlobeValue());
        }
        else if (panelRaffleSpin.activeSelf == true)
        {
            SendMessageSpinInfos(
       GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].nome,
       GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].numero,
       GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
       GameManager.instance.spinScriptable.sorteioOrdem,
       GameManager.instance.spinScriptable.sorteioDescricao,
       GameManager.instance.spinScriptable.sorteioValor);
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
    public void SendMessageToClientChangeRaffle(string _messageString)
    {
        if (!GameManager.instance.isBackup)
        {
            TcpNetworkManager.instance.Server.SendToAll(GetMessageString(Message.Create(MessageSendMode.reliable, ServerToClientId.messageTypeRaffle), _messageString));
        }
    }
    public void SendMessageLotteryInfos(string _editionNumber, string __competitionNumber, string _dateRaffle, string _dateCompetition, int _ordem, string _description, float _value)
    {
        if (!GameManager.instance.isBackup)
        {
            TcpNetworkManager.instance.Server.SendToAll(GetMessageLotteryInfos(Message.Create(MessageSendMode.reliable, ServerToClientId.messageInfosLottery), _editionNumber, __competitionNumber, _dateRaffle, _dateCompetition, _ordem, _description, _value));
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
    private Message GetMessageString(Message message, string _textMessage)
    {
        message.AddString(_textMessage);

        return message;
    }
    private Message GetMessage(Message message)
    {
        return message;
    }

    private Message GetMessageLotteryInfos(Message message, string _editionNumber, string __competitionNumber, string _dateRaffle, string _dateCompetition, int _ordem, string _description, float _value)
    {
        message.AddString(_editionNumber);
        message.AddString(__competitionNumber);
        message.AddString(_dateRaffle);
        message.AddString(_dateCompetition);
        message.AddInt(_ordem);
        message.AddString(_description);
        message.AddFloat(_value);

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
