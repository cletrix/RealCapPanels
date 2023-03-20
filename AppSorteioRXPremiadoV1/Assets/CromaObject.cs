using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CromaObject : MonoBehaviour
{
    [SerializeField] private Image imageCroma;
    void Start()
    {
        imageCroma = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (imageCroma.enabled == true)
                imageCroma.enabled = false;
            else
                imageCroma.enabled = true;
        }
    }
}
