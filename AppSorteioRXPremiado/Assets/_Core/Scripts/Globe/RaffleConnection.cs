//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;
//using UnityEngine.Networking;

//public class RaffleConnection : MonoBehaviour
//{
//    #region Singleton
//    private static RaffleConnection _instance;

//    public static RaffleConnection instance { get { return _instance; } }

//    private void Awake()
//    {
//        if (_instance != null && _instance != this)
//        {
//            Destroy(this.gameObject);
//        }
//        else
//        {
//            _instance = this;
//        }


//    }
//    #endregion

//    [Header("RESTORE")]
//    public List<int> restoreNumbers;
//    [Header("COMPONENTS")]
//    public BallController ballController;
//    public List<LastBallRaffle.TicketInfos> ticketsID;
//    public int indexPossibleWinners;

//    [Header("RESPONSES")]
//    public JsonRaffleResponse raffleDataResponse;
//    public JsonSelectRaffleResponse selectRaffleResponse;


//    [Header("URLS")]

//    public string baseURL = "http://dev3.blanescorp.com:3000/";
//    public string urlSelectRaffle = "selecionarSorteio";
//    public string urlGetBallRaffle = "sorteiosjson";
//    public string urlRaffle = "sorteiosphp";

//    private void Start()
//    {
//        // StartCoroutine(GetLastBallsRaffled(0.1f));
//        indexPossibleWinners = 10;
//        //RaffleController.instance.ActiveFade();

//    }

//    public void PlaySorteio()
//    {
//        GameManager.instance.isActiveRaffle = true;
//        //StartCoroutine(GetBallRaffle(0.1f));
//    }
//    void SpliStringToList(string text)
//    {
//        restoreNumbers.Clear();
//        char[] elements = new char[] { '-', ' ' };

//        string[] stringArray = text.Split(elements);

//        for (int i = 0; i < stringArray.Length - 1; i++)
//        {
//            restoreNumbers.Add(System.Convert.ToInt32(stringArray[i]));
//        }
//        ballController.GetLastIndexBallsRaffle();
//    }

//    public void CallRestartScene()
//    {
//        GameManager.instance.isActiveRaffle = false;
//        if (GameManager.instance.roundID < GameManager.instance.raffleDataResponse.dados[GameManager.instance.raffleIndex].sorteio_quantidade_rodadas)
//            RaffleController.instance.LoadScene("Raffle", true);
//        else
//            RaffleController.instance.LoadScene("LuckySpin", true);
//    }

//    //public IEnumerator GetLastBallsRaffled(float time)
//    //{
//    //    yield return new WaitForSeconds(time);
//    //    JsonSelectRaffleRequest data = new JsonSelectRaffleRequest();

//    //    data.cok = GameManager.instance.loginData.dados[0].cookie;
//    //    data.cliente_id = GameManager.instance.clientID;
//    //    data.sala_id = GameManager.instance.roomID;
//    //    data.sorteio_id = GameManager.instance.raffleID;

//    //    string json = JsonUtility.ToJson(data);
//    //    UnityWebRequest request = UnityWebRequest.Post(baseURL + urlSelectRaffle, json);
//    //    byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
//    //    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
//    //    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
//    //    request.SetRequestHeader("Content-Type", "application/json");
//    //    yield return request.SendWebRequest();
//    //    if (request.responseCode == 200)
//    //    {
//    //        string response = request.downloadHandler.text;
//    //        print("RESPONSE = " + response);
//    //        selectRaffleResponse = JsonUtility.FromJson<JsonSelectRaffleResponse>(response);

//    //        if (selectRaffleResponse.dados[0].rodada_id >= GameManager.instance.currentRaffle.sorteio_quantidade_rodadas && selectRaffleResponse.dados[0].rodada_final_data != string.Empty)
//    //        {
//    //            RaffleController.instance.LoadScene("LuckySpin", false);
//    //        }
//    //        else
//    //        {
//    //            if (selectRaffleResponse.dados[0].esperando_ultimo_num > 0)
//    //            {
//    //                RaffleController.instance.ActiveAnimHeart();
//    //            }
//    //            RaffleController.instance.ActiveFade();
//    //            SpliStringToList(selectRaffleResponse.dados[0].rodada_numeros);
//    //            ballController.lastBallRaffle.SetInfoWaitLastBall(RaffleConnection.instance.selectRaffleResponse.dados[0].esperando_ultimo_num);

