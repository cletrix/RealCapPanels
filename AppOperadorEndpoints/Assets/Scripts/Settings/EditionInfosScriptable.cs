using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EditionInfos", menuName = "Settings/EditionInfos")]
public class EditionInfosScriptable : ScriptableObject
{
    public string tecnicoNome;
    public string tecnicoCPF;
    [Space]
    public List<edicaoInfo> edicaoInfos;

    public void SetInfosTecnical(string _tecnicoName, string _tecnicoCPF)
    {
        tecnicoNome = $"{_tecnicoName}";
        tecnicoCPF = $"{_tecnicoCPF}";
    }

    [System.Serializable]
    public class edicaoInfo
    {
        public string nome;
        public int iD;
        public string numero;
        public string dataRealizacao;
        public string nomePlano;
        public string processoSUSEP;
        public string denominacaoComercial;
        public int tipoTamanhoSerie;
        public int modalidades;
        public string tipoQuantidadeChances;
        public string valor;
        public int status;
    }
}
