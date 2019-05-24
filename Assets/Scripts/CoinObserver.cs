using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CoinObserver : MonoBehaviour
{
    public StoryState state;
    
    // Start is called before the first frame update
    void Start()
    {
        state.SubscribeToQuality("Coins", UpdateCoins);
        state.SubscribeToQuality("Flurbs", UpdateFlurbs);
    }

    private void OnDestroy()
    {
        
        state.UnsubscribeFromQuality(UpdateFlurbs);
        state.UnsubscribeFromQuality(UpdateCoins);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCoins(string qualityName, int newValue)
    {
        Debug.Log("You have " + newValue + " coins");
    }
    
    void UpdateFlurbs(string qualityName, int newValue)
    {
        Debug.Log("You have " + newValue + " flurbs");
    }
}