//    //            ballController.totalBallsCount.SetInfoTotalBall(selectRaffleResponse.dados[0].rodada_quantidade_sorteados.ToString());
//    //            RaffleController.instance.countBallsRaffle = selectRaffleResponse.dados[0].rodada_quantidade_sorteados;

//    //            ballController.indexNumberBall = selectRaffleResponse.dados[0].rodada_quantidade_sorteados - 1;
//    //            GameManager.instance.roundID = selectRaffleResponse.dados[0].rodada_id;
//    //        }
//    //    }
//    //    else
//    //    {
//    //        print("ResponseCode" + request.responseCode);
//    //        print(request.error);
//    //        StartCoroutine(GetLastBallsRaffled(0.1f));
//    //    }
//    //}

//    //public IEnumerator GetBallRaffle(float time)
//    //{
//    //    yield return new WaitForSeconds(time);
//    //    JsonRequest data = new JsonRequest();

//    //    data.funcao = "iniciar";
//    //    data.cok = GameManager.instance.loginData.dados[0].cookie;
//    //    data.cliente_id = GameManager.instance.clientID;
//    //    data.sala_id = GameManager.instance.roomID;
//    //    data.sorteio_id = GameManager.instance.raffleID;
//    //    data.rodada_id = GameManager.instance.roundID;

//    //    string json = JsonUtility.ToJson(data);

//    //    var request = new UnityWebRequest(
//    //        baseURL + urlGetBallRaffle, "POST");
//    //    byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
//    //    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
//    //    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
//    //    request.SetRequestHeader("Content-Type", "application/json");
//    //    yield return request.SendWebRequest();
//    //    if (request.responseCode == 200)
//    //    {
//    //        string response = request.downloadHandler.text;

//    //        raffleDataResponse = JsonUtility.FromJson<JsonRaffleResponse>(response);

//    //        if (raffleDataResponse.dados[0].sorteio_ganhadores.Count > 0)
//    //        {
//    //            RaffleController.instance.isWinner = true;
//    //            GameManager.instance.isActiveRaffle = false;
//    //            ballController.ActiveticketWinner();
//    //            // ShowPossiblesWinners();
//    //            Invoke("CallRestartScene", 10);
//    //        }
//    //        else
//    //        {
//    //            if (raffleDataResponse.dados[0].tickets_numeros.Count > 0)
//    //            {
//    //                RaffleController.instance.ActiveAnimHeart();
//    //            }
//    //            ShowNewBall();
//    //            StartCoroutine(GetBallRaffle(0.1f));
//    //        }

//    //    }
//    //    else
//    //    {
//    //        print(request.error);
//    //        StartCoroutine(GetBallRaffle(0.1f));
//    //    }
//    //}

//    //void ShowPosiblesWinnersWithWinner()
//    //{
//    //    if (raffleDataResponse.dados[0].tickets_numeros.Count > 0)
//    //    {
//    //        LastBallRaffle.TicketInfos info = new LastBallRaffle.TicketInfos();

//    //        if (raffleDataResponse.dados[0].sorteio_ganhadores.Count > 0)
//    //        {
//    //            for (int i = 0; i < raffleDataResponse.dados[0].sorteio_ganhadores.Count; i++)
//    //            {
//    //                info.ticketID = raffleDataResponse.dados[0].tickets_numeros[i].Substring(0, 11);
//    //                info.colorText = Color.yellow;
//    //                ticketsID.Add(info);
//    //                indexPossibleWinners--;
//    //                print("Ativou Winners");
//    //            }
//    //        }
//    //        else
//    //        {
//    //            print("Ativou NO WINNERS");
//    //        }
//    //        ballController.PopulateInfoTicketPossibleWinners(ticketsID);
//    //    }

