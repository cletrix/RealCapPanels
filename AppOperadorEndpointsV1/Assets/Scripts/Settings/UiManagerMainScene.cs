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
        uiInfosEdition.ShowTechnicalInfos(GameManager.instance.editonData.tecnicoNome, GameManager.instance.editonData.tecnicoCPF);
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
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].nome,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].numero,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].dataRealizacao,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].nomePlano,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].processoSUSEP,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].denominacaoComercial,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].tipoTamanhoSerie,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].modalidades,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].tipoQuantidadeChances,
            GameManager.instance.editonData.edicaoInfos[GameManager.instance.EditionIndex].valor
            );
    }
}
