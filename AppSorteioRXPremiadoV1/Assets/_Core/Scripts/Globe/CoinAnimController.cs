using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimController : MonoBehaviour
{
    public Animator animCoin;
    public bool isback;
    public const float time = 3f;
    void Start()
    {
        animCoin = GetComponent<Animator>();
        Invoke("SetAnimCoin", 5f);
    }

    private void SetAnimCoin()
    {
        if (isback)
        {
            isback = false;
        }
        else
        {
            isback = true;
        }

        animCoin.SetBool("isBack", isback);

        Invoke("SetAnimCoin", time);

    }
    void Update()
    {

    }
}
