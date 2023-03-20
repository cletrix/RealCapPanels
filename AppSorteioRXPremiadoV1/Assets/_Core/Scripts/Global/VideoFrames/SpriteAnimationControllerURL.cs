using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Text.RegularExpressions;

public class SpriteAnimationControllerURL : MonoBehaviour
{
    public enum TypeAnim
    {
        Entrada = 1,
        Loop = 2,
        Saida = 3
    }
    public TypeAnim typeAnim = TypeAnim.Entrada;

    [SerializeField] private string url;
    [Space]
    [SerializeField] private AnimationsSettings lotteryAnimSettings;
    [SerializeField] private AnimationsSettings globeAnimSettings;
    [SerializeField] private AnimationsSettings luckySpinAnimSettings;
    [SerializeField] private RawImage frameVideo;

    Texture2D[] textList;
    private const int ignoreIndex = 2;

    [HideInInspector] public List<String> stageOne;
    public static SpriteAnimationControllerURL instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
  
    private void Start()
    {
        GetStageOneFolders(url);
    }
    private void GetStageOneFolders(string url)
    {
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        Regex regex = new Regex("<a href=\".*\">(?<name>.*)</a>");

        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            string result = reader.ReadToEnd();

            MatchCollection matches = regex.Matches(result);
            if (matches.Count == 0)
            {
                Console.WriteLine("parse failed.");
                return;
            }
            for (int i = ignoreIndex; i < matches.Count; i++)
            {
                stageOne.Add(matches[i].Groups["name"].Value);
            }
        }
        for (int j = 0; j < stageOne.Count; j++)
        {
            GetStageTwoFolders(j + 1, url + stageOne[j]);
        }
    }
    private void GetStageTwoFolders(int index, string url)
    {
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        Regex regex = new Regex("<a href=\".*\">(?<name>.*)</a>");
        string newUrl = string.Empty;
        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            string result = reader.ReadToEnd();

            MatchCollection matches = regex.Matches(result);
            if (matches.Count == 0)
            {
                Console.WriteLine("parse failed.");
                return;
            }
            for (int i = ignoreIndex; i < matches.Count; i++)
            {
                SetAnimFolders(index, url + matches[i].Groups["name"].Value);
            }
        }
        if (index == 1)
        {
            for (int i = 0; i < lotteryAnimSettings.folders.Count; i++)
            {
                GetFileImages(lotteryAnimSettings, i, lotteryAnimSettings.folders[i]);
            }
        }
        if (index == 2)
        {
            for (int i = 0; i < globeAnimSettings.folders.Count; i++)
            {
                GetFileImages(globeAnimSettings, i, globeAnimSettings.folders[i]);
            }
        }
        if (index == 3)
        {
            for (int i = 0; i < luckySpinAnimSettings.folders.Count; i++)
            {
                GetFileImages(luckySpinAnimSettings, i, luckySpinAnimSettings.folders[i]);
            }
        }
    }

    private void GetFileImages(AnimationsSettings animSettings, int index, string url)
    {
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        Regex regex = new Regex("<a href=\".*\">(?<name>.*)</a>");

        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            string result = reader.ReadToEnd();

            MatchCollection matches = regex.Matches(result);
            if (matches.Count == 0)
            {
                Console.WriteLine("parse failed.");
                return;
            }

            for (int i = 0; i < matches.Count; i++)
            {
                if (index == 0)
                {
                    if (matches[i].Groups["name"].Value.Contains(".png"))
                        animSettings.animEntrada.pathImages.Add(url + matches[i].Groups["name"].Value);
                }
                else if (index == 1)
                {
                    if (matches[i].Groups["name"].Value.Contains(".png"))
                        animSettings.animLoop.pathImages.Add(url + matches[i].Groups["name"].Value);
                }
                else if (index == 2)
                {
                    if (matches[i].Groups["name"].Value.Contains(".png"))
                        animSettings.animSaida.pathImages.Add(url + matches[i].Groups["name"].Value);
                }
            }
        }
        StartCoroutine(LoadImages(animSettings, index));
    }

    private IEnumerator LoadImages(AnimationsSettings animSettings, int indexAnim)
    {
        if (indexAnim == 0)
        {
            textList = new Texture2D[animSettings.animEntrada.pathImages.Count];

            int index = 0;
            foreach (string item in animSettings.animEntrada.pathImages)
            {
                string pathTemp = item;
                print("IMAGE URL= " + pathTemp);
                WWW www = new WWW(pathTemp);
                yield return www;
                Texture2D texTmp = new Texture2D(10, 10, TextureFormat.ETC2_RGBA1, false);
                www.LoadImageIntoTexture(texTmp);

                textList[index] = texTmp;
                SetImageOnList(animSettings, indexAnim, texTmp);
                index++;
            }
        }
        else if (indexAnim == 1)
        {
            textList = new Texture2D[animSettings.animLoop.pathImages.Count];

            int index = 0;
            foreach (string item in animSettings.animLoop.pathImages)
            {
                string pathTemp = item;
                WWW www = new WWW(pathTemp);
                yield return www;
                Texture2D texTmp = new Texture2D(10, 10, TextureFormat.ETC2_RGBA1, false);
                www.LoadImageIntoTexture(texTmp);

                textList[index] = texTmp;
                SetImageOnList(animSettings, indexAnim, texTmp);
                index++;
            }
        }
        else if (indexAnim == 2)
        {
            textList = new Texture2D[animSettings.animSaida.pathImages.Count];

            int index = 0;
            foreach (string item in animSettings.animSaida.pathImages)
            {
                string pathTemp = item;
                WWW www = new WWW(pathTemp);
                yield return www;
                Texture2D texTmp = new Texture2D(10, 10, TextureFormat.ETC2_RGBA1, false);
                www.LoadImageIntoTexture(texTmp);

                textList[index] = texTmp;
                SetImageOnList(animSettings, indexAnim, texTmp);
                index++;
            }
        }
    }
    private void SetAnimFolders(int index, string url)
    {
        switch (index)
        {
            case 1:
                {
                    lotteryAnimSettings.folders.Add(url);
                    break;
                }
            case 2:
                {
                    globeAnimSettings.folders.Add(url);
                    break;
                }
            case 3:
                {
                    luckySpinAnimSettings.folders.Add(url);
                    break;
                }
        }
    }
    private void SetImageOnList(AnimationsSettings animSettings, int indexAnim, Texture2D image)
    {
        switch (indexAnim)
        {
            case 0:
                {
                    animSettings.animEntrada.images.Add(image);
                    break;
                }
            case 01:
                {
                    animSettings.animLoop.images.Add(image);
                    break;
                }
            case 02:
                {
                    animSettings.animSaida.images.Add(image);
                    break;
                }
        }
    }
    public void CallPlayAnimation()
    {
        StopAllCoroutines();
        typeAnim = TypeAnim.Entrada;
        StartCoroutine(PlayAnimation(lotteryAnimSettings.animEntrada));
    }
    private IEnumerator PlayAnimation(Animation anim)
    {
        Animation currentAnim = anim;
        int indexAnim = 0;
        while (indexAnim <= anim.images.Count - 1)
        {
            frameVideo.texture = anim.images[indexAnim];
            indexAnim++;
            yield return new WaitForSeconds(anim.interval);
        }
        if (anim.isLoop)
        {
            StartCoroutine(PlayAnimation(currentAnim));
        }
        else
        {
            if (typeAnim == TypeAnim.Entrada)
            {
                StartCoroutine(PlayAnimation(lotteryAnimSettings.animLoop));
                typeAnim = TypeAnim.Loop;
            }
            else if (typeAnim == TypeAnim.Loop)
            {
                StartCoroutine(PlayAnimation(lotteryAnimSettings.animSaida));
                typeAnim = TypeAnim.Saida;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            CallPlayAnimation();
        }
    }

    [Serializable]
    public class AnimationsSettings
    {
        [HideInInspector] public List<string> folders;
        public Animation animEntrada;
        public Animation animLoop;
        public Animation animSaida;
    }
    [Serializable]
    public class Animation
    {
        [HideInInspector] public List<string> pathImages;
        public bool isLoop = false;
        public float interval = 0.05f;
        public List<Texture2D> images;
    }
}
