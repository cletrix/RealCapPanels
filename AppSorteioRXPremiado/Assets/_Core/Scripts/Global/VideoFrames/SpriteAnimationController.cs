using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class SpriteAnimationController : MonoBehaviour
{
    public enum TypeAnim
    {
        Entrada = 1,
        Loop = 2,
        Saida = 3
    }
    public TypeAnim typeAnim = TypeAnim.Entrada;

    [Space]
    [SerializeField] private AnimationsSettings lotteryAnimSettings;
    [SerializeField] private AnimationsSettings globeAnimSettings;
    [SerializeField] private AnimationsSettings luckySpinAnimSettings;
    [SerializeField] private RawImage frameVideo;

    public static SpriteAnimationController instance { get; private set; }

  
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
        CallPlayAnimation(lotteryAnimSettings, 1);
    }
    public void CallPlayAnimation(AnimationsSettings anim, int index)
    {
        StopAllCoroutines();
        switch (index)
        {
            case 1:
                {
                    StartCoroutine(PlayAnimation(anim,anim.animEntrada));
                    break;
                }
            case 2:
                {
                    StartCoroutine(PlayAnimation(anim,anim.animLoop));
                    break;
                }
            case 3:
                {
                    StartCoroutine(PlayAnimation(anim,anim.animSaida));
                    break;
                }
        }
        
    }
    private IEnumerator PlayAnimation(AnimationsSettings animationsSettings,Animation anim)
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
            StartCoroutine(PlayAnimation(animationsSettings, currentAnim));
        }
        else
        {
            if (typeAnim == TypeAnim.Entrada)
            {
                StartCoroutine(PlayAnimation(animationsSettings, animationsSettings.animLoop));
                typeAnim = TypeAnim.Loop;
            }
            else if (typeAnim == TypeAnim.Loop)
            {
                StartCoroutine(PlayAnimation(animationsSettings, animationsSettings.animSaida));
                typeAnim = TypeAnim.Saida;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            CallPlayAnimation(lotteryAnimSettings,1);
        }
    }

    [Serializable]
    public class AnimationsSettings
    {
        public Animation animEntrada;
        public Animation animLoop;
        public Animation animSaida;
    }
    [Serializable]
    public class Animation
    {
        public bool isLoop = false;
        public float interval = 0.05f;
        public List<Texture2D> images;
    }
}


