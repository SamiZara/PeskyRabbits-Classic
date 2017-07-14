using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

    public static AdManager instance;
    private int adShowTime,currentTime;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	public void LevelEnded()
    {
        currentTime++;
        if(adShowTime <= currentTime)
        {
            Debug.Log("Showing ad");
            ShowAd();
            currentTime = 0;
            adShowTime = Random.Range(3, 6);
        }
    }
    public void ShowAd()
    {
        Advertisement.Show();
    }
}
