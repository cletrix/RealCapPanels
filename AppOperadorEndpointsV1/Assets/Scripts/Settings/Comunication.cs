using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Comunication
{
    public async Task<string> Get(string url)
    {
        string response = string.Empty;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            var operation = webRequest.SendWebRequest();
            while (!operation.isDone)
                await Task.Delay(1000);

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    response = string.Empty;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    response = string.Empty;
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        response = webRequest.downloadHandler.text;
                       
                        break;
                    }
            }
        }
        return response;
    }

    public async Task<string> Post(string url,string json)
    {
        string response = string.Empty;
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            var operation = webRequest.SendWebRequest();
            while (!operation.isDone)
                await Task.Delay(1000);

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    response = string.Empty;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    response = string.Empty;
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        response = webRequest.downloadHandler.text;
                        break;
                    }
            }
        }
        return response;
    }

}
