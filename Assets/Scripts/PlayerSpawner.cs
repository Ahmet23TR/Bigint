using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    public static PlayerMovement LocalPlayerInstance; // Yerel oyuncu referansı
    public static Animator LocalPlayerAnimator; // Yerel oyuncunun Animator referansı

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var playerObject = Runner.Spawn(PlayerPrefab, new Vector3(100, 1, 100), Quaternion.identity);
            LocalPlayerInstance = playerObject.GetComponent<PlayerMovement>(); // Yerel oyuncuyu kaydet
            LocalPlayerAnimator = playerObject.GetComponent<Animator>(); // Yerel oyuncunun Animator'ını kaydet
        }
    }
}
