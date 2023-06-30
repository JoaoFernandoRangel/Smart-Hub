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
    bool ativou = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Copo") && !ativou)
        {
            ativou = true;
            StartCoroutine(PrepararParaAtirarEvento());
        }
    }

    IEnumerator PrepararParaAtirarEvento()
    {

        yield return new WaitForSeconds(segundosParaAtivaSensor);
        eventoDeDisparo.Invoke();
        ativou = false;
    }
}
