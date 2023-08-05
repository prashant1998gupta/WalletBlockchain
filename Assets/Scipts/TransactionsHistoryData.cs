using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransactionsHistoryData : MonoBehaviour
{
    public TextMeshProUGUI TxnHash;
    public TextMeshProUGUI Block;
    public TextMeshProUGUI Age;
    public TextMeshProUGUI From;
    public TextMeshProUGUI To;
    public TextMeshProUGUI Value;

    public void SetData(string TxnHash , string Block , string Age , string From , string To , string  Value)
    {
        this.TxnHash.text = TxnHash;
        this.Block.text = Block;
        this.Age.text = Age;
        this.From.text = From;
        this.To.text = To;
        this.Value.text = Value;
    }
}
