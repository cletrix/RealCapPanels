using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RiptideNetworking;

public class LotteryController : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private TicketController ticketController;
    [Header("INPUTS")]
    [SerializeField] private TMP_InputField inputFirstRaffle;
    [SerializeField] private TMP_InputField inputSecondRaffle;
    [SerializeField] private TMP_InputField inputThirdRaffle;
    [SerializeField] private TMP_InputField inputFourthRaffle;
    [SerializeField] private TMP_InputField inputFifthRaffle;
    [Header("BUTTONS LOTTERY")]
    [SerializeField] private Button btFirstRaffle;
    [SerializeField] private Button btSecondRaffle;
    [SerializeField] private Button btThirdRaffle;
    [SerializeField] private Button btFourthRaffle;
    [SerializeField] private Button btFifthRaffle;
    [SerializeField] private Button btLotteryNumberRaffle;
    [SerializeField] private Button btRaffleLuckyNumber;
    [SerializeField] private Button btVisibilityTicket;
    [Header("BUTTONS Infos ")]
    [SerializeField] private Button btVisibilityPanelLotteryRaffle;

    [Header("LOTTERY NUMBER RAFFLE")]
    [SerializeField] private string numberRaffle;
    [SerializeField] private TextMeshProUGUI txtFirstRaffle;
    [SerializeField] private TextMeshProUGUI txtSecondRaffle;
    [SerializeField] private TextMeshProUGUI txtThirdRaffle;
    [SerializeField] private TextMeshProUGUI txtFourthRaffle;
    [SerializeField] private TextMeshProUGUI txtFifthRaffle;
    [SerializeField] private TextMeshProUGUI txtSixthRaffle;


    private const int numberUnlock = 5;
    void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        ticketController = FindObjectOfType<TicketController>();

        inputFirstRaffle = GameObject.Find("InputFirstRaffle").GetComponent<TMP_InputField>();
        inputSecondRaffle = GameObject.Find("InputSecondRaffle").GetComponent<TMP_InputField>();
        inputThirdRaffle = GameObject.Find("InputThirdRaffle").GetComponent<TMP_InputField>();
        inputFourthRaffle = GameObject.Find("InputFourthRaffle").GetComponent<TMP_InputField>();
        inputFifthRaffle = GameObject.Find("InputFifthRaffle").GetComponent<TMP_InputField>();

        btFirstRaffle = GameObject.Find("BtFirstRaffle").GetComponent<Button>();
        btSecondRaffle = GameObject.Find("BtSecondRaffle").GetComponent<Button>();
        btThirdRaffle = GameObject.Find("BtThirdRaffle").GetComponent<Button>();
        btFourthRaffle = GameObject.Find("BtFourthRaffle").GetComponent<Button>();
        btFifthRaffle = GameObject.Find("BtFifthRaffle").GetComponent<Button>();
        btLotteryNumberRaffle = GameObject.Find("BtLotteryNumberRaffle").GetComponent<Button>();
        btRaffleLuckyNumber = GameObject.Find("BtRaffleLuckyNumber").GetComponent<Button>();
        btVisibilityTicket = GameObject.Find("BtVisibilityTicketLottery").GetComponent<Button>();
        btVisibilityPanelLotteryRaffle = GameObject.Find("BtVisibilityPanelLotteryRaffle").GetComponent<Button>();

        txtFirstRaffle = GameObject.Find("TxtFirstRaffle").GetComponent<TextMeshProUGUI>();
        txtSecondRaffle = GameObject.Find("TxtSecondRaffle").GetComponent<TextMeshProUGUI>();
        txtThirdRaffle = GameObject.Find("TxtThirdRaffle").GetComponent<TextMeshProUGUI>();
        txtFourthRaffle = GameObject.Find("TxtFourthRaffle").GetComponent<TextMeshProUGUI>();
        txtFifthRaffle = GameObject.Find("TxtFifthRaffle").GetComponent<TextMeshProUGUI>();
        txtSixthRaffle = GameObject.Find("TxtSixthRaffle").GetComponent<TextMeshProUGUI>();

        SetButtonsEvent();
    }

    public void PopulateLotteryFederalExtractions(string first, string second, string third, string fourth, string fifith)
    {
        inputFirstRaffle.text = first;
        inputSecondRaffle.text = second;
        inputThirdRaffle.text = third;
        inputFourthRaffle.text = fourth;
        inputFifthRaffle.text = fifith;
    }
    public void ResetNumberRaffle()
    {
        numberRaffle = string.Empty;
        txtFirstRaffle.text = string.Empty;
        txtSecondRaffle.text = string.Empty;
        txtThirdRaffle.text = string.Empty;
        txtFourthRaffle.text = string.Empty;
        txtFifthRaffle.text = string.Empty;
        txtSixthRaffle.text = string.Empty;
    }
 
    private void SetButtonsEvent()
    {
        btFirstRaffle.onClick.AddListener(SetLotteryFirstRaffle);
        btSecondRaffle.onClick.AddListener(SetLotterySecondRaffle);
        btThirdRaffle.onClick.AddListener(SetLotteryThirdRaffle);
        btFourthRaffle.onClick.AddListener(SetLotteryFourthRaffle);
        btFifthRaffle.onClick.AddListener(SetLotteryFifithRaffle);
        btLotteryNumberRaffle.onClick.AddListener(SendLotteryNumberRaffle);
        btRaffleLuckyNumber.onClick.AddListener(RaffleLuckyNumber);
        btVisibilityPanelLotteryRaffle.onClick.AddListener(SendMessageToAllClientsVisibilityLotteryPanelRaffle);
    }

    private void SetLotteryFirstRaffle()
    {
        string lotteryNumber = inputFirstRaffle.text;
        SendMessageToClient(1, lotteryNumber);
    }
    private void SetLotterySecondRaffle()
    {
        string lotteryNumber = inputSecondRaffle.text;

        SendMessageToClient(2, lotteryNumber);
    }
    private void SetLotteryThirdRaffle()
    {
        string lotteryNumber = inputThirdRaffle.text;
        SendMessageToClient(3, lotteryNumber);

    }
    private void SetLotteryFourthRaffle()
    {
        string lotteryNumber = inputFourthRaffle.text;
        SendMessageToClient(4, lotteryNumber);
    }
    private void SetLotteryFifithRaffle()
    {
        string lotteryNumber = inputFifthRaffle.text;
        SendMessageToClient(5, lotteryNumber);

    }
    private void RaffleLuckyNumber()
    {
        numberRaffle = GameManager.instance.lotteryResultScriptable.combinacaoResultado;
        txtFirstRaffle.text = numberRaffle[0].ToString();
        txtSecondRaffle.text = numberRaffle[1].ToString();
        txtThirdRaffle.text = numberRaffle[2].ToString();
        txtFourthRaffle.text = numberRaffle[3].ToString();
        txtFifthRaffle.text = numberRaffle[4].ToString();
        txtSixthRaffle.text = numberRaffle[5].ToString();

    }
    private void SendLotteryNumberRaffle()
    {
        SendMessageToClientLotteryRaffleNumber(numberRaffle);
    }
    private void ActiveOrDesactiveRafflesButtons()
    {
        btFirstRaffle.interactable = inputFirstRaffle.text.Length == numberUnlock ? true : false;
        btSecondRaffle.interactable = inputSecondRaffle.text.Length == numberUnlock ? true : false;
        btThirdRaffle.interactable = inputThirdRaffle.text.Length == numberUnlock ? true : false;
        btFourthRaffle.interactable = inputFourthRaffle.text.Length == numberUnlock ? true : false;
        btFifthRaffle.interactable = inputFifthRaffle.text.Length == numberUnlock ? true : false;
        btLotteryNumberRaffle.interactable = numberRaffle.Length - 1 == numberUnlock ? true : false;
        btVisibilityTicket.interactable = numberRaffle.Length - 1 == numberUnlock ? true : false;

        btFirstRaffle.GetComponentInChildren<TextMeshProUGUI>().color = inputFirstRaffle.text.Length == numberUnlock ? Color.white : new Color(1, 1, 1, 0.4f);
        btSecondRaffle.GetComponentInChildren<TextMeshProUGUI>().color = inputSecondRaffle.text.Length == numberUnlock ? Color.white : new Color(1, 1, 1, 0.4f);
        btThirdRaffle.GetComponentInChildren<TextMeshProUGUI>().color = inputThirdRaffle.text.Length == numberUnlock ? Color.white : new Color(1, 1, 1, 0.4f);
        btFourthRaffle.GetComponentInChildren<TextMeshProUGUI>().color = inputFourthRaffle.text.Length == numberUnlock ? Color.white : new Color(1, 1, 1, 0.4f);
        btFifthRaffle.GetComponentInChildren<TextMeshProUGUI>().color = inputFifthRaffle.text.Length == numberUnlock ? Color.white : new Color(1, 1, 1, 0.4f);
    }
    public void ShowTicketLottery()
    {
        string prizeValueDescription = $"Loteria Federal - {GameManager.instance.lotteryScriptable.sorteioDescricao}";
        ticketController.PopulateTicketInfos(
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.nome,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.cpf,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.dataNascimento,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.telefone,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.email,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.bairro,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.municipio,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.estado,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.dataSorteio,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].numero,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.valor,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.PDV,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.bairoPDV,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.dataCompra,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.horaCompra,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.numeroTitulo,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.chance,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.numeroCartela,
            GameManager.instance.lotteryResultScriptable.ganhadorContemplado.numeroSorte);
    }
    #region SendMessages
    public void SendMessageToClient(int _index, string _number)
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessageStrings(Message.Create(MessageSendMode.reliable, ServerToClientId.messageLotteryNumber), _index, _number));
    }
    public void SendMessageToClientLotteryRaffleNumber(string _numberRaffle)
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessageString(Message.Create(MessageSendMode.reliable, ServerToClientId.messageLotteryNumberRaffle), _numberRaffle));
    }
    public void SendMessageToAllClientsVisibilityLotteryPanelRaffle()
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.messageVisibilityPanelLotteryRaffle)));
    }
    private Message GetMessageStrings(Message message, int index, string _number)
    {
        message.AddInt(index);
        message.AddString(_number);
        return message;
    }
    private Message GetMessageString(Message message, string _number)
    {
        message.AddString(_number);
        return message;
    }
    private Message GetMessage(Message message)
    {
        //message.AddBool(_isActive);
        return message;
    }
    #endregion
    private void Update()
    {
        ActiveOrDesactiveRafflesButtons();
    }

}
