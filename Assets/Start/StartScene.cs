using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class StartScene : MonoBehaviour {

    public GameObject numPlayers;
    public GameObject StartB;

    // Use this for initialization
    void Start () {
        numPlayers = GameObject.Find("Player");
        StartB = GameObject.Find("StartB");
        numPlayers.SetActive(false);
    }

    public void StartButton()
    {
        StartB.SetActive(false);
        numPlayers.SetActive(true);
    }
}
