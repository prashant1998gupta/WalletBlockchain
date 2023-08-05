using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontDestroyManager : GenericSingleton<DontDestroyManager>
{

    public GameObject internetCanvas;
    public GameObject audioManager;
    public Button interNetOkButton;
    public GameObject PanelPopup;

    // Start is called before the first frame update
    void Start()
    {
        InternetChecker.MyInternet += MyListener;
        interNetOkButton.onClick.AddListener(OnClickInterNetOkButton);
    }

    private void OnClickInterNetOkButton()
    {
        CheckInternetConn();
    }

    //public IEnumerator CheckInternetConn()
    public void CheckInternetConn()
    {
        InternetChecker.ICInstance.StartInternetCheck();

        //yield return new WaitForSeconds(1f);
    }

    public void MyListener(bool isInternetAvailable)
    {
        if (isInternetAvailable)
        {
            Debug.Log("Internet is Available");
            internetCanvas.gameObject.SetActive(false);
            PanelPopup.SetActive(false);
           
        }

        else if (!isInternetAvailable)
        {
            Debug.Log("Internet is not Available");
            internetCanvas.gameObject.SetActive(true);
            
        }
    }

    void OnDisable()
    {
        InternetChecker.MyInternet -= MyListener;
    }

}
