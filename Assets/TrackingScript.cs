using System;
using System.Text;
using TMPro;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class TrackingScript : MonoBehaviour
{
    private MqttClient client;

    private string brokerAddress = "dd6e8d1cc8524360a537e7db4e5924f8.s2.eu.hivemq.cloud";
    private int brokerPort = 8883;
    private string username = "DigitalTwin";
    private string password = "Digital7w1n";
    private string[] topics = { "pistao", "xyzr" ,"garra"};
    [Header("Valores dos pistões")]
    [SerializeField]
    private string pistaoA;
    [SerializeField]
    private string pistaoB;
    [SerializeField]
    private string pistaoC;
    [SerializeField]
    private string pistaoD;
    [Header("Valores da posição da Garra")]
    [SerializeField]
    private string garraX;
    [SerializeField]
    private string garraY;
    [SerializeField]
    private string garraZ;
    [SerializeField]
    private string rotacaoGarra;
    [SerializeField]
    private string aberturaGarra;
    [Header("Epoch Time")]
    [SerializeField]
    public string epochTimeA;
    [SerializeField]
    public string epochTimeB;
    [SerializeField]
    public long epochTimeUnity;
    [SerializeField]
    public string epochTimeDiferenca;
    [Header("Valores do Potenciômetro da Garra")]
    [SerializeField]
    private string valorPotenciometroGarra;



    [Header("UI")]
    [SerializeField]
    public AtualizarGUI atualizarGUI;
    [Header("NTP")]
    [SerializeField]
    public NTPCLient ntpcCLient;
    public string PistaoA { get => pistaoA; set => pistaoA = value; }
    public string PistaoB { get => pistaoB; set => pistaoB = value; }
    public string PistaoC { get => pistaoC; set => pistaoC = value; }
    public string PistaoD { get => pistaoD; set => pistaoD = value; }
    public string GarraX { get => garraX; set => garraX = value; }
    public string GarraY { get => garraY; set => garraY = value; }
    public string GarraZ { get => garraZ; set => garraZ = value; }
    public string RotacaoGarra { get => rotacaoGarra; set => rotacaoGarra = value; }
    public string AberturaGarra { get => aberturaGarra; set => aberturaGarra = value; }

    public event Action<char, string> PistaoValueChanged;
    public event Action<string, string, string, string> GarraValueChanged;

    private void Start()
    {
        client = new MqttClient(brokerAddress, brokerPort, true, null, null, MqttSslProtocols.TLSv1_2);
        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
        ConnectToBroker();
    }

    private void ConnectToBroker()
    {
        client.Connect(Guid.NewGuid().ToString(), username, password);

        foreach (var topic in topics)
        {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            print($"Conectado ao tópico: {topic}");
        }
    }

    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string topic = e.Topic;
        string message = Encoding.UTF8.GetString(e.Message);




        if (topic == "pistao" && message.Contains("PISTAO"))
        {
            UpdatePistaoValuesFromMessage(message);
        }
        else if (topic == "xyzr" && message.Contains("%%"))
        {
            ProcessGarraMessage(message);
        }        
        else if (topic == "garra" && message.Contains("%%"))
        {
            print("ProcessGarraMessage");

            ProcessPotenciometroMessage(message);
        }
        
        else
        {
            // Print the received message for other topics
            // print($"Received message on topic '{topic}': {message}");

        }
    }
    //%%PISTAO%%14:25:46%%1713450346%%A01B01C11D01
    private void UpdatePistaoValuesFromMessage(string message)
    {
        // Verifica se a mensagem começa com "%%PISTAO%%" e divide em partes
        string[] msgParts = message.Split(new[] { "%%" }, StringSplitOptions.RemoveEmptyEntries);

        // Verifica se há partes suficientes na mensagem
        if (msgParts.Length >= 2)
        {
            // A quarta parte contém os valores do pistão
            string pistaoValues = msgParts[3];

            // Loop através dos valores do pistão
            for (int i = 0; i < pistaoValues.Length - 2; i += 3)
            {
                // Extrai o nome do pistão e o valor do pistão
                char pistaoName = pistaoValues[i];

                // Verifica se ainda há caracteres suficientes para extrair o valor do pistão
                if (i + 2 < pistaoValues.Length)
                {
                    string pistaoValueStr = pistaoValues.Substring(i + 1, 2);

                    // Verifica se o nome do pistão é uma letra e se o valor é um número
                    if (char.IsLetter(pistaoName) && int.TryParse(pistaoValueStr, out int pistaoValue))
                    {
                        // Atualiza os valores do pistão usando o método existente
                        UpdatePistaoValues(pistaoName, pistaoValueStr);
                    }
                    else
                    {
                        // Caso contrário, há um erro na análise do valor do pistão
                        // Você pode lidar com isso de acordo com sua lógica
                        // print($"Error parsing Pistão {pistaoName} value.");
                    }
                }
            }
        }
    }

    //1713462309876%%14-45-09%%265,629%%-3,436%%45,296%%-18,497%%GA%%1713462310011
    private void ProcessGarraMessage(string message)
    {
        string[] garraValues = message.Split(new[] { "%%" }, StringSplitOptions.RemoveEmptyEntries);
        print(message);
        if (garraValues.Length >= 7)
        {


            epochTimeA = garraValues[0];
            epochTimeB = garraValues[7];

            // Atualize os valores da garra conforme a funcionalidade anterior
            GarraX = garraValues[3];
            GarraY = garraValues[4];
            GarraZ = garraValues[2];
            /*
            GarraX = garraValues[2];
            GarraY = garraValues[3];
            GarraZ = garraValues[1];*/
            /*
             *
             *
               ROBO      UNITY
               z     ->     Y
               y     ->     X
               x     ->     Z
             *
             *
             */

            RotacaoGarra = garraValues[5];
            AberturaGarra = garraValues[6];/*

            */
            long epochTimeALong;
            if (long.TryParse(epochTimeA, out epochTimeALong))
            {
                // Conversão bem-sucedida para long
                int epochTimeAInt = (int)epochTimeALong; // Converte de long para int, se necessário
                //print(epochTimeALong + " - " + Epoch.epochTime() * 1000 + " = " + (epochTimeALong - (Epoch.epochTime() * 1000)));
                ntpcCLient.GetNetworkTimeInSeconds();
                epochTimeUnity = ntpcCLient.epochTimeUnity;
                long epochTimeUnityLong = (long)epochTimeUnity;
                epochTimeDiferenca = (epochTimeALong - epochTimeUnityLong).ToString();

            }
            else
            {
                // Se a conversão falhar, exiba uma mensagem de erro
            }
            MainThreadDispatcher.Instance.Enqueue(() => atualizarGUI.Atualizar()); 

            


            // Invoke event to notify listeners about the updated garra values
            GarraValueChanged?.Invoke(GarraX, GarraY, GarraZ, RotacaoGarra);
        }
    }

    //%%GARRA%%14:41:35%%1713451295%%3254
    private void ProcessPotenciometroMessage(string message)
    {
        string[] garraValues = message.Split(new[] { "%%" }, StringSplitOptions.RemoveEmptyEntries);
        if (garraValues.Length >= 4)
        {

            // Obtém o último valor da mensagem
            string ultimoValor = garraValues[3];

            // Armazene o último valor como valor do potenciômetro da garra
            valorPotenciometroGarra = ultimoValor;

            // Restante do seu código...
        }
    }



    private void UpdatePistaoValues(char pistaoName, string pistaoValue)
    {
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
                print($"Unknown Pistão: {pistaoName}");
                break;
        }

        PistaoValueChanged?.Invoke(pistaoName, pistaoValue);
    }



    private void OnApplicationQuit()
    {
        if (client != null && client.IsConnected)
        {
            client.Disconnect();
        }
    }
}

