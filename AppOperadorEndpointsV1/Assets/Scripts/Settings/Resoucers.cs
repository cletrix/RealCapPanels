using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resoucers
{
    #region LoginPlayload

    [Serializable]
    public class EditionData
    {
        public string tecnicoNome;
        public string tecnicoCPF;
        [Space]
        public List<edicaoInfo> edicaoInfos;

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
            public float valor;
            public int status;
        }
        public void SetInfosTecnical(string _tecnicoName, string _tecnicoCPF)
        {
            tecnicoNome = $"{_tecnicoName}";
            tecnicoCPF = $"{_tecnicoCPF}";
        }

    }
    #endregion

    #region RecoverySystemPayload

    [Serializable]
    public class RecoveryData
    {
        public int limit_globo;
        public int limit_spin;
        public string sorteio_tipo;
        public int sorteio_globo_numero;
        public int sorteio_spin_numero;
        public List<int> globe_balls = new List<int>();
        public List<int> globe_partial_result = new List<int>();
        public List<string> titulo_winner_giro = new List<string>();
        public List<string> number_winner_giro = new List<string>();

        public List<List<int>> titulo_winner_globe = new List<List<int>>();
        public List<List<int>> card_winner_globe = new List<List<int>>();
        public List<List<int>> winner_globo_balls = new List<List<int>>();


        public void ResetInfos()
        {
            limit_globo = 0;
            limit_spin = 0;
            sorteio_tipo = string.Empty;
            sorteio_globo_numero = 0;
            sorteio_spin_numero = 0;
            globe_balls.Clear();
            globe_partial_result.Clear();
            titulo_winner_giro.Clear();
            number_winner_giro.Clear();
            titulo_winner_globe.Clear();
            card_winner_globe.Clear();
            winner_globo_balls.Clear();
        }
        public void UpdateInfosGlobe()
        {
            GameManager.instance.globeDraw.bolasSorteadas.Clear();
            GameManager.instance.OperatorData.spinNumbers.Clear();
            foreach (var item in globe_balls)
            {
                GameManager.instance.globeDraw.bolasSorteadas.Add(item.ToString());
            }

            for (int i = 0; i < number_winner_giro.Count; i++)
            {
                if (number_winner_giro[i].Length > 0)
                {
                    GameManager.instance.OperatorData.spinNumbers.Add(number_winner_giro[i]);
                }
            }
        }
    }
    #endregion

    #region OperatorPayload
    public class OperatorData
    {
        public int currentSceneID;
        public int currentRaffle;
        public string currentGlobeDesc;
        public float currentGlobeValue;
        public int panelActive;
        public bool isVisibleRaffle;
        public int forTwoBalls;
        public List<GlobeRaffleScriptable.porUmaBola> forOneBalls = new List<GlobeRaffleScriptable.porUmaBola>();
        public List<TicketInfos> ticketInfos;

        public List<bool> ticketsShown;
        public int currentTicketIndex;
        public bool isTicketVisible;

        public List<string> spinNumbers;
        public int spinIndex = 1;
        public TicketInfos ticketSpin;

        public void ResetInfos()
        {
            currentSceneID = 0;
            currentSceneID = 1;
            panelActive = 0;
            currentTicketIndex = 0;
            forTwoBalls = 0;
            isVisibleRaffle = false;
            isTicketVisible = false;
            forOneBalls.Clear();
            ticketInfos.Clear();

            spinIndex = 1;
            spinNumbers.Clear();
        }
        public void UpdateConfig(int sceneId, int _currentRaffle, bool raffleVisibility, int _forTwoBalls, List<GlobeRaffleScriptable.porUmaBola> _forOneBall,
            List<TicketInfos> _tickets, List<bool> _ticketsShown, int _currentTicketIndex, bool _isTicketVisible)
        {
            currentSceneID = sceneId;
            isVisibleRaffle = raffleVisibility;
            forTwoBalls = _forTwoBalls;
            forOneBalls = _forOneBall;

            ticketInfos.Clear();
            ticketInfos.AddRange(_tickets);
            ticketsShown.Clear();
            ticketsShown.AddRange(_ticketsShown);
            currentTicketIndex = _currentTicketIndex;
            isTicketVisible = _isTicketVisible;
            currentRaffle = _currentRaffle;
            currentGlobeDesc = GameManager.instance.globeScriptable.GetGlobeDescription();
            currentGlobeValue = GameManager.instance.globeScriptable.GetGlobeValue();

            RestNetworkManager.instance.CallWriteMemory();
        }
        public void UpdateSpinConfig(string _spinNumber, TicketInfos _ticketSpin)
        {
            ticketSpin = _ticketSpin;
            if (!spinNumbers.Contains(_spinNumber))
                spinNumbers.Add(_spinNumber);
            spinIndex = spinNumbers.Count;
            RestNetworkManager.instance.CallWriteMemory();

        }
        public void PopulateConfig()
        {
            GameManager.instance.sceneId = currentSceneID;
            GameManager.instance.globeScriptable.SetGlobeOrder(currentRaffle);
            GameManager.instance.globeScriptable.SetGlobeDesc(currentGlobeDesc);
            GameManager.instance.globeScriptable.SetGlobeValue(currentGlobeValue);
            GameManager.instance.isVisibleRaffle = isVisibleRaffle;
            GameManager.instance.globeDraw.porDuasBolas = forTwoBalls;

            GameManager.instance.globeDraw.IncreseForOneBalls(forOneBalls);

            GameManager.instance.globeDraw.ganhadorContemplado = new TicketInfos[ticketInfos.Count];
            GameManager.instance.globeDraw.ticketListVisible = new bool[ticketInfos.Count];
            GameManager.instance.isTicketVisible = isTicketVisible;
            GameManager.instance.ticketWinnerIndex = currentTicketIndex;


            if (GameManager.instance.sceneId == 2)
            {
                for (int i = 0; i < ticketInfos.Count; i++)
                {
                    GameManager.instance.globeDraw.ganhadorContemplado[i] = ticketInfos[i];
                    GameManager.instance.globeDraw.ticketListVisible[i] = ticketsShown[i];
                }
                GameManager.instance.RecoveryGlobeScreen();
            }
            if (GameManager.instance.sceneId == 3)
            {
                GameManager.instance.RecoverySpinScreen();
                GameManager.instance.spinScriptable.sorteioOrdem = spinIndex;
                GameManager.instance.spinResultScriptable.ganhadorContemplado = ticketSpin;
            }
        }

    }
    #endregion
}