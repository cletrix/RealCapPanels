using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VersionController : MonoBehaviour
{
    public string version = "Vers�o : 12/04/2023 - 0.0.1";

    [SerializeField] private TextMeshProUGUI txtVersion;
    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        txtVersion.text = version;
    }

    private void SetVisibilityVersion()
    {
        if (txtVersion.gameObject.activeSelf == true)
            txtVersion.gameObject.SetActive(false);
        else
            txtVersion.gameObject.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            SetVisibilityVersion();
        }
    }
}