//    //}
//    //void ShowPossiblesWinners()
//    //{
//    //    if (raffleDataResponse.dados[0].tickets_numeros.Count > 0)
//    //    {
//    //        indexPossibleWinners = 10;
//    //        LastBallRaffle.TicketInfos info = new LastBallRaffle.TicketInfos();
//    //        if (raffleDataResponse.dados[0].sorteio_ganhadores.Count > 0)
//    //        {
//    //            for (int i = 0; i < raffleDataResponse.dados[0].sorteio_ganhadores.Count; i++)
//    //            {
//    //                info.ticketID = raffleDataResponse.dados[0].tickets_numeros[i].Substring(0, 11);
//    //                info.colorText = Color.white;
//    //                ticketsID.Add(info);
//    //                indexPossibleWinners--;
//    //            }
//    //            // print("PassouWinner");
//    //        }
//    //        else
//    //        {
//    //            for (int i = 0; i < indexPossibleWinners; i++)
//    //            {
//    //                if (raffleDataResponse.dados[0].tickets_numeros.Count > 0)
//    //                {
//    //                    info.ticketID = "111111";//raffleDataResponse.dados[0].tickets_numeros[i].Substring(0, 11);
//    //                    info.colorText = Color.white;
//    //                    ticketsID.Add(info);
//    //                }
//    //            }
//    //            print("Passou Not Winner");
//    //        }
//    //    }
//    //    ballController.PopulateInfoTicketPossibleWinners(ticketsID);
//    //}

//    public void PopulateTickets()
//    {
//        for (int i = 0; i < indexPossibleWinners; i++)
//        {
//            if (raffleDataResponse.dados[0].tickets_numeros.Count > 0)
//            {
//                LastBallRaffle.TicketInfos info = new LastBallRaffle.TicketInfos();
//                info.ticketID = "111111";//raffleDataResponse.dados[0].tickets_numeros[i].Substring(0, 11);
//                info.colorText = Color.white;
//                ticketsID.Add(info);
//            }
//        }
//        ballController.PopulateInfoTicketPossibleWinners(ticketsID);
//    }
//    //void ShowNewBall()
//    //{
//    //    RaffleController.instance.countBallsRaffle = raffleDataResponse.dados[0].bolas_sorteadas.Count;
//    //    ballController.totalBallsCount.SetInfoTotalBall(raffleDataResponse.dados[0].bolas_sorteadas.Count.ToString());
//    //    if (GameManager.instance.isActiveRaffle)
//    //    {
//    //        if (ballController.indexNumberBall < raffleDataResponse.dados[0].bolas_sorteadas.Count - 1)
//    //        {
//    //            ballController.indexNumberBall = raffleDataResponse.dados[0].bolas_sorteadas.Count - 1;
//    //            ballController.ShowBigBall();
//    //            ticketsID.Clear();
//    //            ShowPossiblesWinners();
//    //        }
//    //    }

//    //}
//    [System.Serializable]
//    public class JsonSelectRaffleRequest
//    {
//        public string funcao;
//        public string cok;
//        public int cliente_id;
//        public int sala_id;
//        public int sorteio_id;
//    }
//    [System.Serializable]
//    public class JsonSelectRaffleResponse
//    {
//        public int erro;
//        public string mensagem;
//        public int registros;
//        public JsonSelectRaffleData[] dados;
//    }
//    [System.Serializable]
//    public class JsonSelectRaffleData
//    {
//        public int rodada_id;
//        public string rodada_numeros;
//        public string rodada_final_data;
//        public int rodada_quantidade_sorteados;
//        public int esperando_ultimo_num;
//    }

//    [System.Serializable]
//    public class JsonRequest
//    {
//        public string funcao;
//        public string cok;
//        public int cliente_id;
//        public int sorteio_id;
//        public int sala_id;
//        public int rodada_id;
//    }
//    [System.Serializable]
//    public class JsonRaffleResponse
//    {
//        public int erro;
//        public string mensagem;
//        public int registros;
//        public JsonData[] dados;
//    }
//    [System.Serializable]
//    public class JsonData
//    {
//        public List<int> bolas_sorteadas;
//        public List<JsonGanhadores> sorteio_ganhadores;

//        public List<string> tickets_numeros;

//    }

//    [System.Serializable]
//    public class JsonGanhadores
//    {
//        public string comprador_nome;
//        public int comprador_id;
//        public string comprador_cpf;
//        public string comprador_endereco_logradouro;
//        public int comprador_endereco_numero;
//        public string comprador_endereco_complemento;
//        public string comprador_endereco_bairro;
//        public string comprador_endereco_cep;
//        public string comprador_endereco_cidade;
//        public string comprador_endereco_uf;
//        public string comprador_nascimento;
//        public string comprador_telefone;
//        public string comprador_email;
//        public string ticket_ganhador;
//    }
//}
