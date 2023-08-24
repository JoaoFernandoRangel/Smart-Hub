using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SensorScript : MonoBehaviour
{
    [SerializeField]
    [Range(1, 10)]
    float segundosParaAtivaSensor;
    [SerializeField]
    UnityEvent eventoDeDisparo;
    [SerializeField]
    bool ativou = false;
    [SerializeField]
    GameObject copo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Copo") && !ativou)
        {
            copo = other.gameObject;
            ativou = true;
            StartCoroutine(PrepararParaAtirarEvento());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        copo = null;

    }
    IEnumerator PrepararParaAtirarEvento()
    {

        yield return new WaitForSeconds(segundosParaAtivaSensor);
        ativou = false;

        eventoDeDisparo.Invoke();
    }
}
