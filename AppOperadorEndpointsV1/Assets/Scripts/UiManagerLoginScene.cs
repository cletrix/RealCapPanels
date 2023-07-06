using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
public class UiManagerLoginScene : MonoBehaviour
{
    [Header("GERAL REFERENCES")]
    [SerializeField] private FadeController fadeController;

    [Header("LOGIN COMPONENTS")]
    [SerializeField] private GameObject panelLogin;

    [Header("COMPONENTS")]
    [SerializeField] private GameObject panelSelectEdition;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_InputField inputUsername;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private TMP_InputField inputIPAddress;
    public Button btSelectBackup;
    public Button btConfirm;
    public bool hasbackup = false;
    void Start()
    {
        InitializeVariables();
    }
    private void InitializeVariables()
    {
        fadeController = FindObjectOfType<FadeController>();
        panelSelectEdition.SetActive(false);
        panelLogin.SetActive(true);
        btSelectBackup.onClick.AddListener(SelectBackup);
        PopulateFieldIPAddress(RestNetworkManager.instance.baseUrl);
    }

    private void PopulateFieldIPAddress(string url)
    {
        inputIPAddress.text = url;
    }
    private void SetNewIPAddress()
    {
        RestNetworkManager.instance.baseUrl = inputIPAddress.text;
    }
    public void SelectBackup()
    {
        if (GameManager.instance.isBackup)
        {
            GameManager.instance.isBackup = false;
            btSelectBackup.image.color = Color.white;
        }
        else
        {
            GameManager.instance.isBackup = true;
            btSelectBackup.image.color = Color.green;
        }
    }
    public void Login()
    {
        SetNewIPAddress();
        btConfirm.interactable = false;
        string url = RestNetworkManager.instance.baseUrl + RestNetworkManager.instance.urlLogin;
        StartCoroutine(GetLoginInfos(url));
    }
    private IEnumerator GetLoginInfos(string uri)
    {
        print("urlLogin   " + uri);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    btConfirm.interactable = true;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    btConfirm.interactable = true;
                    break;
                case UnityWebRequest.Result.Success:
                    {

                        string json = webRequest.downloadHandler.text;
                        JsonUtility.FromJsonOverwrite(json, GameManager.instance.editionScriptable);
                        GameManager.instance.CallEventLogin();
                        panelSelectEdition.SetActive(true);
                        panelLogin.SetActive(false);
                        PopulateDropdownEditions();
                        GameManager.instance.editionScriptable.SetInfosTecnical(inputUsername.text,"XXX.XXX.XXX-XX");
                    }
                    break;
            }
        }
    }
  
    public void PopulateDropdownEditions()
    {
        dropdown.ClearOptions();
        List<string> newOptions = new List<string>();

        for (int i = 0; i < GameManager.instance.editionScriptable.edicaoInfos.Count; i++)
        {
            string option = $"{GameManager.instance.editionScriptable.edicaoInfos[i].numero} - {GameManager.instance.editionScriptable.edicaoInfos[i].nome} - {GameManager.instance.editionScriptable.edicaoInfos[i].dataRealizacao}";
            newOptions.Add(option);
        }
        dropdown.AddOptions(newOptions);
    }
    public void ConfirmSelection()
    {
        StartCoroutine(SelectEdition());
    }
    private IEnumerator SelectEdition()
    {
        GameManager.instance.SetEditionIndex(dropdown.value);
        fadeController.SetStateFadeOUT();
        yield return new WaitForSeconds(1f);
        GameManager.instance.LoadSceneGame("MainScene");
    }

    public class RequestLogin
    {
        public string usuario;
        public string senha;
    }
}
