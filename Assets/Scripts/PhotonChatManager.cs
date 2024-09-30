using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    public PlayerNickname playerNickname;

    #region Setup
    ChatClient chatClient;
    bool isConnected;

    [SerializeField] GameObject chatPanel;  
    [SerializeField] InputField chatField;  
    [SerializeField] Text chatDisplay;
    [SerializeField] Button chatButton;
    private bool isChatOpen = false;  

    private NetworkRunner runner; // NetworkRunner referansı    

    void Start()
    {
        chatPanel.SetActive(false);  // Chat paneli başlangıçta kapalı
        chatField.gameObject.SetActive(false);  // Input alanı kapalı
        chatDisplay.gameObject.SetActive(false);  // Mesajlar başlangıçta gizli

        runner = FindObjectOfType<NetworkRunner>(); // Sahnedeki NetworkRunner'ı bul
        if (runner == null)
        {
            Debug.LogError("NetworkRunner bulunamadı!");
        }
    }


    void Update()
    {
        if (isConnected)
        {
            chatClient.Service();
        }

        // Tab tuşuna basıldığında chat'i aç/kapat
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab'a basıldı");
            ToggleChat();
        }

        if (isChatOpen && chatField.text != "" && Input.GetKey(KeyCode.Return))
        {
            SubmitPublicChatOnClick();
        }
    }

    // Chat'i açma/kapatma ve bağlanma fonksiyonu
    void ToggleChat()
    {
        isChatOpen = !isChatOpen;  // Chat açık mı kapalı mı tersine çevir

        chatPanel.SetActive(isChatOpen);
        chatField.gameObject.SetActive(isChatOpen);
        chatDisplay.gameObject.SetActive(isChatOpen);
        chatButton.gameObject.SetActive(isChatOpen);


        if (isChatOpen && !isConnected)
        {
            ChatConnectOnClick();
        }

        if (isChatOpen)
        {
            chatField.ActivateInputField();
            DisableCharacterMovement();
            DisableCharacterAnimation();
        }
        else
        {
            EnableCharacterMovement();
            EnableCharacterAnimation();
        }
    }

    // Chat bağlantısını kurma 
    public void ChatConnectOnClick()
    {
        isConnected = true;
        chatClient = new ChatClient(this);

        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(GameManager.Instance.PlayerNickName));

        Debug.Log("Connecting to chat...");
        Debug.Log(GameManager.Instance.PlayerNickName);
    }

    void DisableCharacterMovement()
    {
        if (PlayerSpawner.LocalPlayerInstance == null)
        {
            Debug.LogError("Yerel oyuncu bulunamadı!");
            return;
        }

        // Hareketi devre dışı bırak
        PlayerSpawner.LocalPlayerInstance.enabled = false;
        Debug.Log("Yerel oyuncu, hareket devre dışı bırakıldı.");
    }

    void EnableCharacterMovement()
    {
        if (PlayerSpawner.LocalPlayerInstance == null)
        {
            Debug.LogError("Yerel oyuncu bulunamadı!");
            return;
        }

        // Hareketi tekrar aktif et
        PlayerSpawner.LocalPlayerInstance.enabled = true;
        Debug.Log("Yerel oyuncu, hareket tekrar etkinleştirildi.");
    }

    void DisableCharacterAnimation()
    {
        if (PlayerSpawner.LocalPlayerAnimator != null)
        {
            PlayerSpawner.LocalPlayerAnimator.SetBool("isWalking", false);
            PlayerSpawner.LocalPlayerAnimator.SetBool("isRunning", false);
        }
    }

    void EnableCharacterAnimation()
    {
        if (PlayerSpawner.LocalPlayerAnimator != null)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

            PlayerSpawner.LocalPlayerAnimator.SetBool("isWalking", isMoving && !isRunning);
            PlayerSpawner.LocalPlayerAnimator.SetBool("isRunning", isMoving && isRunning);
        }
    }


    #endregion Setup

    #region General
    string privateReceiver = "";
    string currentChat;


    #endregion General

    #region PublicChat
    public void SubmitPublicChatOnClick()
    {
        if (chatClient == null || !isConnected)
        {
            Debug.LogError("ChatClient is not connected or initialized.");
            return;  
        }

        if (!string.IsNullOrEmpty(chatField.text))
        {
            chatClient.PublishMessage("RegionChannel", chatField.text);  
            chatField.text = "";  

            // Chat açık kaldığı sürece odak kaybolmasın
            chatField.ActivateInputField();
        }
    }

    public void TypeChatOnValueChange(string valueIn)
    {
        currentChat = valueIn;
    }
    #endregion PublicChat

    #region Callbacks
    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        Debug.Log(message); 
    }

    public void OnChatStateChange(ChatState state)
    {
        if (state == ChatState.Uninitialized)
        {
            isConnected = false;
            chatPanel.SetActive(false);
        }
    }

    public void OnConnected()
    {
        Debug.Log("Connected to chat server");
        isConnected = true;  

        chatClient.Subscribe(new string[] { "RegionChannel" });
    }

    public void OnDisconnected()
    {
        Debug.Log("Disconnected from chat server");
        isConnected = false;  
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            msgs = string.Format("{0}: {1}", senders[i], messages[i]);
            chatDisplay.text += "\n" + msgs;  // Mesajı chat display'de göster
            Debug.Log(msgs);

            if(!isChatOpen)
            {
                StartCoroutine(ShowMessageTemporarily(10f));  // 10 saniye boyunca mesajı göster
            }
        }
    }

    private IEnumerator ShowMessageTemporarily(float delay)
    {
        // Sadece chat display'i aktif yap
        chatDisplay.gameObject.SetActive(true);
        chatButton.gameObject.SetActive(false);
        chatPanel.SetActive(true); 


        yield return new WaitForSeconds(delay);

        // 10 saniye sonra chat display'i tekrar gizle
        chatDisplay.gameObject.SetActive(false);
        chatButton.gameObject.SetActive(false);
        chatPanel.SetActive(false);  
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string msgs = string.Format("(Private) {0}: {1}", sender, message);
        chatDisplay.text += "\n " + msgs;
        Debug.Log(msgs);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed to channel: " + string.Join(", ", channels));
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("Unsubscribed from channel: " + string.Join(", ", channels));
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }
    #endregion Callbacks
}
