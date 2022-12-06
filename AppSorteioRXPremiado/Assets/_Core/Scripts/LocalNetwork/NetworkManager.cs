using System;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine.SceneManagement;

public enum ServerToClientId : ushort
{
    messageTypeRaffle = 1,
    messageVisibilityRaffle = 2,
    messageLotteryNumberRaffle = 3,
    messageInfosLottery = 4,
    messageVisibilityPanelLotteryRaffle = 5,
    messageLotteryNumber = 6,
    messageInfosGlobe = 7,
    messageBall = 8,
    messageBallRevoked = 9,
    messageSpinNumber = 10,
    messageSpinRoundID = 11,
    messageInfosSpin = 12,
    messageShowTicket = 13,
    messageHideTicket = 14,
    messageCheckSceneActive = 15,
    messageVisibilityPrize = 16,
    messageNextRaffleGlobe = 17

}
public enum ClientToServerId : ushort
{
    messageConfirmBall = 1,
    messageChangeScene = 2,
    messageCheckVisibilityScreen = 3,
    messageGetActiveScene = 4
}
public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance;
    public static NetworkManager instance
    {
        get => _instance;
        private set
        {
            if (_instance == null)
                _instance = value;
            else if (_instance != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private string ip;
    [SerializeField] private string ip2;
    [SerializeField] private ushort port;

    public static Client Client { get; private set; }
    public static Client Client2 { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.Disconnected += DidDisconnect;
        Client.ClientDisconnected += DidClientDisconnect;

        Client2 = new Client();
        Client2.Connected += DidConnect;
        Client2.ConnectionFailed += FailedToConnect;
        Client2.Disconnected += DidDisconnect;
        Client2.ClientDisconnected += DidClientDisconnect;

    }
    public void Connect()
    {
        if (Client.IsNotConnected)
        {
            Client.Connect($"{ip}:{port}");
        }
        if (Client2.IsNotConnected)
        {
            Client2.Connect($"{ip2}:{port}");
        }
    }
    private void OnApplicationQuit()
    {
        Client.Disconnect();

        Client.Connected -= DidConnect;
        Client.ConnectionFailed -= FailedToConnect;
        Client.Disconnected -= DidDisconnect;
        Client.ClientDisconnected -= DidClientDisconnect;

        Client2.Disconnect();

        Client2.Connected -= DidConnect;
        Client2.ConnectionFailed -= FailedToConnect;
        Client2.Disconnected -= DidDisconnect;
        Client2.ClientDisconnected -= DidClientDisconnect;

    }

    private void DidConnect(object sender, EventArgs e)
    {
        GameManager.instance.isConnected = true;
    }
    private void DidClientDisconnect(object sender, EventArgs e)
    {
        GameManager.instance.ConnectServer();
    }
    private void DidDisconnect(object sender, EventArgs e)
    {
        GameManager.instance.ConnectServer();
    }
    private void FailedToConnect(object sender, EventArgs e)
    {
        GameManager.instance.ConnectServer();
    }
    private void FixedUpdate()
    {
        Client.Tick();
        Client2.Tick();
    }

    #region Messages

    [MessageHandler((ushort)ServerToClientId.messageVisibilityRaffle)]
    private static void ReceiveMessageVisibilityRaffle(Message message)
    {
        bool isActive = message.GetBool();
        StandyByScreen.instance.SetManualVisibilityScreen(isActive);
    }
    [MessageHandler((ushort)ServerToClientId.messageCheckSceneActive)]
    private static void ReceiveMessageGetSceneActive(Message message)
    {
        GameManager.instance.SendMessageToServerVisibilityScene(StandyByScreen.instance.GetVisibilityStandBy());
        GameManager.instance.SendMessageToServerGetActiveScene(SceneManager.GetActiveScene().buildIndex);
    }

    [MessageHandler((ushort)ServerToClientId.messageTypeRaffle)]
    private static void ReceiveMessageChangeType(Message message)
    {
        GameManager.instance.CallChangeSceneRaffle(message.GetString());
    }

    [MessageHandler((ushort)ServerToClientId.messageInfosLottery)]
    private static void ReceiveMessageInfosLottery(Message message)
    {
        string _editionNumber = message.GetString();
        string __competitionNumber = message.GetString();
        string _dateRaffle = message.GetString();
        string _dateCompetition = message.GetString();
        int _ordem = message.GetInt();
        string _description = message.GetString();
        string _value = message.GetString();

        LotteryController lotteryController = FindObjectOfType<LotteryController>();
        lotteryController.SetPopulateInfosLottery(_editionNumber, __competitionNumber, _dateRaffle, _dateCompetition);
    }
    [MessageHandler((ushort)ServerToClientId.messageLotteryNumber)]
    private static void ReceiveMessageLotteryNumber(Message message)
    {
        int indexNumber = 0;
        string recieveNumbers = string.Empty;
        indexNumber = message.GetInt();
        recieveNumbers = message.GetString();

        LotteryController lotteryController = FindObjectOfType<LotteryController>();
        lotteryController.SetLotteryNumber(indexNumber, recieveNumbers);
    }
    [MessageHandler((ushort)ServerToClientId.messageLotteryNumberRaffle)]
    private static void ReceiveMessageLotteryNumberRaffle(Message message)
    {
        string recieveNumber;
        recieveNumber = message.GetString();

        LotteryController lotteryController = FindObjectOfType<LotteryController>();
        lotteryController.ShowLotteryNumberRaffle(recieveNumber);
    }
    [MessageHandler((ushort)ServerToClientId.messageVisibilityPanelLotteryRaffle)]
    private static void ReceiveMessageVisibilityPanelLotteryRaffle(Message message)
    {
        LotteryController lotteryController = FindObjectOfType<LotteryController>();
        lotteryController.SetVisibilityLotteryRaffle();
    }
    [MessageHandler((ushort)ServerToClientId.messageNextRaffleGlobe)]
    private static void ReceiveMessageNextRaffleGlobe(Message message)
    {
        UiGlobeManager uiRaffleManager = FindObjectOfType<UiGlobeManager>();
        uiRaffleManager.UpdateOrder();
        GameManager.instance.ResetScene();
    }
    [MessageHandler((ushort)ServerToClientId.messageInfosGlobe)]
    private static void ReceiveMessageGlobeBall(Message message)
    {
        string editionName = message.GetString();
        string editionNumber = message.GetString();
        string date = message.GetString();
        int order = message.GetInt();
        string description = message.GetString();
        string value = message.GetString();

        UiGlobeManager uiRaffleManager = FindObjectOfType<UiGlobeManager>();
        if (SceneManager.GetActiveScene().buildIndex == 2)
            uiRaffleManager.SetPopulateInfosGlobe(editionName, editionNumber, date, order, description, value);
    }

    [MessageHandler((ushort)ServerToClientId.messageBall)]
    private static void ReceiveMessageInfosGlobe(Message message)
    {
        string[] ballsRaffled = message.GetStrings();
        int forOneBall = message.GetInt();
        int winnersCount = message.GetInt();
        string prizeValue = message.GetString();
        UiGlobeManager uiRaffleManager = FindObjectOfType<UiGlobeManager>();
        if (uiRaffleManager != null && ballsRaffled.Length > 0)
        {
            uiRaffleManager.SetGlobeRaffle(ballsRaffled, forOneBall, winnersCount, prizeValue);
        }
    }
    [MessageHandler((ushort)ServerToClientId.messageBallRevoked)]
    private static void ReceiveMessageBallRevoked(Message message)
    {
        string[] ballsRaffled = message.GetStrings();
        int forOneBall = message.GetInt();
        int winnersCount = message.GetInt();
        string prizeValue = message.GetString();
        UiGlobeManager uiRaffleManager = FindObjectOfType<UiGlobeManager>();
        if (uiRaffleManager != null && ballsRaffled.Length > 0)
        {
            uiRaffleManager.SetGlobeRaffle(ballsRaffled, forOneBall, winnersCount, prizeValue);
        }
    }

    [MessageHandler((ushort)ServerToClientId.messageSpinNumber)]
    private static void ReceiveMessageSpinNumber(Message message)
    {
        string luckyNumber = string.Empty;
        luckyNumber = message.GetString();
        print("Ativou");
        LuckySpinController luckySpinController = FindObjectOfType<LuckySpinController>();
        luckySpinController.SetResult(luckyNumber);
    }
    [MessageHandler((ushort)ServerToClientId.messageInfosSpin)]
    private static void ReceiveMessageInfosSpin(Message message)
    {
        string editionName = message.GetString();
        string editionNumber = message.GetString();
        string date = message.GetString();
        int order = message.GetInt();
        string description = message.GetString();
        string value = message.GetString();
        LuckySpinController luckySpinController = FindObjectOfType<LuckySpinController>();
        if (SceneManager.GetActiveScene().buildIndex == 3)
            luckySpinController.SetPopulateSpinInfos(value, editionNumber,description);
    }
    [MessageHandler((ushort)ServerToClientId.messageSpinRoundID)]
    private static void ReceiveMessageLotterySpinRoundID(Message message)
    {
        int roundID;
        roundID = message.GetInt();

        LuckySpinController luckySpinController = FindObjectOfType<LuckySpinController>();
        luckySpinController.SetRoundIDSpin(roundID);
        luckySpinController.ResetResult();

    }
    [MessageHandler((ushort)ServerToClientId.messageShowTicket)]
    private static void ReceiveMessageShowTicket(Message message)
    {
        TicketScreen ticketScreen = FindObjectOfType<TicketScreen>();

        bool isTicketVisibility = message.GetBool();
        string[] ticketInfos = message.GetStrings();
        int[] numberCards = message.GetInts();
        bool iscard = message.GetBool();
        int typeRaffle = message.GetInt();
        print("TICKET VISIBLE"+isTicketVisibility);

        ticketScreen.SetTicketVisibility(isTicketVisibility);
        ticketScreen.SetTicketInfos(ticketInfos, numberCards, iscard, typeRaffle);

    }
    [MessageHandler((ushort)ServerToClientId.messageHideTicket)]
    private static void ReceiveMessageHideTicket(Message message)
    {
        TicketScreen ticketScreen = FindObjectOfType<TicketScreen>();

        bool isTicketVisibility = message.GetBool();
        ticketScreen.SetTicketVisibility(isTicketVisibility);
    }

    [MessageHandler((ushort)ServerToClientId.messageVisibilityPrize)]
    private static void ReceiveMessageVisibilityPrize(Message message)
    {
        WinnersScreen winnersScreen = FindObjectOfType<WinnersScreen>();

        bool isPrizeVisibility = message.GetBool();
        print("PRIZE VISIBLE? ==>" + isPrizeVisibility);
        winnersScreen.SetWinnersScreenVisibility(isPrizeVisibility);
    }

    #endregion




}

