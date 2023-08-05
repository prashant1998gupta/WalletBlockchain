using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Prashant;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;
using System.Numerics;

public class WalletManager : MonoBehaviour
{
    public ProfileManager profileManager;
    public TokenTransfer tokenTransfer; 

    [Header("CoinSendSelectionPanel")]
    [SerializeField]
    private GameObject coinSendSelectionPanel;
    [SerializeField]
    private Tokens walletToken;
    [SerializeField]
    private Button mldButton;
    [SerializeField]
    private Button lordButton;
    [SerializeField]
    private Button maticButton;
    [SerializeField]
    private Button receiveInfoButton;
    [SerializeField]
    private Button historyFetchButton;

    [Header("SendReceivePanel")]
    [SerializeField]
    private GameObject sendReceivePanel;
    [SerializeField]
    private Button sendButton;
    [SerializeField]
    private Button receiveButton;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Button copyToClipBoard;
    [SerializeField]
    private TextMeshProUGUI walletAddressText;
    [SerializeField]
    private List<Sprite> coinSprite;
    [SerializeField]
    private Image coinIcon;
    [SerializeField]
    private TextMeshProUGUI totalCoinText;
    [SerializeField]
    private Button sendReceivePanelBackButton;

    [Header("SendPanel")]
    [SerializeField]
    private GameObject SendPanel;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button cancleButton;
    [SerializeField]
    private TMP_InputField toWalletAddressText;
    [SerializeField]
    private TMP_InputField amountInputText;
    [SerializeField]
    private Button SendPanelBackButton;

    [Header("QRPanel")]
    [SerializeField]
    private GameObject QRPanel;
    [SerializeField]
    private Button copyToClipBoardInQR;
    [SerializeField]
    private TextMeshProUGUI walletAddressTextInQR;
    [SerializeField]
    private Button QRPanelBackButton;
    public Text infoText;
    public QRCodeEncodeController e_qrController;
    public RawImage qrCodeImage;
    public Texture2D codeTex;

    [Header("transectionHistoryData")]
    [SerializeField]
    private GameObject transectionHistoryPanel;
    public GameObject transectionHistoryPrefeb;
    public Transform transectionHistoryTransform;
    public Button transectionHistoryBackButton;
    public GameObject transectionHistoryNotificationPanel;


    PopUpInfo popUpInfo;

    private void Awake()
    {
        profileManager = GetComponent<ProfileManager>();
        tokenTransfer = FindObjectOfType<TokenTransfer>();
        popUpInfo = Prashant.GameManager.instance.popUpScreen.GetComponent<PopUpInfo>();
    }

    public void Start()
    {
        walletAddressText.text = Prashant.Utils.userWalletAccount;
        walletAddressTextInQR.text = Prashant.Utils.userWalletAccount;
       

        //CoinSendSelectionPanel buttons
        mldButton.onClick.AddListener(OnClickMldButton);
        lordButton.onClick.AddListener(OnClickLordButton);
        maticButton.onClick.AddListener(OnClickMaticButton);
        receiveInfoButton.onClick.AddListener(OnClickreceiveInfoButton);
        historyFetchButton.onClick.AddListener(OnClickhistoryFetchButton);

        //SendReceivePanel buttons
        sendButton.onClick.AddListener(OnClickSendButton);
        receiveButton.onClick.AddListener(OnClickreceiveInfoButton);
        buyButton.onClick.AddListener(OnClickBuyButton);
        copyToClipBoard.onClick.AddListener(profileManager.OnCopyButtonClick);
        sendReceivePanelBackButton.onClick.AddListener(OnClickSendReceivePanelBackButton);

        //SendPanel buttons
        confirmButton.onClick.AddListener(OnClickConfirmButton);
        cancleButton.onClick.AddListener(OnClickCancleButton);
        SendPanelBackButton.onClick.AddListener(OnSendPanelBackButton);

        // QRPanel buttons 
        QRPanelBackButton.onClick.AddListener(OnClickQRPanelBackButton);
        copyToClipBoardInQR.onClick.AddListener(profileManager.OnCopyButtonClick);

        // QRPanel buttons 
        transectionHistoryBackButton.onClick.AddListener(OntransectionHistoryBackButton);

        Encode();
    }


