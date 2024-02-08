using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class TrackingScript : MonoBehaviour
{
    private MqttClient client;

    // Change these according to your MQTT broker details
    private string brokerAddress = "dd6e8d1cc8524360a537e7db4e5924f8.s2.eu.hivemq.cloud";
    private int brokerPort = 8883;
    private string username = "DigitalTwin";
    private string password = "Digital7w1n";
    private string[] topics = { "pistao", "garra" };  // Add more topics as needed

    [SerializeField]
    private string pistaoA;
    [SerializeField]
    private string pistaoB;
    [SerializeField]
    private string pistaoC;
    [SerializeField]
    private string pistaoD;

    public string PistaoA { get => pistaoA; set => pistaoA = value; }
    public string PistaoB { get => pistaoB; set => pistaoB = value; }
    public string PistaoC { get => pistaoC; set => pistaoC = value; }
    public string PistaoD { get => pistaoD; set => pistaoD = value; }

    public event Action<char, string> PistaoValueChanged;



    private void Start()
    {
        // Create a new instance of the MqttClient
        client = new MqttClient(brokerAddress, brokerPort, true, null, null, MqttSslProtocols.TLSv1_2);

        // Register to the events
        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        // Connect to the broker
        ConnectToBroker();
    }

    private void ConnectToBroker()
    {
        // Connect to the broker with a client ID, username, and password
        client.Connect(Guid.NewGuid().ToString(), username, password);

        // Subscribe to all specified topics
        foreach (var topic in topics)
        {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            print($"Conectado ao t�pico: {topic}");
        }
    }

    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        // Message received
        string topic = e.Topic;
        string message = Encoding.UTF8.GetString(e.Message);

        // Parse the received message and separate values for pist�es
        if (topic == "pistao" && message.Contains("PISTAO"))
        {
            string[] msgParts = message.Split(new[] { "%%", "%" }, StringSplitOptions.RemoveEmptyEntries);

            // Extract values for pist�es
            if (msgParts.Length >= 3)
            {
                for (int i = 0; i < msgParts[2].Length; i += 3)
                {
                    char pistaoName = msgParts[2][i];
                    string pistaoValueStr = msgParts[2].Substring(i + 1, 2);

                    if (char.IsLetter(pistaoName) && int.TryParse(pistaoValueStr, out int pistaoValue))
                    {
                        // Update Serialized Fields for each pist�o
                        UpdatePistaoValues(pistaoName, pistaoValueStr);
                        // Add the parsed value to the dictionary
                        // Print the received message and separated values
                        
                        //print($"Pist�o {pistaoName} recebe {pistaoValueStr}");
                    }
                    else
                    {
                        //print($"Error parsing Pist�o {pistaoName} value.");
                    }
                }
            }
        }
        else
        {
            // Print the received message for other topics
            
            
            //print($"Received message on topic '{topic}': {message}");
        }
    }

    private void UpdatePistaoValues(char pistaoName, string pistaoValue)
    {
        // Update Serialized Fields for each pist�o
        switch (pistaoName)
        {
            case 'A':
                PistaoA = pistaoValue;
                break;
            case 'B':
                PistaoB = pistaoValue;
                break;
            case 'C':
                PistaoC = pistaoValue;
                break;
            case 'D':
                PistaoD = pistaoValue;
                break;
            default:
                print($"Unknown Pist�o: {pistaoName}");
                break;
        }


        PistaoValueChanged?.Invoke(pistaoName, pistaoValue);

    }

    private void Update()
    {
        // Keep the application running
    }

    private void OnApplicationQuit()
    {
        // Disconnect from the broker when the application is closed
        if (client != null && client.IsConnected)
        {
            client.Disconnect();
        }
    }
}


