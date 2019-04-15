using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    
    private Texture2D texture;
    private Texture2D texture2;
    private Texture2D texture3;
    private Texture2D texture4;
    public Image img;
    public Image img2;
    public Image img3;
    public Image img4;
    public CustomGradient tasteGradient;
    public CustomGradient tasteGradient2;
    public CustomGradient tasteGradient3;
    public CustomGradient tasteGradient4;
    public CustomGradient tasteGradient_Dislike;
    public CustomGradient tasteGradient_Dislike2;
    public CustomGradient tasteGradient_Dislike3;
    public CustomGradient tasteGradient_Dislike4;
    public CustomGradient tasteGradient_Irrelevant;
    public CustomGradient tasteGradient_Irrelevant2;
    public CustomGradient tasteGradient_Irrelevant3;
    public CustomGradient tasteGradient_Irrelevant4;

    public List<GameObject> leftHandGradients;
    public List<GameObject> rightHandGradients;
    public void ResetGradients(int index, bool state)
    {
        switch (index)
        {
            case 0:
                foreach (var item in leftHandGradients)
                {

                    item.SetActive(state);
                }
                break;
            case 1:
                foreach (var item in rightHandGradients)
                {

                    item.SetActive(state);
                }
                break;
            default:
                break;
        }
    }
    public void SetGradient1(float green, int index)
    {
        switch (index)
        {
            case 0:
                tasteGradient.UpdateKeyTime(1, green / 100f);
                texture = tasteGradient.GetTexture(100);
                break;
            case 1:
                tasteGradient_Dislike.UpdateKeyTime(2, green / 100f);
                texture = tasteGradient_Dislike.GetTexture(100);
                break;
            case 2:
                tasteGradient_Irrelevant.UpdateKeyTime(1, green / 100f);
                texture = tasteGradient_Irrelevant.GetTexture(100);
                break;
            default:
                break;
        }
       
        img.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
    public void SetGradient2(float green, int index)
    {
        switch (index)
        {
            case 0:
                tasteGradient2.UpdateKeyTime(1, green / 100f);
                texture2 = tasteGradient.GetTexture(100);
                break;
            case 1:
                tasteGradient_Dislike2.UpdateKeyTime(2, green / 100f);
                texture2 = tasteGradient_Dislike2.GetTexture(100);
                break;
            case 2:
                tasteGradient_Irrelevant2.UpdateKeyTime(1, green / 100f);
                texture2 = tasteGradient_Irrelevant2.GetTexture(100);
                break;
            default:
                break;
        }

        img2.sprite = Sprite.Create(texture2, new Rect(0.0f, 0.0f, texture2.width, texture2.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
    public void SetGradient3(float green, int index)
    {
        switch (index)
        {
            case 0:
                tasteGradient3.UpdateKeyTime(1, green / 100f);
                texture3 = tasteGradient3.GetTexture(100);
                break;
            case 1:
                tasteGradient_Dislike3.UpdateKeyTime(2, green / 100f);
                texture3 = tasteGradient_Dislike3.GetTexture(100);
                break;
            case 2:
                tasteGradient_Irrelevant3.UpdateKeyTime(1, green / 100f);
                texture3 = tasteGradient_Irrelevant3.GetTexture(100);
                break;
            default:
                break;
        }

        img3.sprite = Sprite.Create(texture3, new Rect(0.0f, 0.0f, texture3.width, texture3.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
    public void SetGradient4(float green, int index)
    {
        switch (index)
        {
            case 0:
                tasteGradient4.UpdateKeyTime(1, green / 100f);
                texture4 = tasteGradient4.GetTexture(100);
                break;
            case 1:
                tasteGradient_Dislike4.UpdateKeyTime(2, green / 100f);
                texture4 = tasteGradient_Dislike4.GetTexture(100);
                break;
            case 2:
                tasteGradient_Irrelevant4.UpdateKeyTime(1, green / 100f);
                texture4 = tasteGradient_Irrelevant4.GetTexture(100);
                break;
            default:
                break;
        }

        img4.sprite = Sprite.Create(texture4, new Rect(0.0f, 0.0f, texture4.width, texture4.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