    //SendPanel methods
    private async void OnClickConfirmButton()
    {
        TransferResult result = new TransferResult();

        GameManager.instance.newLoadingScreen.SetActive(true);
       
        if (walletToken == Tokens.mldToken)
        {
            result = await tokenTransfer.SendTokensMLD(toWalletAddressText.text , amountInputText.text);
        }
        else if(walletToken == Tokens.lordToken)
        {
            result = await tokenTransfer.SendTokensLORD(toWalletAddressText.text, amountInputText.text);
        }
        else if(walletToken == Tokens.matic)
        {
            result = await tokenTransfer.SendTokensMATIC(toWalletAddressText.text, amountInputText.text);
        }

        if(result.StatusCode == 1)
        {
            popUpInfo.popUpTextInfo.text = result.Message;
            GameManager.instance.popUpScreen.SetActive(true);
        }
        else if(result.StatusCode == 0)
        {
            popUpInfo.popUpTextInfo.text = result.Message;
            GameManager.instance.popUpScreen.SetActive(true);
        }
        else if(result.StatusCode == -1)
        {
            popUpInfo.popUpTextInfo.text = result.Message;
            GameManager.instance.popUpScreen.SetActive(true);
        }
        else if (result.StatusCode == -2)
        {
            popUpInfo.popUpTextInfo.text = result.Message;
            GameManager.instance.popUpScreen.SetActive(true);
        }
        else if (result.StatusCode == -3)
        {
            popUpInfo.popUpTextInfo.text = result.Message;
            GameManager.instance.popUpScreen.SetActive(true);
        }
        else
        {
            popUpInfo.popUpTextInfo.text = "Token Transfer UnSuccessfull!";
            GameManager.instance.popUpScreen.SetActive(true);
        }
        GameManager.instance.newLoadingScreen.SetActive(false);
    }

    private void OnClickCancleButton()
    {
        toWalletAddressText.text= string.Empty;
        amountInputText.text = string.Empty;
    }

    private async void OnSendPanelBackButton()
    {
        GameManager.instance.newLoadingScreen.SetActive(true);
        decimal balance = 0;

        if (walletToken == Tokens.mldToken)
        {
             balance = await tokenTransfer.GetBalanceAsyncMLD();
            totalCoinText.text = balance.ToString("F3") + " MLD";
        }
        else if (walletToken == Tokens.lordToken)
        {
             balance = await tokenTransfer.GetBalanceAsyncLORD();
            totalCoinText.text = balance.ToString("F3") + " LORD";
        }
        else if (walletToken == Tokens.matic)
        {
            balance = await tokenTransfer.GetBalanceAsyncMatic();
            totalCoinText.text = balance.ToString("F3") + " MATIC";
        }

        if (balance == -1)
        {
            popUpInfo.popUpTextInfo.text = "something wrong fetching token balance please try again!";
            GameManager.instance.popUpScreen.SetActive(true);
        }
        else
        {
            sendReceivePanel.gameObject.SetActive(true);
            SendPanel.gameObject.SetActive(false);
        }

        toWalletAddressText.text = string.Empty;
        amountInputText.text = string.Empty;

        GameManager.instance.newLoadingScreen.SetActive(false);
    }



    //SendReceivePanel methods
    private void OnClickSendButton()
    {
        sendReceivePanel.gameObject.SetActive(false);
        SendPanel.gameObject.SetActive(true);
    }

    private void OnClickBuyButton()
    {
        UnityEngine.Debug.Log($"not in use");
    }

    private void OnClickSendReceivePanelBackButton()
    {
        coinSendSelectionPanel.gameObject.SetActive(true);
        sendReceivePanel.gameObject.SetActive(false);
    }


    // CoinSendSelectionPanel methods

    private async void OnClickMldButton()
    {
        walletToken = Tokens.mldToken;
        GameManager.instance.newLoadingScreen.SetActive(true);
        decimal balance = await tokenTransfer.GetBalanceAsyncMLD();
        if(balance == -1)
        {
            GameManager.instance.newLoadingScreen.SetActive(false);
            popUpInfo.popUpTextInfo.text = "something wrong fetching token balance please try again!";
            GameManager.instance.popUpScreen.SetActive(true);
        }
        else
        {
            coinIcon.sprite = coinSprite[0];
            coinSendSelectionPanel.gameObject.SetActive(false);
            sendReceivePanel.gameObject.SetActive(true);
            totalCoinText.text = balance.ToString("F3") + " MLD";
            GameManager.instance.newLoadingScreen.SetActive(false);
        }
       
    }

    private async void OnClickLordButton()
    {
        walletToken = Tokens.lordToken;
        GameManager.instance.newLoadingScreen.SetActive(true);
        decimal balance = await tokenTransfer.GetBalanceAsyncLORD();
        if(balance == -1)
        {
            GameManager.instance.newLoadingScreen.SetActive(false);
            popUpInfo.popUpTextInfo.text = "something wrong fetching token balance please try again!";
            GameManager.instance.popUpScreen.SetActive(true);
        }
        else
        {
            coinIcon.sprite = coinSprite[1];
            coinSendSelectionPanel.gameObject.SetActive(false);
            sendReceivePanel.gameObject.SetActive(true);
            totalCoinText.text = balance.ToString("F3") + " LORD";
            GameManager.instance.newLoadingScreen.SetActive(false);
        } 
    }

