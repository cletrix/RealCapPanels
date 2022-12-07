using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RiptideNetworking;

public class SpinController : MonoBehaviour
{
    [Header("SPIN RAFFLE")]
    [SerializeField] private List<SpinRaffleData> spinRaffleDatas = new List<SpinRaffleData>();
    [SerializeField] private List<TextMeshProUGUI> NumberRaffle = new List<TextMeshProUGUI>();
    [SerializeField] private TextMeshProUGUI txtSpinID;
    [SerializeField] private int indexSpin;
    [SerializeField] private string currentNumberRaffled;

    [Header("BUTTONS")]
    [SerializeField] private Button btGenerateLuckyNumber;

    [Header("REFERENCES")]
    [SerializeField] private GameObject contentScrollView;
    [SerializeField] private GameObject groupNumberSpinRaffle;
    [SerializeField] private Scrollbar verticalScrollbar;
    [SerializeField] private TicketController ticketController;
    void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        contentScrollView = GameObject.Find("Content");
        groupNumberSpinRaffle = GameObject.Find("GroupNumberSpinRaffle");

        ticketController = FindObjectOfType<TicketController>();

        verticalScrollbar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        btGenerateLuckyNumber = GameObject.Find("BtSGenerateTicket").GetComponent<Button>();
        txtSpinID = GameObject.Find("TxtSpinID").GetComponent<TextMeshProUGUI>();

        NumberRaffle.Clear();
        NumberRaffle.AddRange(groupNumberSpinRaffle.GetComponentsInChildren<TextMeshProUGUI>());

