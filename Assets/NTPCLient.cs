using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NTPCLient : MonoBehaviour
{
    public string ntpServer = "europe.pool.ntp.org";
    public int ntpPort = 123;

    private UdpClient udpClient;
    [SerializeField]
    public Int64 epochTimeUnity;


    void Start()
    {
        /*udpClient = new UdpClient(ntpServer, ntpPort);
        udpClient.BeginReceive(ReceiveCallback, null);*/
        GetNetworkTimeInSeconds() ;
    }

    void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = udpClient.EndReceive(result, ref remoteEP);

            ulong intPart = BitConverter.ToUInt32(receiveBytes, 40);
            ulong fractPart = BitConverter.ToUInt32(receiveBytes, 44);

            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);

            Debug.Log("Ping to " + ntpServer + " is " + timeSpan.TotalMilliseconds.ToString("F2") + "ms");

            udpClient.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving NTP response: " + e.Message);
        }
    }
    public double GetNetworkTimeInSeconds()
    {
        const string ntpServer = "europe.pool.ntp.org";
        var ntpData = new byte[48];
        ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

        var addresses = Dns.GetHostEntry(ntpServer).AddressList;
        var ipEndPoint = new IPEndPoint(addresses[0], 123);
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        socket.Connect(ipEndPoint);
        socket.Send(ntpData);
        socket.Receive(ntpData);
        socket.Close();

        ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
        ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
        var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

        // Calculando o Epoch time em segundos
        var epochTimeInSeconds = (networkDateTime - new DateTime(1970, 1, 1)).TotalSeconds;

        print("NTP Data: " + ntpData);
        print("Network DateTime: " + networkDateTime);
        print("Epoch Time in Miliseconds: " + epochTimeInSeconds * 1000);
        epochTimeUnity = (Int64)(epochTimeInSeconds * 1000);



        /*

        Epoch Time in Seconds: 1711475141,259
        Epoch Time in Miliseconds: 1711475238516
        */
        return epochTimeInSeconds;
    }


}
