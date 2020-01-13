using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelActivatorButton : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowExitPanel);
    }

    // Update is called once per frame
    void ShowExitPanel()
    {
        panel.SetActive(true);
    }
}
