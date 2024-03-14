using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtualizarGUI : MonoBehaviour
{

    [Header("UI")]
    [SerializeField]
    private TMP_Text TMP_TextepochTimeA;
    [SerializeField]
    private TMP_Text TMP_TextepochTimeB;
    [SerializeField]
    private TMP_Text TMP_TextepochTimeUnity;
    [SerializeField]
    private TMP_Text TMP_TextepochTimeDiference;
    [Header("ref")]
    [SerializeField]
    private TrackingScript trackingScript;

    public void Atualizar()
    {
        TMP_TextepochTimeA.text = trackingScript.epochTimeA;
        TMP_TextepochTimeB.text = trackingScript.epochTimeB;
        TMP_TextepochTimeUnity.text = trackingScript.epochTimeUnity;
        TMP_TextepochTimeDiference.text = trackingScript.epochTimeDiferenca;
    }


}
