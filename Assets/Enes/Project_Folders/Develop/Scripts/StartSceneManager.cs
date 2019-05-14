using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public GameObject tutorialScreen, factionScreen;

    public void SkipTutorialScreen()
    {
        SceneManager.LoadScene("VRScene");
    }

    public void StartScreen()
    {
        tutorialScreen.SetActive(false);
        factionScreen.SetActive(true);
    }
    public void ChooseFaction(int i)
    {
        Debug.Log(i+" seçildi.");
    }
}
