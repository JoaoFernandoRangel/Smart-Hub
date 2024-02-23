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
            // Obtenha a rotação do objeto de referência
            Quaternion rotacaoReferencia = objetoReferencia.rotation;

            // Ajuste a rotação do objeto atual, mantendo a normal para baixo
            transform.rotation = Quaternion.Euler(rotacao.x, rotacaoReferencia.eulerAngles.y+ rotacao.y, rotacao.z);

            // Atualize a posição do objeto atual com a posição do objeto de referência
            transform.position = objetoReferencia.position;
        }
        else
        {
            Debug.LogWarning("Objeto de referência não atribuído.");
        }
    }
}
