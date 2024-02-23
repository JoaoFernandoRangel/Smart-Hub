using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarraFollowBody : MonoBehaviour
{
    [SerializeField]
    private GameObject objetoARodar; // Refer�ncia ao objeto que voc� deseja seguir
    [SerializeField]
    private Transform objetoASeguir; // Refer�ncia ao objeto que voc� deseja seguir

    [SerializeField]
    private Vector3 offset; // Deslocamento (offset) a ser aplicado � posi��o

    [SerializeField]
    TrackingScript trackingScript;
    [SerializeField]
    float rotacaoTransformadaEscopo;
    [SerializeField]
    float valorASomado =270;
    [SerializeField]
    Vector3 offsetRotation;
    public float velocidadeDoLerp = 2;

    private void Start()
    {
        trackingScript.GarraValueChanged += TrackingScript_GarraValueChanged;
    }
    private void Update()
    {
        // Verifica se a refer�ncia do objeto a seguir est� configurada
        if (objetoASeguir != null && objetoARodar != null)
        {
            // Obt�m a posi��o atual do objeto a seguir com o offset
            Vector3 posicaoAlvo = objetoASeguir.position + offset;

            // Move este objeto em dire��o � posi��o alvo suavemente
            transform.position = posicaoAlvo;

            // Copia a rota��o do objeto de refer�ncia
            transform.rotation = objetoASeguir.rotation;

            // Copia a rota��o do objeto pai
            Quaternion novaRotacao = objetoASeguir.rotation;

            // Substitui a rota��o em torno do eixo Y pelo valor de rotacaoTransformadaEscopo
            novaRotacao *= Quaternion.Euler(0 + offsetRotation.x, rotacaoTransformadaEscopo + offsetRotation.y, 0 + offsetRotation.z);

            // Aplica a nova rota��o ao objetoARodar
            objetoARodar.transform.rotation = novaRotacao;
        }
    }

    private void TrackingScript_GarraValueChanged(string arg1, string arg2, string arg3, string arg4)
    {
        float.TryParse(arg4, out float rotacaoTransformada);

        // Converta a rota��o para se alinhar corretamente com a orienta��o desejada
        float rotacaoConvertida = 0f;
        /*
        if (rotacaoTransformada > 0)
        {
            rotacaoConvertida = -rotacaoTransformada;
        }
        else if (rotacaoTransformada < 0)
        {
            rotacaoConvertida = (rotacaoTransformada * -1)  + valorASomado;
            print((rotacaoTransformada * -1) + " + " + valorASomado + " = " + rotacaoConvertida);
        
        }*/
        print(rotacaoConvertida);
        // Ajuste a rota��o para o objeto seguir corretamente
        rotacaoTransformadaEscopo = rotacaoTransformada * -1;
    }




}
