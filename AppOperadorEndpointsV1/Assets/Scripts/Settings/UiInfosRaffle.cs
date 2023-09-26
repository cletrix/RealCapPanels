using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UiInfosRaffle : MonoBehaviour
{
    [SerializeField] private Button btEditionInfos;
    [SerializeField] private GameObject panelEditionInfos;
    public static event Action OnActiveEditionInfos;

    [Header("Raffle Infos")]
    [SerializeField] private TextMeshProUGUI txtOrder;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private TextMeshProUGUI txtValue;

    void Start()
    {
        InitializeVariables();
    }

    void InitializeVariables()
    {
        string name = $"Informações da edição: {GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].numero}";
        btEditionInfos.GetComponentInChildren<TextMeshProUGUI>().text = name;
        btEditionInfos.onClick.AddListener(ActivePanelEditionInfos);
    }
 
    public void PopulateRaffleInfos(string _order, string _description, float _value)
    {
        txtOrder.text = $"Ordem\n{_order}"; 
        txtDescription.text = $"Descrição\n{_description}";
        txtValue.text = $"Valor líquido\n {GameManager.instance.FormatMoneyInfo(_value, 2)}";
    }
    public void UpdateOrder(string _order)
    {
        txtOrder.text = $"Ordem\n{_order}";
    }

    public void ActivePanelEditionInfos()
    {
        panelEditionInfos.SetActive(true);
        OnActiveEditionInfos?.Invoke();
    }
}
