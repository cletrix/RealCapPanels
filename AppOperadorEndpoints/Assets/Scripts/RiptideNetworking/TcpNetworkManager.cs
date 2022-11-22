using RiptideNetworking.Utils;
using RiptideNetworking;
using UnityEngine;
using System.Linq;

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

public class TcpNetworkManager : MonoBehaviour
{
    private static TcpNetworkManager _instance;
    public static TcpNetworkManager instance
    {
        get => _instance;
        private set
        {
            if (_instance == null)
                _instance = value;
            else if (_instance != value)
            {
                Debug.Log($"{nameof(TcpNetworkManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    public Server Server { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;


        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();

        Server.Start(port, maxClientCount);

    }
    private void InvokeScreen()
    {

    }


    [MessageHandler((ushort)ClientToServerId.messageConfirmBall)]
    private static void RecieveMessageFromClientConfirmBall(ClientToServerId id, Message message)
    {
        //if (!GameManager.instance.isbackup)
        //{
        //    GlobeController globeController = FindObjectOfType<GlobeController>();
        //    globeController.UpdateScreen();
        //}

    }

    [MessageHandler((ushort)ClientToServerId.messageChangeScene)]
    private static void RecieveMessageFromClientConfirmChangeScene(ClientToServerId id, Message message)
    {
        //if (!GameManager.instance.isbackup)
        //{
        //    UIChangeRaffleType raffleType = FindObjectOfType<UIChangeRaffleType>();
        //    bool isActive = message.GetBool();
        //    int sceneID = message.GetInt();
        //    GameManager.instance.sceneId = sceneID;
        //    raffleType.SetStateCanChangeScene(isActive);

        //    GameManager.instance.technicalScriptable.UpdateConfig(
        //              GameManager.instance.sceneId,
        //              GameManager.instance.globeRaffleScriptable.bolasSorteadas,
        //              GameManager.instance.globeScriptable.sorteioOrdem,
        //              GameManager.instance.isVisibleRaffle
        //              );
        //}
    }

    [MessageHandler((ushort)ClientToServerId.messageCheckVisibilityScreen)]
    private static void RecieveMessageFromClientCheckSVisibilityRaffle(ClientToServerId id, Message message)
    {
        //if (!GameManager.instance.isbackup)
        //{
        //    UIChangeRaffleType raffleType = FindObjectOfType<UIChangeRaffleType>();
        //    bool isActive = message.GetBool();

        //    //GameManager.instance
        //    //raffleType.SetStateVisibilityOfRaffle();
        //}
    }

    [MessageHandler((ushort)ClientToServerId.messageGetActiveScene)]
    private static void RecieveMessageFromClientSceneIndex(ClientToServerId id, Message message)
    {
        //if (!GameManager.instance.isbackup)
        //{
        //    UIChangeRaffleType raffleType = FindObjectOfType<UIChangeRaffleType>();
        //    int sceneID = message.GetInt();
        //    if (sceneID != 0)
        //        GameManager.instance.sceneId = sceneID;
        //    raffleType.SelectPanelForActivate(sceneID);
        //    GameManager.instance.technicalScriptable.UpdateConfig(
        //              GameManager.instance.sceneId,
        //              GameManager.instance.globeRaffleScriptable.bolasSorteadas,
        //              GameManager.instance.globeScriptable.sorteioOrdem,
        //              GameManager.instance.isVisibleRaffle
        //              );
        //}
    }

    private void FixedUpdate()
    {
        Server.Tick();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }
}



