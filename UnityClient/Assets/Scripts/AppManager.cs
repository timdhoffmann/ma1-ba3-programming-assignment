using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    [SerializeField] private string _tcpServerAddress = "127.0.0.1";
    [SerializeField] private int _tcpPort = 13000;
    [SerializeField] private Button _sendButton;
    [SerializeField] private Button _connectButton;
    [SerializeField] private InputField _input;
    [SerializeField] private Text _output;
    [SerializeField] private Text _statusText;
    [SerializeField] private ScrollRect _scrollRect;

    private NetworkConnection _connection;

    private void Start()
    {
        Assert.IsNotNull(_sendButton);
        Assert.IsNotNull(_connectButton);
        Assert.IsNotNull(_input);
        Assert.IsNotNull(_output);
        Assert.IsNotNull(_statusText);
        Assert.IsNotNull(_scrollRect);

        Init();
    }

    private void Init()
    {
        _output.text = string.Empty;

        _connectButton.onClick.AddListener(Connect);
        _connectButton.GetComponentInChildren<Text>().text = "Connect";

        _statusText.text = "Press button to connect.";
    }

    private void Connect()
    {
        _connection = new NetworkConnection();
        _connection.EstablishConnectionWithServer(
            _tcpServerAddress,
            _tcpPort);

        if (_connection.IsConnectedToServer)
        {
            _sendButton.onClick.AddListener(SendMessage);

            _connectButton.onClick.RemoveListener(Connect);
            _connectButton.onClick.AddListener(Disconnect);
            _connectButton.GetComponentInChildren<Text>().text = "Disconnect";

            _statusText.text = "Connected to server.";

            _connection.OnRecievedMessage += ReceivedMessage;

            _input.Select();
        }
    }

    private void Disconnect()
    {
        if (_connection != null)
        {
            _connection.SendDataToServer("exit");
        }
        Assert.IsNotNull(_connection);
        _connection.Disconnect();

        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(_input.text))
            SendMessage();
    }

    private void OnApplicationQuit()
    {
        _connection.Disconnect();
    }

    private void SendMessage()
    {
        _connection.SendDataToServer(_input.text);

        _input.text = string.Empty;
        _input.Select();
    }

    private void ReceivedMessage(string msg)
    {
        _output.text += msg + "\n";

        _scrollRect.verticalNormalizedPosition = 0;
    }
}