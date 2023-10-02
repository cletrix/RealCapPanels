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
    public enum MODE { LOCAL, PRODUCAO }

    public MODE current_mode = MODE.LOCAL;
    [Space]
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
        SetCurrentMode(current_mode);
    }

    private void SetCurrentMode(MODE _current_mode)
    {
        switch (_current_mode)
        {
            case MODE.LOCAL:
                ip = "192.168.0.4";
                break;
            case MODE.PRODUCAO:
                ip = "192.168.20.31";
                ip2 = "192.168.20.32";
                break;
            default:
                break;
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
        string scene = message.GetString();
        int maxBalls = message.GetInt();
        int gridBallsTicket = message.GetInt();
        GameManager.instance.CallChangeSceneRaffle(scene);
        GameManager.instance.SetGridBallsTicket(gridBallsTicket);
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
        float value = message.GetFloat();
        UiGlobeManager uiRaffleManager = FindObjectOfType<UiGlobeManager>();
        if (SceneManager.GetActiveScene().name == "SceneGlobeFull")
            uiRaffleManager.SetPopulateInfosGlobe(editionName, editionNumber, date, order, description, value);
    }

    [MessageHandler((ushort)ServerToClientId.messageBall)]
    private static void ReceiveMessageInfosGlobe(Message message)
    {
        string[] ballsRaffled = message.GetStrings();
        int forOneBall = message.GetInt();
        int winnersCount = message.GetInt();
        float prizeValue = message.GetFloat();
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
        float prizeValue = message.GetFloat();
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
        float value = message.GetFloat();
        LuckySpinController luckySpinController = FindObjectOfType<LuckySpinController>();
        if (SceneManager.GetActiveScene().buildIndex == 3)
            luckySpinController.SetPopulateSpinInfos(value, editionNumber, description);
    }
    [MessageHandler((ushort)ServerToClientId.messageSpinRoundID)]
    private static void ReceiveMessageSpinRoundID(Message message)
    {
        int roundID;
        roundID = message.GetInt();

        LuckySpinController luckySpinController = FindObjectOfType<LuckySpinController>();
        if (luckySpinController != null)
        {
            luckySpinController.SetRoundIDSpin(roundID);
            luckySpinController.ResetResult();
        }
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
        print("TICKET VISIBLE" + isTicketVisibility);

        ticketScreen.SetTicketVisibilityForNextDraw(isTicketVisibility);
        ticketScreen.SetTicketInfos(ticketInfos, numberCards, iscard, typeRaffle);

    }
    [MessageHandler((ushort)ServerToClientId.messageHideTicket)]
    private static void ReceiveMessageHideTicket(Message message)
    {
        TicketScreen ticketScreen = FindObjectOfType<TicketScreen>();

        bool isTicketVisibility = message.GetBool();
        bool withNextDrawn = message.GetBool();
        //print(withNextDrawn);
        //if (withNextDrawn == true)
        //    ticketScreen.SetTicketVisibilityForNextDraw(isTicketVisibility);
        //else
            ticketScreen.SetTicketVisibility();
    }

    [MessageHandler((ushort)ServerToClientId.messageVisibilityPrize)]
    private static void ReceiveMessageVisibilityPrize(Message message)
    {
        WinnersScreen winnersScreen = FindObjectOfType<WinnersScreen>();

        bool isPrizeVisibility = message.GetBool();

        winnersScreen.SetWinnersScreenVisibility(isPrizeVisibility);
    }

    #endregion




}

