using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	public void CheckInternetConn()
	{		
		InternetChecker.MyInternet += MyListener;
		InternetChecker.ICInstance.StartInternetCheck ();
	}

	public void MyListener(bool isInternetAvailable) 
	{
		if (isInternetAvailable) 
		{
			Debug.Log ("Internet is Available");
			//gameObject.SetActive(false);
		}

		else if (!isInternetAvailable) 
		{
			Debug.Log ("Internet is not Available");
		}
	}

	void OnDisable()
	{
		InternetChecker.MyInternet -= MyListener;
	}
}
