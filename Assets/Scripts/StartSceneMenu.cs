using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneMenu : MonoBehaviour
{
    public InputField inputField;

    public void SaveNickName()
    {
        GameManager.Instance.PlayerNickName = inputField.text;
        SceneManager.LoadSceneAsync("Game");
    }
}
