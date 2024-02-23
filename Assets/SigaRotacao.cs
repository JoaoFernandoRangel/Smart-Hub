using UnityEngine;

public class SigaRotacao : MonoBehaviour
{
    [SerializeField]
    private Transform objetoReferencia;
    [SerializeField]
    private Vector3 rotacao;

    void Update()
    {
        if (objetoReferencia != null)
        {
            // Obtenha a rota��o do objeto de refer�ncia
            Quaternion rotacaoReferencia = objetoReferencia.rotation;

            // Ajuste a rota��o do objeto atual, mantendo a normal para baixo
            transform.rotation = Quaternion.Euler(rotacao.x, rotacaoReferencia.eulerAngles.y+ rotacao.y, rotacao.z);

            // Atualize a posi��o do objeto atual com a posi��o do objeto de refer�ncia
            transform.position = objetoReferencia.position;
        }
        else
        {
            Debug.LogWarning("Objeto de refer�ncia n�o atribu�do.");
        }
    }
}
