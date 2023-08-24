using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class algorithm : MonoBehaviour
{



    [Header("Piston Reference")]
    [SerializeField]
    PistaoAnimatorScript pistonA;   
    [SerializeField]
    PistaoAnimatorScript pistonB; 
    [SerializeField]
    PistaoAnimatorScript pistonC;

    [Header("Container Reference")]
    [SerializeField]
    GameObject Container;
    [SerializeField]
    GameObject spotToGoBack;




    [Header("Velocity")]
    [SerializeField]
    float slowDownRatioValue = 0.01f;

    
    float ultimoTempoRegistrado = 1;

    [SerializeField]
    int iteration = 0;

    [SerializeField]
    float currentCycleTime = 0;

    [SerializeField]
    float bestTime = 0;

    float tempoParaOCopoVoltar = 0;
    [Header("Piston Velocity")]
    [SerializeField]
    float pistonVelocityA ;
    [SerializeField]
    float pistonVelocityB;
    [SerializeField]
    float pistonVelocityC;
    [Header("Maximum Piston Velocity")]
    [SerializeField]
    float fastestPistonSpeedA;
    [SerializeField]
    float fastestPistonSpeedB;
    [SerializeField]
    float fastestPistonSpeedC;

    [SerializeField]
    FallDetector fallDetector;

    AnimatorReset animatorReset;


    [Header("UI")]
    [SerializeField]
    TMP_Text iterationText;
    [SerializeField]
    TMP_Text slowDownRatioValueText;
    [SerializeField]
    TMP_Text currentCycleTimeText;
    [SerializeField]
    TMP_Text bestTimeText;
    [SerializeField]
    TMP_Text pistonVelocityAText;
    [SerializeField]
    TMP_Text pistonVelocityBText;
    [SerializeField]
    TMP_Text pistonVelocityCText; 
    
    [SerializeField]
    TextOpacity textOpacity;


    float inGameValue = 4f;

    float inGameMin = 0f;


    float inGameMax = 5f;

    float realMin = 0f;

    float realMax = 1.23f;



    bool resetTime = false;
    Vector3 initialRotation;

    private static algorithm _instance;

    public static algorithm Instance { get { return _instance; } }



    public void UpdateGraphics()
    {
        iterationText.text = iteration.ToString();
        slowDownRatioValueText.text = slowDownRatioValue.ToString();
        currentCycleTimeText.text = currentCycleTime.ToString() + " seconds";
        bestTimeText.text = bestTime.ToString() + " seconds";
        pistonVelocityAText.text = pistonVelocityA.ToString() + " m/s";
        pistonVelocityBText.text = pistonVelocityB.ToString() + " m/s";
        pistonVelocityCText.text = pistonVelocityC.ToString() + " m/s";
    }

    private void Awake()
    {
        // Check if an instance already exists
        if (_instance != null && _instance != this)
        {
            // Destroy the duplicate instance
            Destroy(gameObject);
        }
        else
        {
            // Set this instance as the singleton
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    private float ReverseScale(float inGameValue, float inGameMin, float inGameMax, float realMin, float realMax)
    {
        float realValue = ((inGameValue - inGameMin) / (inGameMax - inGameMin)) * (realMax - realMin) + realMin;
        return realValue;
    }


    private void Start()
    {
        UpdateGraphics();
        resetTime = false;
        currentCycleTime = 0;
        RedefinirMelhorVelocidadePistao();
        animatorReset = GetComponent<AnimatorReset>();
        AtualizarParametros();

        initialRotation = transform.eulerAngles;
    }

    private void AtualizarParametros()
    {
        pistonVelocityA = ReverseScale(pistonA.velocidade, inGameMin, inGameMax, realMin, realMax);
        pistonVelocityB= ReverseScale(pistonB.velocidade, inGameMin, inGameMax, realMin, realMax);
        pistonVelocityC = ReverseScale(pistonC.velocidade, inGameMin, inGameMax, realMin, realMax);

    }

    private void RedefinirMelhorVelocidadePistao()
    {
        if (currentCycleTime < bestTime)
        {
            if (pistonA.velocidade > fastestPistonSpeedA)
            {
                fastestPistonSpeedA = pistonA.velocidade;
            }
            if (pistonB.velocidade > fastestPistonSpeedA)
            {
                fastestPistonSpeedB = pistonB.velocidade;
            }
            if (pistonC.velocidade > fastestPistonSpeedA)
            {
                fastestPistonSpeedC = pistonC.velocidade;
            }
        }

        fastestPistonSpeedA = pistonA.velocidade;
        fastestPistonSpeedB = pistonB.velocidade;
        fastestPistonSpeedC = pistonC.velocidade;
    }

    private void Update()
    {
        AtualizarParametros();
        UpdateGraphics();
        if (!resetTime)
        {

            currentCycleTime += Time.deltaTime;

        }



        if (currentCycleTime > 25)
        {
            pistonA.ajustarVelocidade(pistonA.velocidade - slowDownRatioValue);
            pistonB.ajustarVelocidade(pistonB.velocidade - slowDownRatioValue);
            pistonC.ajustarVelocidade(pistonC.velocidade - slowDownRatioValue);

            StartTeleport(tempoParaOCopoVoltar);
            currentCycleTime = 0;

        }
    }
    public void ChegouFinal()
    {
        print("chegou final");
        bestTime = currentCycleTime;
        RecomecarTempo();
        AtualizarParametros();

        RedefinirMelhorVelocidadePistao();

        pistonA.ajustarVelocidade(pistonA.velocidade - slowDownRatioValue);
        pistonB.ajustarVelocidade(pistonB.velocidade - slowDownRatioValue);
        pistonC.ajustarVelocidade(pistonC.velocidade - slowDownRatioValue);

        StartTeleport(tempoParaOCopoVoltar);
    }

    public void StartTeleport(float time)
    {
        animatorReset.PlayDefaultAnimations();
        StartCoroutine(TeleportAfterDelay(time));
    }

    private IEnumerator TeleportAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        print("recomecar");
        // Teleportar o objeto para a posição do teleportTarget

        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Container.transform.position = spotToGoBack.transform.position;
        Container.transform.eulerAngles = new Vector3(-90,0,0) ;
    }
    public void RecomecarTempo()
    {
        StartCoroutine(ResetTimer(0.2f));
    }

    private IEnumerator ResetTimer(float time)
    {
        resetTime = true;
        print("resetTime +  " + resetTime);
        iteration++;

        currentCycleTime = 0;
        yield return new WaitForSeconds(time);

        resetTime = false;
    }

    public void CopoCaiu()
    {
        textOpacity.FadeTextNow();
        RecomecarTempo();
        AtualizarParametros();

        print("Copo Caiu!");
        pistonA.ajustarVelocidade(pistonA.velocidade - slowDownRatioValue);
        pistonB.ajustarVelocidade(pistonB.velocidade - slowDownRatioValue);
        pistonC.ajustarVelocidade(pistonC.velocidade - slowDownRatioValue);
        StartTeleport(tempoParaOCopoVoltar);
   
    }


}
