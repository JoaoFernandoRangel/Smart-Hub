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
    private Vector3 offsetRotacao; // Deslocamento (offset) a ser aplicado à posição

    [SerializeField]
    TrackingScript trackingScript;
    [SerializeField]
    float rotacaoTransformadaEscopo;

    public float velocidadeDoLerp = 2;

    private void Start()
    {
        trackingScript.GarraValueChanged += TrackingScript_GarraValueChanged;
    }
    private void Update()
    {
        if (objetoASeguir != null && objetoARodar != null)
        {
            Vector3 posicaoAlvo = objetoASeguir.position + offset;
            transform.position = posicaoAlvo;
            transform.rotation = objetoASeguir.rotation;

            Quaternion novaRotacao = objetoASeguir.rotation;
            novaRotacao *= Quaternion.Euler(0 + offsetRotacao.x, rotacaoTransformadaEscopo + offsetRotacao.y, 0 + offsetRotacao.z);

            // Interpola suavemente entre a rotação atual e a nova rotação
            objetoARodar.transform.rotation = Quaternion.Slerp(objetoARodar.transform.rotation, novaRotacao, Time.deltaTime * velocidadeDoLerp);
        }
    }


    private void TrackingScript_GarraValueChanged(string arg1, string arg2, string arg3, string arg4)
    {
        float.TryParse(arg4, out float rotacaoTransformada);/*
        if (rotacaoTransformada > 0)
        {
            rotacaoTransformada += 90;
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
        }*/
        rotacaoTransformadaEscopo = rotacaoTransformada;


    }



}
