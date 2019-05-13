using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Config : MonoBehaviour
{
    public GameObject skipButton;
    string filePath; 


    void Start()
    {
        filePath= Application.persistentDataPath + "/config";

        try
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                skipButton.SetActive(false);
            }
            else
            {
                skipButton.SetActive(true);
            }

        }
        catch (IOException ex)
        {
            Debug.Log("Hata: " + ex.Message);
        }


    }
}
