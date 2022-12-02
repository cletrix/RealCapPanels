using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIInfosEdition : MonoBehaviour
{
    [Header("TECHNICAL INFOS")]
    [SerializeField] private TextMeshProUGUI technicalName;
    [SerializeField] private TextMeshProUGUI technicalCPF;

    [Header("EDITION INFOS")]
    [SerializeField] private TextMeshProUGUI editionName;
    [SerializeField] private TextMeshProUGUI editionNumber;
    [SerializeField] private TextMeshProUGUI editionDateRealization;
    [SerializeField] private TextMeshProUGUI editionNamePlan;
    [SerializeField] private TextMeshProUGUI editionProcessSUSEP;
    [SerializeField] private TextMeshProUGUI editionTradeName;
    [SerializeField] private TextMeshProUGUI editionTypeSizeSerie;
    [SerializeField] private TextMeshProUGUI editionModalities;
    [SerializeField] private TextMeshProUGUI editionTypeAmountChances;
    [SerializeField] private TextMeshProUGUI editionValue;

    public void ShowTechnicalInfos(string _technicalName, string _technicalCPF)
    {
        technicalName.text = $"Tecnico:{_technicalName}";
        technicalCPF.text = $"CPF: {_technicalCPF}";
    }
    public void ShowEditionInfos(string _nome, string _numero,string _dataRealizacao ,string _nomePlano, string _processoSUSEP, string _denominacaoComercial, int _tipoTamanhoSerie, int _modalidades, string _tipoQuantidadeChances, string _valor)
    {
        editionName.text =$"Nome da edição\n\n {_nome}";
        editionNumber.text = $"Número \n\n {_numero}";
        editionDateRealization.text= $"Data\n\n {_dataRealizacao}";
        editionNamePlan.text= $"Nome do Plano\n\n {_nomePlano}";
        editionProcessSUSEP.text= $"Processo SUSEP\n\n {_processoSUSEP}";
        editionTradeName.text= $"Denominação Comercial\n\n{_denominacaoComercial}";
        editionTypeSizeSerie.text= $"Tamanho Serie\n\n {_tipoTamanhoSerie}";
        editionModalities.text= $"Modalidades\n\n {_modalidades}";
        editionTypeAmountChances.text = $"chances\n\n {_tipoQuantidadeChances}";
        editionValue.text = $"Valor\n{GameManager.instance.FormatMoneyInfo(int.Parse(_valor), 2)}";
    }
}
