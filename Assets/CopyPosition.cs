using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CopyPosition : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField]
    GameObject objetoASerSeguido;
    [SerializeField]
    TrackingScript trackingScript;

    [Header("Valores")]
    [SerializeField]
    float xValueFloatDividido;
    [SerializeField]
    float yValueFloatDividido;
    [SerializeField]
    float zValueFloatDividido;

    [Header("Configurações de Interpolação")]
    [SerializeField]
    float interpSpeed = 2f; // Ajuste conforme necessário
    Vector3 targetPosition;

    private void Start()
    {
        trackingScript.GarraValueChanged += TrackingScript_GarraValueChanged;
    }

    private void TrackingScript_GarraValueChanged(string xValue, string yValue, string zValue, string rValue)
    {
        float.TryParse(xValue, out float xValueFloat);
        float.TryParse(yValue, out float yValueFloat);
        float.TryParse(zValue, out float zValueFloat);

        xValueFloatDividido = (xValueFloat / 1000) * -1;
        yValueFloatDividido = yValueFloat / 1000;
        zValueFloatDividido = zValueFloat / 1000;
        MainThreadDispatcher.Instance.Enqueue(() => IrParaPos());
    }

    private void IrParaPos()
    {
        targetPosition = new Vector3(xValueFloatDividido, yValueFloatDividido, zValueFloatDividido);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = objetoASerSeguido.transform.position;


        objetoASerSeguido.transform.localPosition = Vector3.Lerp(objetoASerSeguido.transform.localPosition, targetPosition, Time.deltaTime * interpSpeed);

    }
}
