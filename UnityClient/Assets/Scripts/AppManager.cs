using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    [SerializeField] private string TCPServerAddress;
    [SerializeField] private int TCPPort;
    [SerializeField] private Button _sendButton;
    [SerializeField] private InputField _input;
    [SerializeField] private Text _outPut;
    [SerializeField] private ScrollRect _scrollRect;

    private NetworkConnection connection;

    private void Start()
    {
        _sendButton.onClick.AddListener(Connect);
        _sendButton.GetComponentInChildren<Text>().text = "Connect";
    }

    private void Connect()
    {
        _sendButton.onClick.RemoveListener(Connect);
        _sendButton.GetComponentInChildren<Text>().text = "Send";

        connection = new NetworkConnection();
        connection.StablishConnectionWithServer(
            TCPServerAddress,
            TCPPort);

        _sendButton.onClick.AddListener(SendMessage);

        connection.OnRecievedMessage += ReceivedMessage;

        _input.Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(_input.text))
            SendMessage();
    }

    private void OnApplicationQuit()
    {
        connection.Disconnect();
    }

    private void SendMessage()
    {
        connection.SendDataToServer(_input.text);

        _input.text = string.Empty;
        _input.Select();
    }

    private void ReceivedMessage(string msg)
    {
        _outPut.text += msg + "\n";

        _scrollRect.verticalNormalizedPosition = 0;
    }
}