        foreach (var item in spinRaffleDatas)
        {
            item.InitializeVariables();
        }
        SetButtonsEvent();
        ShowSpinOrder(GameManager.instance.spinScriptable.sorteioOrdem);
        SetSpinPrize();
        PopulateFieldsSpinData();
        UpdateFieldScreen();
        ActiveButtonNewRaffleSpin();
    }
    public void SetIndexSpin(int spinOrder)
    {
        indexSpin = spinOrder;
    }
    public void ShowSpinOrder(int order)
    {
        UiInfosRaffle uiInfos = FindObjectOfType<UiInfosRaffle>();

        txtSpinID.text = $"{order}º GIRO";
        uiInfos.PopulateRaffleInfos(order.ToString(), GameManager.instance.spinScriptable.sorteioDescricao, GameManager.instance.spinScriptable.sorteioValor);
    }
    private void SetSpinPrize()
    {
        for (int i = 0; i < spinRaffleDatas.Count; i++)
        {
            spinRaffleDatas[i].SetSpinPrize(GameManager.instance.spinScriptable.sorteioValor);
            spinRaffleDatas[i].SetStateInteractableButton(false);
        }
    }

    private void UpdateFieldScreen()
    {
        for (int i = 0; i < spinRaffleDatas.Count; i++)
        {
            if (i != indexSpin-1)
            {
                if (spinRaffleDatas[i].GetIsFinishedRaffle())
                {
                    spinRaffleDatas[i].SetBgFinishedColor();
                    spinRaffleDatas[i].SetBtFinishedColor();
                }
                else
                {
                    spinRaffleDatas[i].SetBgNormalColor();
                    spinRaffleDatas[i].SetBtNormalColor();
                }
            }
            else
            {
                spinRaffleDatas[i].SetBgSelectedColor();
                spinRaffleDatas[i].SetBtSelectedColor();
            }
        }
    }
    private void SetButtonsEvent()
    {
        btGenerateLuckyNumber.onClick.AddListener(SpawnNewLuckyNumber);
    }
    private void SpawnNewLuckyNumber()
    {
        RestNetworkManager.instance.SetPostResultSpin(GameManager.instance.spinScriptable.sorteioOrdem);
       
        btGenerateLuckyNumber.interactable = false;
    }
    public void ShowNumberLuckySpin()
    {
        StartCoroutine(RaffleNumberLuckySpin());
    }
    private IEnumerator RaffleNumberLuckySpin()
    {
        SendMessageToClientSpinNumber(GameManager.instance.spinResultScriptable.numeroSorteado);

        currentNumberRaffled = GameManager.instance.spinResultScriptable.numeroSorteado;
        GameManager.instance.technicalScriptable.UpdateSpinConfig(currentNumberRaffled, GameManager.instance.spinResultScriptable.ganhadorContemplado);
        GameManager.instance.spinScriptable.sorteioOrdem = GameManager.instance.technicalScriptable.spinIndex;
        ShowSpinOrder(GameManager.instance.spinScriptable.sorteioOrdem);
        indexSpin = GameManager.instance.spinScriptable.sorteioOrdem;

        UpdateFieldScreen();
        SendMessageRoundID(GameManager.instance.spinScriptable.sorteioOrdem);
       

        //currentNumberRaffled = string.Empty;
        foreach (var item in NumberRaffle)
        {
            item.text = "0";
        }
        yield return new WaitForSeconds(0.1f);
       

        for (int i = 0; i < NumberRaffle.Count; i++)
        {
            NumberRaffle[i].text = currentNumberRaffled[i].ToString();
        }
        
        spinRaffleDatas[indexSpin-1].SetNumberTicket(currentNumberRaffled);
        yield return new WaitForSeconds(0.1f);
        spinRaffleDatas[indexSpin-1].SetStateInteractableButton(true);
        spinRaffleDatas[indexSpin-1].SetStateFinishedRaffle(true);


    }
    public void ActiveLastSpin()
    {
        spinRaffleDatas[GameManager.instance.spinScriptable.sorteioOrdem - 1].SetStateInteractableButton(true);
    }
    public void PopulateSpinsFields(List<string> spinNumbers)
    {
        UpdateFieldScreen();
        for (int i = 0; i < spinNumbers.Count; i++)
        {
            spinRaffleDatas[i].SetNumberTicket(spinNumbers[i]);
            if (i != spinNumbers.Count - 1)
            {
                spinRaffleDatas[i].SetStateFinishedRaffle(true);
                spinRaffleDatas[i].SetBgFinishedColor();
                spinRaffleDatas[i].SetBtFinishedColor();
            }
            else
            {
                string currentNumber = spinNumbers[i];
                spinRaffleDatas[i].SetStateInteractableButton(true);
                for (int h = 0; h < currentNumber.Length; h++)
                {
                    NumberRaffle[h].text = currentNumber[h].ToString();
                }
                spinRaffleDatas[i].SetBgSelectedColor();
                spinRaffleDatas[i].SetBtSelectedColor();
                spinRaffleDatas[i].SetStateFinishedRaffle(true);

            }
        }

    }
    private void PopulateFieldsSpinData()
    {
        for (int i = 0; i < spinRaffleDatas.Count; i++)
        {
            spinRaffleDatas[i].SetNumberSpin(i + 1);

            if (i == 0)
            {
                spinRaffleDatas[i].SetBgSelectedColor();
            }
            else
            {
                spinRaffleDatas[i].SetBgNormalColor();
            }
        }
    }
    public void ActiveButtonNewRaffleSpin()
    {
        if (GameManager.instance.isBackup)
        {
            btGenerateLuckyNumber.interactable = false;
        }
        else
        {
            if (GameManager.instance.technicalScriptable.spinIndex < GameManager.instance.recoveryScriptable.limit_spin)
                btGenerateLuckyNumber.interactable = true;
            else
                btGenerateLuckyNumber.interactable = false;

        }
    }
    public void ShowTicketSpin()
    {
        PopulateTicketSpin();
        ticketController.SetTicketVisibility();
    }
    public void PopulateTicketSpin()
    {
        ticketController.PopulateTicketInfos(
            GameManager.instance.spinResultScriptable.ganhadorContemplado.nome,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.cpf,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.dataNascimento,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.telefone,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.email,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.endereco,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.numero,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.complemento,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.bairro,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.municipio,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.cep,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.estado,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.dataSorteio,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].numero,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.valor,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.PDV,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.bairoPDV,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.dataCompra,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.horaCompra,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.numeroTitulo,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.chance,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.numeroCartela,
            GameManager.instance.spinResultScriptable.ganhadorContemplado.numeroSorte,
            false,
            3);
    }
    public void SendMessageRoundID(int _roundID)
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessageInt(Message.Create(MessageSendMode.reliable, ServerToClientId.messageSpinRoundID), _roundID));
    }
    public void SendMessageToClientSpinNumber(string _number)
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessageString(Message.Create(MessageSendMode.reliable, ServerToClientId.messageSpinNumber), _number));
    }
    private Message GetMessageInt(Message message, int _roundID)
    {
        message.AddInt(_roundID);
        return message;
    }
    private Message GetMessageString(Message message, string _number)
    {
        message.AddString(_number);
        return message;
    }

    //private void FixedUpdate()
    //{
    //    txtSpinID.text = $"{ GameManager.instance.spinScriptable.sorteioOrdem}º GIRO";
    //}

}
