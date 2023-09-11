using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

class Message
{
    public string timestamp {private set; get;} = "";
    public string arrivedTimestamp {private set; get;} = "";
    public Dictionary<char,int> pistao {private set; get;} = new Dictionary<char, int>();
    public double[] posicao  {private set; get;} = new double[3];

    public Message(string ts, string dado, string tipo)
    {
        //Inserindo timestamp do instante que recebeu
        DateTime now = DateTime.Now;
        arrivedTimestamp = now.ToString("yyyy-MM-dd_HH:mm:ss:ff");

        //Inserindo timestamp enviada na mensagem
        timestamp = ts;

        //pricessando a mensagem pelo tipo dela
        if(tipo == "PISTAO")
        {
            pistaoProcessa(dado);
        }
        if(tipo == "POSICAO")
        {
            posicaoProcessa(dado);
        }
    }

    private void posicaoProcessa(string coord)
    {
        //transforma os numeros de string para double salva no vetor de posições
        string[] posicoes = coord.Split(',');
        NumberFormatInfo provider = new NumberFormatInfo();
        provider.NumberDecimalSeparator = ".";

        for(int i=0; i<3; i++)
        {
            posicao[i] =  Convert.ToDouble(posicoes[i], provider);
        }
    }

    private void pistaoProcessa(string pist)
    {
        //analise as possibilidades do pistão e retorna qual situação ele está
        for(int i = 0; i<pist.Length; i+=3)
        {
            int situacao = 4;  // 4 -> erro         
            if(pist[1+i] == '0' && pist[2+i] == '1') situacao = 1; // parado inicio
            if(pist[1+i] == '1' && pist[2+i] == '1') situacao = 2; // em movimento
            if(pist[1+i] == '1' && pist[2+i] == '0') situacao = 3; // parado final

            pistao.Add( ((char)('A'+ i/3)) , situacao);
        }
    }

    public override string ToString()
    {
        // cria a string que será passada no método Console.WriteLine
        if(pistao.Count > 0)
        {
            string pistaoString ="";
            foreach (KeyValuePair<char, int> item in pistao)
            {
                pistaoString += $"{item.Key}{item.Value},";
            }
            return $"S={timestamp} A={arrivedTimestamp}-> {pistaoString}";
        }
        else{
            return $"S={timestamp} A={arrivedTimestamp}-> x: {posicao[0]}, y: {posicao[1]}, z: {posicao[2]} ";
        }

        
    }

    public int ReturnStringPistao(char pistaoSe)
    {
        // cria a string que será passada no método Console.WriteLine
        if (pistao.Count > 0)
        {
            string pistaoString = "";
             
            foreach (KeyValuePair<char, int> item in pistao)
            {
                if (item.Key == pistaoSe)
                {
                    return item.Value;
                }
               
                pistaoString += $"{item.Key}{item.Value},";
            }
            return -1;
        }
        else
        {
            return -1;
        }
    }
}