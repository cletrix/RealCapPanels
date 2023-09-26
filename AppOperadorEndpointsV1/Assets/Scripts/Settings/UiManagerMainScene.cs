using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiManagerMainScene : MonoBehaviour
{
    [Header("UI INFOS")]
    [SerializeField] private UIInfosEdition uiInfosEdition;
    void Start()
    {
        uiInfosEdition.ShowTechnicalInfos(GameManager.instance.editionData.tecnicoNome, GameManager.instance.editionData.tecnicoCPF);
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
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].nome,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].numero,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].nomePlano,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].processoSUSEP,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].denominacaoComercial,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].tipoTamanhoSerie,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].modalidades,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].tipoQuantidadeChances,
            GameManager.instance.editionData.edicaoInfos[GameManager.instance.EditionIndex].valor
            );
    }
}
