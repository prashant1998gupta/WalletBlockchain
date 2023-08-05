using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PopUpInfo : MonoBehaviour
{
    public TextMeshProUGUI popUpTextInfo;
    public Button backButton;
    private PopupController popupController;

    private void Start()
    {
        popupController = GetComponent<PopupController>();
        backButton.onClick.AddListener(OnClickBackButton);
    }

    private void OnDisable()
    {
        //  backButton.onClick.RemoveAllListeners();
    }

    private void OnClickBackButton()
    {
        popupController.PanelFadeOut(1f);
    }
}
