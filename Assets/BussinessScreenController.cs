using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BussinessScreenController : MonoBehaviour
{
    public List<Button> tabButtons;
    public List<GameObject> tabPanel;

    public void ClickBusinessTab(int index)
    {
        for (int i = 0; i < tabButtons.Count; i++)
        {
            tabButtons[i].GetComponent<Image>().color = tabButtons[i].colors.normalColor;
            tabPanel[i].SetActive(false);
        }
        tabButtons[index].GetComponent<Image>().color = tabButtons[index].colors.pressedColor;
        tabPanel[index].SetActive(true);

    }
}
