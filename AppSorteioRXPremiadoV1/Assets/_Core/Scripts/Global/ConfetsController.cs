using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfetsController : MonoBehaviour
{
    [SerializeField] private ParticleSystem confets;
    void Start()
    {
        confets = GetComponentInChildren<ParticleSystem>();
    }
    void Update()
    {
        if (confets.isStopped)
        {
            gameObject.SetActive(false);
        }
    }
}
