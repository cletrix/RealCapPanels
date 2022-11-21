using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RiptideNetworking;
using UnityEngine.SceneManagement;
using System.Reflection;

public class GameManager : MonoBehaviour
{

    #region Singleton
    private static GameManager _instance;

    public static GameManager instance { get { return _instance; } }

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
    #endregion

    public GlobeScriptable globeScriptable;
    public LuckySpinScriptable luckySpinScriptable;


    // [SerializeField] private FadeController fade;
    [SerializeField] public bool isConnected = false;
    [SerializeField] public bool isLotteryOpenedScreen = false;
    [SerializeField] private string currentSceneName;
    [SerializeField] private int sceneIndex;

    public Camera cameraActive;
    private void Start()
    {

        InitializeVariables();
    }
    private void InitializeVariables()
    {
        // fade = FindObjectOfType<FadeController>();
        Invoke("ConnectServer", 1f);
        Application.targetFrameRate = 60;
        globeScriptable.ResetRaffle();
    }
    public void ConnectServer()
    {
        NetworkManager.instance.Connect();
    }
    public void ResetScene()
    {
        globeScriptable.ResetRaffle();
        SceneManager.LoadScene(currentSceneName);
    }
    public void CallChangeSceneRaffle(string sceneName)
    {
        StartCoroutine(ChangeSceneRaffle(sceneName));
    }
    private IEnumerator ChangeSceneRaffle(string sceneName)
    {
        if (currentSceneName != sceneName)
        {
            currentSceneName = sceneName;
            yield return new WaitForSeconds(0.8f);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
            cameraActive = Camera.main;

            WinnersScreen winners = FindObjectOfType<WinnersScreen>();
            winners.GetComponent<Canvas>().worldCamera = cameraActive;

            TicketScreen ticket = FindObjectOfType<TicketScreen>();
            ticket.GetComponent<Canvas>().worldCamera = cameraActive;

            SendMessageToServerConfirmChange(true, sceneIndex);
        }
    }
    #region SendMessages
    public void SendMessageToServerVisibilityScene(bool _state)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.messageCheckVisibilityScreen);
        message.AddBool(_state);

        NetworkManager.Client.Send(message);
    }
    public void SendMessageToServerGetActiveScene(int index)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.messageGetActiveScene);
        message.AddInt(index);

        NetworkManager.Client.Send(message);
    }
    public void SendMessageToServerConfirmChange(bool _state, int _scene)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.messageChangeScene);
        message.AddBool(_state);
        message.AddInt(_scene);

        NetworkManager.Client.Send(message);
    }
    public int GetSceneIndex()
    {
        return sceneIndex;
    }

    #endregion

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Application.Quit();
            }
        }
    }
}


