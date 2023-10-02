﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Video;

public class UiGlobeManager : MonoBehaviour
{

    DateTime dateValue;
    [Header("COMPONENTS SCRIPTS")]
    [SerializeField] private GlobeController globeController;
    [SerializeField] private PrizeImageController prizeImageController;

    [SerializeField] private GameObject confets;
    [SerializeField] private Image imgPrize;
    [SerializeField] private Image imgGlobeLogo;
  
    [Header("TEXTS")]
    [SerializeField] private TextMeshProUGUI txtEditionInfo;
    [SerializeField] private TextMeshProUGUI txtRoundRaffle;
    [SerializeField] private TextMeshProUGUI txtDateRaffle;
    //[SerializeField] private TextMeshProUGUI txtPrizeName;
    //[SerializeField] private TextMeshProUGUI txtPrizeValue;

    [Header("VARIABLES")]

    [SerializeField] private bool isFirst;

    void Start()
    {

        InitializeVariables();
        //Invoke("PlayVideo", 3f);
    }

    void PlayVideo()
    {
        VideoPlayer video = FindObjectOfType<VideoPlayer>();
        video.Play();
    }
    private void InitializeVariables()
    {
        globeController = FindObjectOfType<GlobeController>();
     
        StartCoroutine(ActiveRaffle());
        UpdateOrder();
    }

    public void SetPopulateInfosGlobe(string _editionName, string _editionNumber, string _date, int _order, string _description, float _value)
    {
        globeController.PopulateInfosGlobe(_editionName, _editionNumber, _date, _order, _description, _value);


        txtRoundRaffle.text = $"{GameManager.instance.globeData.order}º Sorteio";

        //txtPrizeName.text = $"{GameManager.instance.globeData.description}";
        //txtPrizeValue.text = $"Valor Líquido {GameManager.instance.FormatMoneyInfo(GameManager.instance.globeData.value,2)}";
        prizeImageController.SetPrizeImage(GameManager.instance.globeData.order);
        StartCoroutine(ActiveRaffle());

    }
    public void UpdateOrder()
    {
        txtRoundRaffle.text = $"{GameManager.instance.globeData.order}º Sorteio";

    }
    public void SetGlobeRaffle(string[] _ballsRaffled, int _forOneBall, int _winnersCount, float _prizeValue)
    {
        globeController.UpdateScreenRaffle(_ballsRaffled, _forOneBall, _winnersCount, _prizeValue);
    }
    

    public IEnumerator ActiveRaffle()
    {
        yield return new WaitForSeconds(1f);
        imgPrize.transform.DOScale(1, 0.5f).SetDelay(2f);
        imgGlobeLogo.transform.DOScale(1, 0.5f).SetDelay(2f);
        yield return new WaitForSeconds(2.5f);

        //while (txtPrizeName.alpha < 1)
        //{
        //    txtRoundRaffle.alpha += 0.1f;
        //    yield return new WaitForSeconds(0.1f);
        //}
    }
    public void ActiveConfets()
    {
        confets.SetActive(true);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        DateTime theTime = DateTime.Now;
        txtDateRaffle.text = theTime.ToString(("dd/MM/yyyy - HH:mm:ss"));
        txtEditionInfo.text = $"EDIÇÃO N° {GameManager.instance.globeData.editionNumber}";
    }
}

