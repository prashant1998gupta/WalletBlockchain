using UnityEngine;

public class InternetChecker : MonoBehaviour
{
	private const bool allowCarrierDataNetwork = false;
	private const string pingAddress = "8.8.8.8"; // Google Public DNS server
	public const float waitingTime = 5.0f;

	bool internetConnectBool;
	private Ping ping;
	private float pingStartTime;

	public delegate void InternetCheckerD(bool InternetAvailable);
	public static event InternetCheckerD MyInternet;

	public static InternetChecker ICInstance;

	void Awake() 
	{
		//ICInstance = new InternetChecker();
		if (ICInstance != null) 
		{
			GameObject.Destroy(ICInstance);
		}
		else
		{
			ICInstance = this;
		}	
	}

	public void StartInternetCheck()
	{
		Debug.Log("StartInternetCheck");
		bool internetPossiblyAvailable;
		switch (Application.internetReachability)
		{
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			internetPossiblyAvailable = true;
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			//internetPossiblyAvailable = allowCarrierDataNetwork;
			internetPossiblyAvailable = true;
			break;
		default:
			internetPossiblyAvailable = false;
			break;
		}
		if (!internetPossiblyAvailable)
		{
			InternetIsNotAvailable();
			return;
		}
		ping = new Ping(pingAddress);
		pingStartTime = Time.time;
	}
	
	public void Update()
	{
		if (ping != null)
		{
			bool stopCheck = true;
			if (ping.isDone)
				InternetAvailable();
			else if (Time.time - pingStartTime < waitingTime)
				stopCheck = false;
			else
				InternetIsNotAvailable();
			if (stopCheck)
				ping = null;
		}
	}
	
	public void InternetIsNotAvailable()
	{
		if (MyInternet != null) 
		{	
			MyInternet (false);
		}

		internetConnectBool = false;
	}
	
	public void InternetAvailable()
	{
		if (MyInternet != null)
		{	
			MyInternet (true);
		}

		internetConnectBool = true;
	}

	public void InternetCheck()
	{
		bool internetPossiblyAvailable;
		switch (Application.internetReachability)
		{
			case NetworkReachability.ReachableViaLocalAreaNetwork:
				internetPossiblyAvailable = true;
				break;
			case NetworkReachability.ReachableViaCarrierDataNetwork:
			internetPossiblyAvailable = true;
				break;
			default:
				internetPossiblyAvailable = false;
				break;
		}
		if (!internetPossiblyAvailable)
		{
			InternetIsNotAvailable();
			return;
		}
		ping = new Ping(pingAddress);
		pingStartTime = Time.time;
	}
}