/*
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * /*





      PIST�ES HORIZONTAIS:

        01 - PISTAO PARA TRAS
        11 - EM MOVIMENTO
        10 - PISTAO EXTENDIDO

        essa logica muda no pistao C

        PISTAO C quando extendido ele muda pra 0

      TOPICO GARRA

        tem um topico chamado garra-POS
        x y z e r da garra, sendo r a rota��o

      

















// Credenciais de conex�o
// string user = "DigitalTwin";
// string pass = "Digital7w1n";
// string server = "dd6e8d1cc8524360a537e7db4e5924f8.s2.eu.hivemq.cloud";
// int port = 8883;

Message mensagem;
string lastPis = "";
string lastPos = "";
Mqtt mqtt;
[SerializeField]
int pistao1 = 0;
[SerializeField]
int pistao2 = 0;
[SerializeField]
int pistao3 = 0;
[SerializeField]
int pistao4 = 0;

public int Pistao1 { get => pistao1; set => pistao1 = value; }
public int Pistao2 { get => pistao2; set => pistao2 = value; }
public int Pistao3 { get => pistao3; set => pistao3 = value; }
public int Pistao4 { get => pistao4; set => pistao4 = value; }

private void Start()
{
    mqtt = new Mqtt();

}
private void Update()
{
    if (!mqtt.client.IsConnected)
    {
        mqtt.Conectar();


    }
    else
    {
        //print("nao conectado");
    }

    print(mqtt.server);

    /*
    if (mqtt.pistao.Count > 0)
    {
        mensagem = mqtt.pistao.Last();
        print(mensagem);
        if (lastPis != $"{mqtt.pistao.Count} - {mensagem}")
        {
            lastPis = $"{mqtt.pistao.Count} - {mensagem}";

           // print(mensagem);


            pistao1 = mensagem.pistao['A'];
            pistao2 = mensagem.pistao['B'];
            pistao3 = mensagem.pistao['C'];
            pistao4 = mensagem.pistao['D'];

        }

    }

    if (mqtt.posicao.Count > 0)
    {
        mensagem = mqtt.posicao.Last();
        if (lastPos != $"{mqtt.posicao.Count} - {mensagem}")
        {
            lastPos = $"{mqtt.posicao.Count} - {mensagem}";
            //print(lastPos);
        }
    }
    
    Thread.Sleep(10);
}
/*
CODIGO ANTIGO
class Mqtt : MonoBehaviour
{
    //DADOS DE CONEX�O COM O SERVIDOR
    public string user { private set; get; }
    public string pass { private set; get; }
    public string server { private set; get; }
    public int port { private set; get; }
    public string topic { private set; get; }
    public MqttClient client { private set; get; }

    //VARI�VEIS DE FUNCIONAMENTO
    public bool msgToReadPis { private set; get; } //Retorna se h� mensagens n�o "lidas" do pistao
    public bool msgToReadPos { private set; get; } //Retorna se h� mensagens n�o "lidas" da posicao

    //private static readonly List<Message> messages = new List<Message>();

    //Lista que armazena as mensagens do pist�o
    private List<Message> Pistao = new List<Message>();


    public List<Message> pistao // 0 -> timestamp, 1 -> pist�o, 2 -> girosc�pio
    {
        get
        {
            msgToReadPis = false;
            return Pistao;
        }
    }

    //Lista que armazena as mensagens de posicao
    private List<Message> Posicao = new List<Message>();
    public List<Message> posicao // 0 -> timestamp, 1 -> pist�o, 2 -> girosc�pio
    {
        get
        {
            msgToReadPos = false;
            return Posicao;
        }
    }

    //######################################################################################################

    //M�TODOS DA CLASSE
    public Mqtt(string tpc = "pistao", int prt = 8883, string usr = "DigitalTwin", string pss = "Digital7w1n", string svr = "dd6e8d1cc8524360a537e7db4e5924f8.s2.eu.hivemq.cloud")
    {
        // Adicionando as credenciais nas vari�veis
        user = usr; pass = pss; server = svr; port = prt; topic = tpc;

        // criar uma nova inst�ncia do cliente MQTT
        X509Certificate caCert = X509Certificate.CreateFromCertFile("isrgrootx1.pem");
        client = new MqttClient(server, port, true, caCert, null, MqttSslProtocols.TLSv1_2);
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; // manipulador para o recebimento de mensagens
        Conectar();
    }

    public void Conectar()
    {
        // conectar ao broker MQTT e ao t�pico
        int retries = 0;

        while (!client.IsConnected)
        {
            client.Connect("Terminal", user, pass);
            retries++;

            if (retries > 10)
            {
                print("Erro De Conex�o");
                break;
            }
        }

        client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        print("Conectado ao t�pico.");
    }

    private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {

        // mensagem recebida    
        string[] msg = Encoding.UTF8.GetString(e.Message).Replace("%%", "%").Split('%');

        if (msg.Length == 5)
        {
            if (msg[1] == "PISTAO")
            {

                pistao.Add(new Message(msg[2], msg[3], "PISTAO"));
                msgToReadPis = true;
            }
            if (msg[1] == "POSICAO")
            {
                posicao.Add(new Message(msg[2], msg[3], "POSICAO"));
                msgToReadPos = true;
            }
        }
    }*/