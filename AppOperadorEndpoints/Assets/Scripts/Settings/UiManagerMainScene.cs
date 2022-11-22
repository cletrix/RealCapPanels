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
        uiInfosEdition.ShowTechnicalInfos(GameManager.instance.editionScriptable.tecnicoNome, GameManager.instance.editionScriptable.tecnicoCPF);

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
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].nome,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].numero,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].nomePlano,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].processoSUSEP,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].denominacaoComercial,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].tipoTamanhoSerie,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].modalidades,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].tipoQuantidadeChances,
            GameManager.instance.editionScriptable.edicaoInfos[GameManager.instance.EditionIndex].valor
            );
    }
}
