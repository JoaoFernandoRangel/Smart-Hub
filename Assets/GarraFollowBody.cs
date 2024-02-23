using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarraFollowBody : MonoBehaviour
{
    [SerializeField]
    private GameObject objetoARodar; // Referência ao objeto que você deseja seguir
    [SerializeField]
    private Transform objetoASeguir; // Referência ao objeto que você deseja seguir

    [SerializeField]
    private Vector3 offset; // Deslocamento (offset) a ser aplicado à posição

    [SerializeField]
    TrackingScript trackingScript;
    [SerializeField]
    float rotacaoTransformadaEscopo;
    [SerializeField]
    Vector3 offsetRotation;
    public float velocidadeDoLerp = 2;

    private void Start()
    {
        trackingScript.GarraValueChanged += TrackingScript_GarraValueChanged;
    }
    private void Update()
    {
        // Verifica se a referência do objeto a seguir está configurada
        if (objetoASeguir != null && objetoARodar != null)
        {
            // Obtém a posição atual do objeto a seguir com o offset
            Vector3 posicaoAlvo = objetoASeguir.position + offset;

            // Move este objeto em direção à posição alvo suavemente
            transform.position = posicaoAlvo;

            // Copia a rotação do objeto de referência
            transform.rotation = objetoASeguir.rotation;

            // Copia a rotação do objeto pai
            Quaternion novaRotacao = objetoASeguir.rotation;

            // Substitui a rotação em torno do eixo Y pelo valor de rotacaoTransformadaEscopo
            novaRotacao *= Quaternion.Euler(0 + offsetRotation.x, rotacaoTransformadaEscopo, 0 + offsetRotation.z);

            // Aplica a nova rotação ao objetoARodar
            objetoARodar.transform.rotation = novaRotacao;
        }
    }


    private void TrackingScript_GarraValueChanged(string arg1, string arg2, string arg3, string arg4)
    {
        float.TryParse(arg4, out float rotacaoTransformada);


        if (rotacaoTransformada > 0)
        {
            rotacaoTransformada = rotacaoTransformada * -1;
            print(rotacaoTransformada);
        }
        else if (rotacaoTransformada == 0)
        {
            rotacaoTransformada = 180;
            print(rotacaoTransformada);
        }
        else if (rotacaoTransformada < 0)
        {
            rotacaoTransformada -= 90;
            print(rotacaoTransformada);
        }



        rotacaoTransformadaEscopo = rotacaoTransformada;

    }



}
