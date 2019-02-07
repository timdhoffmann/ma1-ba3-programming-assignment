using System;
using System.Net;
using System.Text;
using UnityEngine;
using System.Net.Sockets;
using System.Collections.Generic;

public class NetworkConnection
{
    private struct NetworkData
    {
        public byte[] content;

        public NetworkData(byte[] _content)
        {
            content = _content;
        }
    }

    public delegate void RecievedMessageHandler(string message);

    public event RecievedMessageHandler OnRecievedMessage;

    public delegate void SocketErrorHandler(SocketException error);

    public event SocketErrorHandler OnSocketError;

    private Queue<NetworkData> sendQueue;
    private byte[] bufferedData;
    private int bufferedLength = 0;
    private bool isSendDone = true;
    private byte[] _recieveBuffer = new byte[8096];
    private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    public NetworkConnection()
    {
    }

    private void instance_OnGameReloded()
    {
        OnRecievedMessage = null;
    }

    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    // - Server connection

    public bool IsConnectedToServer
    {
        get { return _socket.Connected; }
    }

    public void Disconnect()
    {
        if (_socket.Connected)
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Disconnect(false);
            _socket.Close();

            Debug.Log("Disconnected from server");
        }
    }

    public bool EstablishConnectionWithServer(string serverAddress, int serverPort)
    {
        try
        {
            if (_socket.Connected)
            {
                Debug.Log("Already connected to server");

                return true;
            }

            _socket.Connect(new IPEndPoint(IPAddress.Parse(serverAddress), serverPort));

            Loom.StartSingleThread(() =>
            {
                _socket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
            });

            _socket.SendTimeout = 10;

            Debug.Log("Connect to server Successfully ");
            Debug.Log("Tcp Address: " + serverAddress + "TCP port: " + serverPort);

            if (sendQueue == null)
                sendQueue = new Queue<NetworkData>();

            if (sendQueue.Count > 0 && isSendDone == true && IsConnectedToServer)
                SendFirstQueuedData();

            return true;
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.ToString());
        }

        return false;
    }

    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    // - Send data

    public void SendDataToServer(string content)
    {
        Debug.Log(content);
        byte[] byteContent = Encoding.UTF8.GetBytes(content + "\r\n");

        SendDataToServer(byteContent);
    }

    protected void SendDataToServer(byte[] contentArray)
    {
        MakeDataFromByteArray(contentArray);
    }

    private void MakeDataFromByteArray(byte[] src)
    {
        //byte[] data;

        //data = new byte[src.Length];
        //data[0] = (byte)msgType;

        //Buffer.BlockCopy(BitConverter.GetBytes((System.Int16)src.Length), 0, data, 1, 2);
        //Buffer.BlockCopy(src, 0, data, 3, src.Length);

        //ProcessSendQueue(msgType, data);

        ProcessSendQueue(src);
    }

    private void ProcessSendQueue(byte[] data)
    {
        NetworkData newData = new NetworkData(data);

        sendQueue.Enqueue(newData);

        if (sendQueue.Count == 1 && isSendDone == true && IsConnectedToServer)
        {
            isSendDone = false;

            SendData(sendQueue.Dequeue().content);
        }
    }

    private void SendData(byte[] data)
    {
        try
        {
            int i = _socket.Send(data);

            Debug.LogFormat("Sent {0} bytes.", i);

            isSendDone = true;
            if (sendQueue.Count > 0 && isSendDone == true && IsConnectedToServer)
            {
                isSendDone = false;
                SendFirstQueuedData();
            }
        }
        catch (SocketException e)
        {
            Debug.LogFormat("{0} Error code: {1}.", e.Message, e.ErrorCode);

            if (OnSocketError != null)
                OnSocketError(e);
        }
    }

    private void SendFirstQueuedData()
    {
        byte[] content = sendQueue.Dequeue().content;

        SendData(content);
    }

    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    // - Receive data

    private void ReceiveCallback(IAsyncResult AR)
    {
        int received = _socket.EndReceive(AR);
        Debug.LogFormat("Recieved Length : {0}", received);

        if (received > 0)
        {
            byte[] recData = new byte[received];
            Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, received);

            byte[] dataToProcess;
            dataToProcess = new byte[recData.Length + bufferedLength];
            if (bufferedData != null)
                Buffer.BlockCopy(bufferedData, 0, dataToProcess, 0, bufferedLength);
            Buffer.BlockCopy(recData, 0, dataToProcess, bufferedLength, recData.Length);

            //if (bufferedLength != 0)
            //bufferedLength = 0;

            ProcessIncomingData(dataToProcess,
                () =>
                {
                    // Process finished sucessfully
                    Loom.StartSingleThread(() =>
                    {
                        _socket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
                    });
                },
                (byte[] incompleteMessage) =>
                {
                    // Process interupted
                    bufferedLength = incompleteMessage.Length;
                    bufferedData = new byte[bufferedLength];
                    Buffer.BlockCopy(incompleteMessage, 0, bufferedData, 0, bufferedLength);

                    Loom.StartSingleThread(() =>
                    {
                        _socket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
                    });
                });
        }
        else
        {
            Loom.DispatchToMainThread(() =>
            {
                Debug.Log("Connection Closed");

                if (OnSocketError != null)
                    OnSocketError(null);
            });
        }
    }

    private void ProcessIncomingData(byte[] incomingData,
                                            Action processDidFinished,
                                            Action<byte[]> processDidInterupt)
    {
        int index = 0;
        int headerSize = 6;

        while (true)
        {
            try
            {
                var stringData = Encoding.UTF8.GetString(incomingData);
                //int contentLength   = int.Parse(stringData.Substring(0, headerSize));
                //int messageSize     = contentLength + headerSize;

                //if (messageSize > incomingData.Length - index)
                //{
                //    int incompleteDataLength    = incomingData.Length - index;
                //    byte[] incompleteData       = new byte[incompleteDataLength];
                //    Buffer.BlockCopy(incomingData, index, incompleteData, 0, incompleteDataLength);
                //    processDidInterupt(incompleteData);
                //    Debug.LogFormat("Waiting for more data for message");

                //    return;
                //}

                var output = stringData; //stringData.Substring(6, contentLength);

                Debug.Log("Received Data: " + output);

                Loom.DispatchToMainThread(() =>
                {
                    if (OnRecievedMessage != null)
                        OnRecievedMessage(output);
                });

                break;

                //if (stringData.Length > messageSize)
                //{
                //    stringData      = stringData.Remove(0, messageSize);
                //    incomingData    = Encoding.UTF8.GetBytes(stringData);
                //    index           = 0;

                //    continue;
                //}

                //index += messageSize;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

            if (incomingData.Length > index)
                continue;
            else
                break;
        }

        processDidFinished();
    }
}