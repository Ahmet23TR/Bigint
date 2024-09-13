using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneMenu : MonoBehaviour
{
    public InputField inputField;
    public Button startButton;  

    void Start()
    {
        inputField.ActivateInputField();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            startButton.onClick.Invoke();  
        }
    }

    public void SaveNickName()
    {
        if (!string.IsNullOrEmpty(inputField.text)) 
        {
            GameManager.Instance.PlayerNickName = inputField.text;
            SceneManager.LoadSceneAsync("Game");
        }
    }
}
