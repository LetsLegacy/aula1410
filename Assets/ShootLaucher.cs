using UnityEngine;
using Fusion;
using TMPro;


public class ShootLaucher : MonoBehaviour
{
    [SerializeField] NetworkRunner runner;
    [SerializeField] TMP_InputField roomInput;
    [SerializeField] TMP_Text connectionStatus;
    [SerializeField] GameObject menuPanel;

    private bool connecting;

    public async void ConnectToRoom()
    {
        if (connecting || runner.IsRunning) return;

        string roomName = roomInput.text;

        if (string.IsNullOrEmpty(roomName))
        {
            connectionStatus.text = "Digite o nome da sala";
            return;
        }

        connecting = true;
        connectionStatus.text = "Conectando...";

        runner.ProvideInput = false;

        var sceneManager = GetComponent<NetworkSceneManagerDefault>();

        var result = await runner.StartGame(
            new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = roomName,
                PlayerCount = 2,
                SceneManager = sceneManager,
            }
        );

        if (result.Ok)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            connectionStatus.text = "Erro: " + result.ShutdownReason;
            Debug.LogError(result.ShutdownReason);
        }

        connecting = false;
    }


}
