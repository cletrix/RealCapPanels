using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "Settings/GlobalSettings")]
public class GlobalScriptable : ScriptableObject
{
    public bool canRecoveryRaffleGlobe;
    public string tecnicoNome;
    public string tecnicoCPF;
    [Space]
    public List<edicaoInfo> edicaoInfos;

    public void SetInfosTecnical(string _tecnicoName, string _tecnicoCPF)
    {
        tecnicoNome = $"Tecnico: {_tecnicoName}";
        tecnicoCPF = $"Tecnico: {_tecnicoCPF}";
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