public class Epoch
{
    /// <summary>
    /// Unix High Resolution Epoch Time (Milliseconds since UTC 1/1/1970 00:00:00)
    /// </summary>
    /// <returns>double</returns>
    public static double EpochTimeHiRes()
    {
        return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }

    /// <summary>
    /// Unix High Resolution Epoch Time (Milliseconds since UTC 1/1/1970 00:00:00)
    /// </summary>
    /// <returns>decimal</returns>
    public static decimal EpochTimeHiResAsDecimal()
    {
        return Convert.ToDecimal((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
    }

    /// <summary>
    /// Unix High Resolution Epoch Time (Milliseconds since UTC 1/1/1970 00:00:00)
    /// </summary>
    /// <returns>string</returns>
    public static string EpochTimeHiResAsString()
    {
        return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds.ToString().Replace(".", "");
    }

    /// <summary>
    /// Unix Epoch Time (Seconds since UTC 1/1/1970 00:00:00)
    /// </summary>
    /// <returns>double</returns>
    public static double epochTime()
    {
        return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }

    /// <summary>
    /// Unix Epoch Days (Days since UTC 1/1/1970 00:00:00)
    /// </summary>
    /// <returns>int</returns>
    public static int epochDays()
    {
        return (int)(((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) / 86400);
    }

    /// <summary>
    /// Unix Epoch Hours (Hours since UTC 1/1/1970 00:00:00)
    /// </summary>
    /// <returns>int</returns>
    public static int epochHours()
    {
        return (int)(((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) / 3600);
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