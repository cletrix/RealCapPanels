using System;
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
            public string matriz;
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
            GameManager.instance.globeDrawData.bolasSorteadas.Clear();
            GameManager.instance.operatorData.spinNumbers.Clear();
            foreach (var item in globe_balls)
            {
                GameManager.instance.globeDrawData.bolasSorteadas.Add(item.ToString());
            }

            for (int i = 0; i < number_winner_giro.Count; i++)
            {
                if (number_winner_giro[i].Length > 0)
                {
                    GameManager.instance.operatorData.spinNumbers.Add(number_winner_giro[i]);
                }
            }
        }
    }
    #endregion

    #region OperatorPayload
    
    [Serializable]
    public class OperatorData
    {
        public int currentSceneID;
        public int currentRaffle;
        public string currentGlobeDesc;
        public float currentGlobeValue;
        public int panelActive;
        public bool isVisibleRaffle;
        public int forTwoBalls;
        public List<GlobeDrawData.porUmaBola> forOneBalls = new List<GlobeDrawData.porUmaBola>();
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
        public void UpdateConfig(int sceneId, int _currentRaffle, bool raffleVisibility, int _forTwoBalls, List<GlobeDrawData.porUmaBola> _forOneBall,
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
            currentGlobeDesc = GameManager.instance.globeData.GetDescription();
            currentGlobeValue = GameManager.instance.globeData.GetValue();

            RestNetworkManager.instance.PostWriteMemory();
        }
        public void UpdateSpinConfig(string _spinNumber, TicketInfos _ticketSpin)
        {
            ticketSpin = _ticketSpin;
            if (!spinNumbers.Contains(_spinNumber))
                spinNumbers.Add(_spinNumber);
            spinIndex = spinNumbers.Count;
            RestNetworkManager.instance.PostWriteMemory();

        }
        public void PopulateConfig()
        {
            GameManager.instance.sceneId = currentSceneID;
            GameManager.instance.globeData.SetOrder(currentRaffle);
            GameManager.instance.globeData.SetDescription(currentGlobeDesc);
            GameManager.instance.globeData.SetValue(currentGlobeValue);
            GameManager.instance.isVisibleRaffle = isVisibleRaffle;
            GameManager.instance.globeDrawData.porDuasBolas = forTwoBalls;

            GameManager.instance.globeDrawData.IncreseForOneBalls(forOneBalls);

            GameManager.instance.globeDrawData.ganhadorContemplado = new TicketInfos[ticketInfos.Count];
            GameManager.instance.globeDrawData.ticketListVisible = new bool[ticketInfos.Count];
            GameManager.instance.isTicketVisible = isTicketVisible;
            GameManager.instance.ticketWinnerIndex = currentTicketIndex;


            if (GameManager.instance.sceneId == 1)
            {
                for (int i = 0; i < ticketInfos.Count; i++)
                {
                    GameManager.instance.globeDrawData.ganhadorContemplado[i] = ticketInfos[i];
                    GameManager.instance.globeDrawData.ticketListVisible[i] = ticketsShown[i];
                }
                GameManager.instance.RecoveryGlobeScreen();
            }
            if (GameManager.instance.sceneId == 2)
            {
                GameManager.instance.RecoverySpinScreen();
                GameManager.instance.spinData.sorteioOrdem = spinIndex;
                GameManager.instance.spinDrawData.ganhadorContemplado = ticketSpin;
            }
        }

    }
    #endregion

    #region GlobePayload

    [Serializable]
    public class GlobeData
    {
        public List<Infos> infosGlobe;
        public int currentIndex = 0;

        [System.Serializable]
        public class Infos
        {
            public string sorteioDescricao;
            public float sorteioValor;
            public int sorteioOrdem = 1;
        }

        public void SetOrder(int _order)
        {
            if (currentIndex < infosGlobe.Count)
                infosGlobe[currentIndex].sorteioOrdem = _order;
        }
        public int GetOrder()
        {
            if (infosGlobe.Count > 0)
            {
                if (currentIndex < infosGlobe.Count)
                    return infosGlobe[currentIndex].sorteioOrdem;
                else
                    return infosGlobe[currentIndex - 1].sorteioOrdem;
            }
            else
                return 999;
        }

        public void SetDescription(string _desc)
        {
            if (currentIndex < infosGlobe.Count)
                infosGlobe[currentIndex].sorteioDescricao = _desc;
        }
        public string GetDescription()
        {
            if (infosGlobe.Count > 0)
            {
                if (currentIndex < infosGlobe.Count)
                {
                    return infosGlobe[currentIndex].sorteioDescricao;
                }
                else
                    return infosGlobe[currentIndex - 1].sorteioDescricao;
            }
            else
                return "Null";
        }

        public void SetValue(float _value)
        {
            if (currentIndex < infosGlobe.Count)
                infosGlobe[currentIndex].sorteioValor = _value;
        }
        public float GetValue()
        {
            if (infosGlobe.Count > 0)
            {
                if (currentIndex < infosGlobe.Count)
                    return infosGlobe[currentIndex].sorteioValor;
                else
                    return infosGlobe[currentIndex - 1].sorteioValor;
            }
            else
                return 0f;
        }
    }
    #endregion

    #region GlobeDrawnPayload

    [Serializable]
    public class GlobeDrawData
    {
        public List<string> bolasSorteadas;
        public int porDuasBolas;
        public List<porUmaBola> porUmaBolas;
        public float valorPremio;
        [Header("Ticket Winner")]
        public TicketInfos[] ganhadorContemplado;
        public bool[] ticketListVisible;
        public void SetNewBall(string ball)
        {
            bolasSorteadas.Add(ball);
        }

        public void RevokeBall(string ball)
        {
            bolasSorteadas.Remove(ball);
        }

        public void IncreseForOneBalls(List<porUmaBola> _oneBalls)
        {
            porUmaBolas.Clear();
            for (int i = 0; i < _oneBalls.Count; i++)
            {
               porUmaBolas.Add(_oneBalls[i]);
            }
        }
        public void ResetInfos()
        {
            bolasSorteadas.Clear();
            ganhadorContemplado = new TicketInfos[0];
            ticketListVisible = new bool[0];
            porUmaBolas.Clear();
            valorPremio = 0;
            porDuasBolas = 0;
        }
        [System.Serializable]
        public class porUmaBola
        {
            public string numeroTitulo;
            public string numeroChance;
            public string numeroBola;
        }
    }
    #endregion

    #region SpinDataPayload

    [Serializable]
    public class SpinData
    {
        public int sorteioOrdem;
        public string sorteioDescricao;
        public float sorteioValor;

        public void SetSpinOrder(int order)
        {
            sorteioOrdem = order; 
        }


    }
    #endregion

    #region SpinDrawnPayload

    [Serializable]
    public class SpinDrawData
    {
        public string numeroSorteado;
        public int sorteioOrdem = 1;
        [Header("Ticket Winner")]
        public TicketInfos ganhadorContemplado;

        public void SetNewRaffleNumber()
        {
            numeroSorteado = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                int random = UnityEngine.Random.Range(0, 9);
                numeroSorteado += random.ToString();
            }
            ganhadorContemplado.numeroSorte = numeroSorteado;
            ganhadorContemplado.numeroTitulo = $"0{numeroSorteado}";
        }
    }
    #endregion


}