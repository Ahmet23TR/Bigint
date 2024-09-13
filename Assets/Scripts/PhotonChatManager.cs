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
    private bool isChatOpen = false;        

    void Start()
    {
        // Başlangıçta chat paneli kapalı 
        chatPanel.SetActive(false);
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

        if (isChatOpen && !isConnected)
        {
            ChatConnectOnClick();
        }

        // Karakter hareketini durdur/aktif et
        var playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
    {
        playerMovement.isChatOpen = isChatOpen;

        // Chat açıldığında animasyonları durdur
        if (isChatOpen)
        {
            Animator animator = playerMovement.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }
    }

        if (isChatOpen)
        {
            chatField.ActivateInputField();
            DisableCharacterMovement();
        }
        else
        {
            EnableCharacterMovement();
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
        var playerController = FindObjectOfType<PlayerMovement>(); 
        if (playerController == null)
        {
            Debug.LogError("PlayerController bulunamadı!");
            return;
        }

        var networkObject = playerController.GetComponent<NetworkObject>();
        if (networkObject == null)
        {
            Debug.LogError("NetworkObject bulunamadı!");
            return;
        }

        if (networkObject.HasInputAuthority)
        {
            Debug.Log("Yerel oyuncu, hareket devre dışı bırakılıyor.");
            playerController.enabled = false; 
        }
        else
        {
            Debug.Log("Bu oyuncu yerel değil, hareketi devre dışı bırakmıyoruz.");
        }
    }

    void EnableCharacterMovement()
    {
        var playerController = FindObjectOfType<PlayerMovement>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController bulunamadı!");
            return;
        }

        var networkObject = playerController.GetComponent<NetworkObject>();
        if (networkObject == null)
        {
            Debug.LogError("NetworkObject bulunamadı!");
            return;
        }

        if (networkObject.HasInputAuthority)
        {
            Debug.Log("Yerel oyuncu, hareket tekrar etkinleştiriliyor.");
            playerController.enabled = true; 
        }
    }

    #endregion Setup

    #region General
    string privateReceiver = "";
    string currentChat;
    [SerializeField] Text chatDisplay;

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
            chatDisplay.text += "\n" + msgs;
            Debug.Log(msgs);
        }
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
