using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VRSceneManager : MonoBehaviour
{
    public GameObject restorant, vrMainUI, businessScreen, settingsScreen;
    public Text lifeTimeDoner;
    public Text averageDurumCost;
    public GameLogic gameLogic;

    private void Awake()
    {
        gameLogic.StartPopularity();
        averageDurumCost.text = (gameLogic.popularity.totalDurumCost / gameLogic.popularity.lifeTimeDoner).ToString()+" ft";
        lifeTimeDoner.text = gameLogic.popularity.lifeTimeDoner.ToString();
    }

    public void StartGame()
    {
        restorant.SetActive(true);      
        businessScreen.SetActive(false);
        settingsScreen.SetActive(false);
        gameLogic.StartGame();
        vrMainUI.SetActive(false);

    }
    public void NewGame()
    {
        SceneManager.LoadScene("VRScene");
    }

    public void OpenBusiness()
    {
        businessScreen.SetActive(true);
    }

    public void OpenSettings()
    {
        settingsScreen.SetActive(true);
    }
}
