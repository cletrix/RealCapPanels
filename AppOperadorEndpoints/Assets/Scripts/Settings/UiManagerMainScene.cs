using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiManagerMainScene : MonoBehaviour
{
    [Header("UI INFOS")]
    [SerializeField] private UIInfosEdition uiInfosEdition;
    [SerializeField] private LotteryController lotteryController;
    void Start()
    {
        
        lotteryController.PopulateLotteryFederalExtractions(
            GameManager.instance.lotteryScriptable.resultadoLoteriaFederalPrimeiroSorteio,
            GameManager.instance.lotteryScriptable.resultadoLoteriaFederalSegundoSorteio,
            GameManager.instance.lotteryScriptable.resultadoLoteriaFederalTerceiroSorteio,
            GameManager.instance.lotteryScriptable.resultadoLoteriaFederalQuartoSorteio,
            GameManager.instance.lotteryScriptable.resultadoLoteriaFederalQuintoSorteio
            );
        uiInfosEdition.ShowTechnicalInfos(GameManager.instance.globalScriptable.tecnicoNome, GameManager.instance.globalScriptable.tecnicoCPF);

    }
    private void OnEnable()
    {
        UiInfosRaffle.OnActiveEditionInfos +=PopulateEditionInfos;
    }
    private void OnDisable()
    {
        UiInfosRaffle.OnActiveEditionInfos -= PopulateEditionInfos;
    }
    private void PopulateEditionInfos()
    {
        uiInfosEdition.ShowEditionInfos(
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].nome,
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].numero,
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].nomePlano,
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].processoSUSEP,
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].denominacaoComercial,
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].tipoTamanhoSerie,
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].modalidades,
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].tipoQuantidadeChances,
            GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].valor
            );
    }
}
