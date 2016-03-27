using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartLoad : MonoBehaviour {

	// Use this for initialization
	public void LoadGame(int numPlayers)
    {
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetInt("NumPlayers", numPlayers);
    }
}
