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
    public void SetIndexSpin(int index)
    {
        indexSpin = index;
        SetSpinID();
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
        txtSpinID.text = $"{GameManager.instance.spinScriptable.sorteioOrdem}º GIRO";
        SetSpinPrize();
        PopulateFieldsSpinData();
    }

    private void SetSpinPrize()
    {
        for (int i = 0; i < spinRaffleDatas.Count; i++)
        {
            spinRaffleDatas[i].SetSpinPrize(GameManager.instance.spinScriptable.sorteioValor);
        }
    }
    private void SetSpinID()
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
        indexSpin++;
        GameManager.instance.spinScriptable.SetNewOrder(indexSpin);
        RestNetworkManager.instance.SetPostResultSpin(indexSpin);
        btGenerateLuckyNumber.interactable = false;
    }
    public void ShowNumberLuckySpin()
    {
        StartCoroutine(RaffleNumberLuckySpin());
    }
    private IEnumerator RaffleNumberLuckySpin()
    {
        SetSpinID();
        txtSpinID.text = $"{indexSpin}º GIRO";
        int roundID = indexSpin;
        GameManager.instance.spinScriptable.sorteioOrdem = indexSpin;
        UiInfosRaffle uiInfos = FindObjectOfType<UiInfosRaffle>();
        uiInfos.PopulateRaffleInfos(GameManager.instance.spinScriptable.sorteioOrdem.ToString(), GameManager.instance.spinScriptable.sorteioDescricao, GameManager.instance.spinScriptable.sorteioValor);
        SendMessageRoundID(roundID);

       // currentNumberRaffled = string.Empty;
        foreach (var item in NumberRaffle)
        {
            item.text = "0";
        }
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < NumberRaffle.Count; i++)
        {
            NumberRaffle[i].text = GameManager.instance.spinResultScriptable.numeroSorteado[i].ToString();
           
        }
        //currentNumberRaffled = GameManager.instance.spinResultScriptable.numeroSorteado;
        spinRaffleDatas[indexSpin - 1].SetNumberTicket(GameManager.instance.spinResultScriptable.numeroSorteado);

        yield return new WaitForSeconds(0.1f);
        SendMessageToClientSpinNumber(GameManager.instance.spinResultScriptable.numeroSorteado);
        spinRaffleDatas[indexSpin - 1].SetStateInteractableButton(true);
        spinRaffleDatas[indexSpin - 1].SetStateFinishedRaffle(true);


    }
    public void PopulateSpinsFields(List<string> spinNumbers)
    {
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
        if (btGenerateLuckyNumber.gameObject.activeSelf)
            btGenerateLuckyNumber.interactable = true;
    }
    public void ShowTicketSpin()
    {
        ticketController.SetTicketVisibility();
        PopulateTicketSpin();
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

}
