using RiptideNetworking.Utils;
using RiptideNetworking;
using System;
using UnityEngine;
using System.Linq;
using DG.Tweening.Core.Easing;

public enum ServerToClientId : ushort
{
    messageTypeRaffle = 1,
    messageVisibilityRaffle = 2,
    messageLotteryNumberRaffle = 3,
    messageInfosLottery = 4,
    messageVisibilityPanelLotteryRaffle = 5,
    messageLotteryNumber = 6,
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
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }
    [SerializeField] private bool isTest;
    [SerializeField] private string ipTest;
    [SerializeField] private string ip;
    [SerializeField] private string ip2;
    [SerializeField] private ushort port;

    public static Client Client { get; private set; }
    public static Client Client2 { get; private set; }

    private void Awake()
    {
        Singleton = this;

        if (isTest == true)
        {
            ip = ipTest;
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

        Connect();
    }

    private void FixedUpdate()
    {
        Client.Tick();
        Client2.Tick();
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

    private void DidConnect(object sender, EventArgs e)
    {
        //GameManager.instance.isConnected = true;
    }
    private void DidClientDisconnect(object sender, EventArgs e)
    {
        Connect();
    }
    private void DidDisconnect(object sender, EventArgs e)
    {
        Connect();
    }
    private void FailedToConnect(object sender, EventArgs e)
    {
        Connect();
    }

    #region Messages

    [MessageHandler((ushort)ServerToClientId.messageTypeRaffle)]
    private static void ReceiveMessageChangeType(Message message)
    {
        string scene = message.GetString();
        if (scene == "SceneGlobe")
        {
            UIManager.instance.ResetRaffle();
        }
    }

    [MessageHandler((ushort)ServerToClientId.messageBall)]
    private static void ReceiveMessage(Message message)
    {
        string[] ballsRaffled = message.GetStrings();
        int[] newBalls = new int[ballsRaffled.Length];
        int forOneBall = message.GetInt();
        int winnersCount = message.GetInt();

        for (int i = 0; i < ballsRaffled.Length; i++)
        {
            newBalls[i] = int.Parse(ballsRaffled[i].ToString());
        }

        UIManager.instance.RecieveBalls(newBalls.ToList(), winnersCount);
    }
    [MessageHandler((ushort)ServerToClientId.messageBallRevoked)]
    private static void ReceiveMessageBallRevoked(Message message)
    {
        string[] ballsRaffled = message.GetStrings();
        int[] newBalls = new int[ballsRaffled.Length];
        int forOneBall = message.GetInt();
        int winnersCount = message.GetInt();


        for (int i = 0; i < ballsRaffled.Length; i++)
        {
            newBalls[i] = int.Parse(ballsRaffled[i].ToString());
        }
        UIManager.instance.RecieveBalls(newBalls.ToList(), winnersCount);
    }

    [MessageHandler((ushort)ServerToClientId.messageNextRaffleGlobe)]
    private static void ReceiveMessageNextRaffleGlobe(Message message)
    {
        UIManager.instance.ResetRaffle();
    }

    [MessageHandler((ushort)ServerToClientId.messageVisibilityRaffle)]
    private static void ReceiveMessageVisibilityRaffle(Message message)
    {
        bool isActive = message.GetBool();
        int sceneActive = message.GetInt();
        if (sceneActive == 2)
        {
            StandyByScreen.instance.SetManualVisibilityScreen(isActive);
        }
    }
    #endregion
}

