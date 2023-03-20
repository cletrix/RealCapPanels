using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StandyByScreen : MonoBehaviour
{
    public static StandyByScreen instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
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
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        //Invoke("SetVisibilityScreen",4f);
    }
  
    public void SetManualVisibilityScreen(bool isActive)
    {
        if (isActive == true)
        {
            canvasGroup.DOFade(0, 1f).OnComplete(() =>
            {
                //GameManager.instance.SendMessageToServerVisibilityScene(GetVisibilityStandBy());
            });
        }
        else
        {
            canvasGroup.DOFade(1, 1f).OnComplete(() =>
            {
               //GameManager.instance.SendMessageToServerVisibilityScene(GetVisibilityStandBy());
            });
        }
    }
    public bool GetVisibilityStandBy()
    {
        if (canvasGroup.alpha == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   

}
