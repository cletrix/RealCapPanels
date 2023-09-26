using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RiptideNetworking;

public class SpinController : MonoBehaviour
{
    [Header("SPIN RAFFLE")]
    [SerializeField] private SpinRaffleData spinRaffleData;
    [SerializeField] private Transform content;
    [SerializeField] private List<SpinRaffleData> spinRaffleDatas = new List<SpinRaffleData>();
    [SerializeField] private List<TextMeshProUGUI> NumberRaffle = new List<TextMeshProUGUI>();
    [SerializeField] private TextMeshProUGUI txtSpinID;
    [SerializeField] public int indexSpin;
    [SerializeField] private string currentNumberRaffled;

    [Header("BUTTONS")]
    public Button btGenerateLuckyNumber;

    [Header("REFERENCES")]
    [SerializeField] private GameObject contentScrollView;
    [SerializeField] private GameObject groupNumberSpinRaffle;
    [SerializeField] private Scrollbar verticalScrollbar;
    [SerializeField] private TicketController ticketController;

    void Start()
    {
        PopulateSpinDatas(GameManager.instance.recoveryData.limit_spin);
        InitializeVariables();
    }

    public void PopulateSpinDatas(int _spinAmout)
    {
        spinRaffleDatas.Clear();

        for (int i = 0; i < _spinAmout; i++)
        {
            SpinRaffleData inst = Instantiate(spinRaffleData, transform.position, Quaternion.identity);
            inst.transform.SetParent(content);
            inst.transform.localScale = Vector3.one;
            spinRaffleDatas.Add(inst);
        }
    }
    private void InitializeVariables()
    {
        contentScrollView = GameObject.Find("Content");
        groupNumberSpinRaffle = GameObject.Find("GroupNumberSpinRaffle");

        ticketController = FindObjectOfType<TicketController>();

        verticalScrollbar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        txtSpinID = GameObject.Find("TxtSpinID").GetComponent<TextMeshProUGUI>();

        NumberRaffle.Clear();
        NumberRaffle.AddRange(groupNumberSpinRaffle.GetComponentsInChildren<TextMeshProUGUI>());


        foreach (var item in spinRaffleDatas)
        {
            item.InitializeVariables();
            item.SetEventButton(ShowTicketSpin);
        }
        SetButtonsEvent();
        ShowSpinOrder(GameManager.instance.spinData.sorteioOrdem);
        SetSpinPrize();
        PopulateFieldsSpinData();
        UpdateFieldScreen();
        btGenerateLuckyNumber.interactable = false;
    }
    public void SetIndexSpin(int spinOrder)
    {
        indexSpin = spinOrder;
    }
    public void ShowSpinOrder(int order)
    {
        UiInfosRaffle uiInfos = FindObjectOfType<UiInfosRaffle>();

        txtSpinID.text = $"{order}º GIRO";
        uiInfos.PopulateRaffleInfos(order.ToString(), GameManager.instance.spinData.sorteioDescricao, GameManager.instance.spinData.sorteioValor);
    }
    private void SetSpinPrize()
    {
        for (int i = 0; i < spinRaffleDatas.Count; i++)
        {
            spinRaffleDatas[i].SetSpinPrize(GameManager.instance.spinData.sorteioValor);
            spinRaffleDatas[i].SetStateInteractableButton(false);
        }
    }

    private void UpdateFieldScreen()
    {
        for (int i = 0; i < spinRaffleDatas.Count; i++)
        {
            if (i != indexSpin - 1)
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
        RestNetworkManager.instance.SetPostResultSpin(GameManager.instance.spinData.sorteioOrdem);

        btGenerateLuckyNumber.interactable = false;
    }

    public void SetActiveBtGenerateSpin(bool _isActive)
    {
        if (!GameManager.instance.isBackup)
        {
            if (GameManager.instance.recoveryData.sorteio_tipo == "finish" || GameManager.instance.isVisibleRaffle==false)
                btGenerateLuckyNumber.interactable = false;
            else
                btGenerateLuckyNumber.interactable = _isActive;
        }
        else
            btGenerateLuckyNumber.interactable = false;
    }
    public void ShowNumberLuckySpin()
    {
        StartCoroutine(RaffleNumberLuckySpin());
    }
    private IEnumerator RaffleNumberLuckySpin()
    {
        SendMessageToClientSpinNumber(GameManager.instance.spinDrawData.numeroSorteado);
        SendMessageRoundID(GameManager.instance.spinData.sorteioOrdem);
        currentNumberRaffled = GameManager.instance.spinDrawData.numeroSorteado;
        GameManager.instance.operatorData.UpdateSpinConfig(currentNumberRaffled, GameManager.instance.spinDrawData.ganhadorContemplado);
        GameManager.instance.spinData.sorteioOrdem = GameManager.instance.operatorData.spinIndex;
        indexSpin = GameManager.instance.spinData.sorteioOrdem;
        UpdateFieldScreen();

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

        spinRaffleDatas[indexSpin - 1].SetNumberTicket(currentNumberRaffled);
        yield return new WaitForSeconds(0.1f);
        spinRaffleDatas[indexSpin - 1].SetStateInteractableButton(true);
        spinRaffleDatas[indexSpin - 1].SetStateFinishedRaffle(true);


    }
    public void ActiveLastSpin()
    {
        spinRaffleDatas[GameManager.instance.spinData.sorteioOrdem - 1].SetStateInteractableButton(true);
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
            if (GameManager.instance.recoveryData.sorteio_tipo == "finish")
                btGenerateLuckyNumber.interactable = false;
            else
                btGenerateLuckyNumber.interactable = true;
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
            GameManager.instance.spinDrawData.ganhadorContemplado.nome,
            GameManager.instance.spinDrawData.ganhadorContemplado.cpf,
            GameManager.instance.spinDrawData.ganhadorContemplado.dataNascimento,
            GameManager.instance.spinDrawData.ganhadorContemplado.telefone,
            GameManager.instance.spinDrawData.ganhadorContemplado.email,
            GameManager.instance.spinDrawData.ganhadorContemplado.bairro,
            GameManager.instance.spinDrawData.ganhadorContemplado.municipio,
            GameManager.instance.spinDrawData.ganhadorContemplado.estado,
            GameManager.instance.spinDrawData.ganhadorContemplado.dataSorteio,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].numero,
            GameManager.instance.spinDrawData.ganhadorContemplado.valor,
            GameManager.instance.spinDrawData.ganhadorContemplado.PDV,
            GameManager.instance.spinDrawData.ganhadorContemplado.bairoPDV,
            GameManager.instance.spinDrawData.ganhadorContemplado.dataCompra,
            GameManager.instance.spinDrawData.ganhadorContemplado.horaCompra,
            GameManager.instance.spinDrawData.ganhadorContemplado.numeroTitulo,
            GameManager.instance.spinDrawData.ganhadorContemplado.chance,
            GameManager.instance.spinDrawData.ganhadorContemplado.numeroCartela,
            GameManager.instance.spinDrawData.ganhadorContemplado.numeroSorte,
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
    //    txtSpinID.text = $"{ GameManager.instance.spinData.sorteioOrdem}º GIRO";
    //}

}