    private async void OnClickMaticButton()
    {
        walletToken = Tokens.matic;
        GameManager.instance.newLoadingScreen.SetActive(true);
        decimal balance = await tokenTransfer.GetBalanceAsyncMatic();
        if(balance == -1)
        {
            GameManager.instance.newLoadingScreen.SetActive(false);
            popUpInfo.popUpTextInfo.text = "something wrong fetching token balance please try again!";
            GameManager.instance.popUpScreen.SetActive(true);
        }
        else
        {
            coinIcon.sprite = coinSprite[2];
            coinSendSelectionPanel.gameObject.SetActive(false);
            sendReceivePanel.gameObject.SetActive(true);
            totalCoinText.text = balance.ToString("F3") + " MATIC";
            GameManager.instance.newLoadingScreen.SetActive(false);
        }
    }

    private void OnClickreceiveInfoButton()
    {
        //coinSendSelectionPanel.gameObject.SetActive(false);
        QRPanel.gameObject.SetActive(true);
    }

    private async void OnClickhistoryFetchButton()
    {
        GameManager.instance.newLoadingScreen.SetActive(true);

        foreach (Transform trans in transectionHistoryTransform)
        {
            Destroy(trans.gameObject);
        }

        transectionHistoryPanel.gameObject.SetActive(true);

        string response = await tokenTransfer.GetTransactionHistory();

        var data = JObject.Parse(response);
        UnityEngine.Debug.Log($"data: {data}");

        if (data["status"].ToString() == "1")
        {
            transectionHistoryNotificationPanel.SetActive(false);

            var transactions = data["result"];
            foreach (var tx in transactions)
            {
                UnityEngine.Debug.Log($"TxHash: {tx["hash"]}");
                UnityEngine.Debug.Log($"From: {tx["from"]}");
                UnityEngine.Debug.Log($"To: {tx["to"]}");
                UnityEngine.Debug.Log($"Value: {Web3.Convert.FromWei(BigInteger.Parse(tx["value"].ToString()))} Matic");
                UnityEngine.Debug.Log($"Block Number: {tx["blockNumber"]}");
                //UnityEngine.Debug.Log($"Timestamp: {await tokenTransfer.GetTransactionTimestamp((ulong)tx["blockNumber"])}");
                UnityEngine.Debug.Log($"Timestamp: {tx["timeStamp"]}");
                UnityEngine.Debug.Log("--------------------------");

                GameObject instantiatedPrefab = Instantiate(transectionHistoryPrefeb, transectionHistoryTransform);
                // Access the instantiated prefab's components or properties and assign the data
                instantiatedPrefab.GetComponent<TransactionsHistoryData>().SetData($"{tx["hash"]}" , $"{tx["blockNumber"]}" , $"{tx["timeStamp"]}" , $"{tx["from"]}" , $"{tx["to"]}" , $"{Web3.Convert.FromWei(BigInteger.Parse(tx["value"].ToString()))} Matic");
            }
           
        }
        else
        {
            transectionHistoryNotificationPanel.SetActive(true);
            UnityEngine.Debug.Log($"Failed to fetch transaction history: {data["message"]}");
        }

        GameManager.instance.newLoadingScreen.SetActive(false);
    }

    // QR methods
    private void OnClickQRPanelBackButton()
    {
        QRPanel.gameObject.SetActive(false);
    }

    public void qrEncodeFinished(Texture2D tex)
    {
        if (tex != null && tex != null)
        {
            int width = tex.width;
            int height = tex.height;
            float aspect = width * 1.0f / height;
            qrCodeImage.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(300.0f, 300.0f / aspect);
            qrCodeImage.texture = tex;
            codeTex = tex;
        }
        else
        {
        }
    }

    public void setCodeType(int typeId)
    {
        e_qrController.eCodeFormat = (QRCodeEncodeController.CodeMode)(typeId);
        UnityEngine.Debug.Log("clicked typeid is " + e_qrController.eCodeFormat);
    }

    public void Encode()
    {
        if (e_qrController != null)
        {
            string valueStr = walletAddressText.text;
            int errorlog = e_qrController.Encode(valueStr);
            infoText.color = Color.red;
            if (errorlog == -13)
            {
                infoText.text = "Must contain 12 digits,the 13th digit is automatically added !";

            }
            else if (errorlog == -8)
            {
                infoText.text = "Must contain 7 digits,the 8th digit is automatically added !";
            }
            else if (errorlog == -39)
            {
                infoText.text = "Only support digits";
            }
            else if (errorlog == -128)
            {
                infoText.text = "Contents length should be between 1 and 80 characters !";

            }
            else if (errorlog == -1)
            {
                infoText.text = "Please select one code type !";
            }
            else if (errorlog == 0)
            {
                infoText.color = Color.green;
                infoText.text = "Encode successfully !";
            }
        }
    }

    public void SaveCode()
    {
        GalleryController.SaveImageToGallery(codeTex);
    }

    // transectionHistoryFetch

    private void OntransectionHistoryBackButton()
    {
        transectionHistoryPanel.gameObject.SetActive(false);
    }

}

public enum Tokens
{
    None,
    mldToken,
    lordToken,
    matic
}

