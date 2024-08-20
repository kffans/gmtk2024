using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonClickManager : MonoBehaviour
{
     void Start()
    {
       Button[] buttons =  Resources.FindObjectsOfTypeAll<Button>();
       

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(delegate { OnButtonClick(button); });
        }
    }
    void OnButtonClick(Button button)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX("button_click");
            Debug.LogWarning("button clicked");
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found. Make sure AudioManager is present in the scene.");
        }
    }
}
