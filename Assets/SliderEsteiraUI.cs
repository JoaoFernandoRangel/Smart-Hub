using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderEsteiraUI : MonoBehaviour
{
    [SerializeField]
    Slider sliderEsteira;
    [SerializeField]
    TMP_Text velocidadeText;
    [SerializeField]
    public float velocidade = 0.1f;
    void Start()
    {
         velocidade= sliderEsteira.value;
        velocidadeText.text = velocidade.ToString();

    }
    private void Update()
    {
        velocidadeText.text = velocidade.ToString();
        velocidade = sliderEsteira.value;
    }


}
