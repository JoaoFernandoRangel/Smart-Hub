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
    // Credenciais de conexão
    // string user = "DigitalTwin";
    // string pass = "Digital7w1n";
    // string server = "be7ffc1c90054731998da6666ee7b112.s2.eu.hivemq.cloud";
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

        if (mqtt.pistao.Count > 0)
        {
            mensagem = mqtt.pistao.Last();
            
            if (lastPis != $"{mqtt.pistao.Count} - {mensagem}")
            {
                lastPis = $"{mqtt.pistao.Count} - {mensagem}";

                print(mensagem);


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
}

class Mqtt : MonoBehaviour
{
    //DADOS DE CONEXÃO COM O SERVIDOR
    public string user { private set; get; }
    public string pass { private set; get; }
    public string server { private set; get; }
    public int port { private set; get; }
    public string topic { private set; get; }
    public MqttClient client { private set; get; }

    //VARIÁVEIS DE FUNCIONAMENTO
    public bool msgToReadPis { private set; get; } //Retorna se há mensagens não "lidas" do pistao
    public bool msgToReadPos { private set; get; } //Retorna se há mensagens não "lidas" da posicao

    //private static readonly List<Message> messages = new List<Message>();

    //Lista que armazena as mensagens do pistão
    private List<Message> Pistao = new List<Message>();


    public List<Message> pistao // 0 -> timestamp, 1 -> pistão, 2 -> giroscópio
    {
        get
        {
            msgToReadPis = false;
            return Pistao;
        }
    }

    //Lista que armazena as mensagens de posicao
    private List<Message> Posicao = new List<Message>();
    public List<Message> posicao // 0 -> timestamp, 1 -> pistão, 2 -> giroscópio
    {
        get
        {
            msgToReadPos = false;
            return Posicao;
        }
    }

    //######################################################################################################

    //MÉTODOS DA CLASSE
    public Mqtt(string tpc = "topic", int prt = 8883, string usr = "DigitalTwin", string pss = "Digital7w1n", string svr = "be7ffc1c90054731998da6666ee7b112.s2.eu.hivemq.cloud")
    {
        // Adicionando as credenciais nas variáveis
        user = usr; pass = pss; server = svr; port = prt; topic = tpc;

        // criar uma nova instância do cliente MQTT
        X509Certificate caCert = X509Certificate.CreateFromCertFile("isrgrootx1.pem");
        client = new MqttClient(server, port, true, caCert, null, MqttSslProtocols.TLSv1_2);
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; // manipulador para o recebimento de mensagens
        Conectar();
    }

    public void Conectar()
    {
        // conectar ao broker MQTT e ao tópico
        int retries = 0;

        while (!client.IsConnected)
        {
            client.Connect("Terminal", user, pass);
            retries++;

            if (retries > 10)
            {
                print("Erro De Conexão");
                break;
            }
        }

        client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        print("Conectado ao tópico.");
    }

    private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {

        // mensagem recebida    
        string[] msg = Encoding.UTF8.GetString(e.Message).Replace("%%","%").Split('%');

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
    }
}
