using UnityEngine;

/// <summary>
/// Controla o movimento e colisão dos obstáculos
/// </summary>
public class Obstaculos : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float limiteEsquerda = -10f; // Limite para destruição
    
    private float velocidade; // Velocidade de movimento (controlada pelo GameManager)
    
    void Start()
    {
        // Obtém a velocidade atual do GameManager
        if (GameManager.Instance != null)
        {
            velocidade = GameManager.Instance.GetVelocidadeAtual();
        }
        else
        {
            velocidade = 5f; // Velocidade padrão caso GameManager não exista
        }
    }
    
    void Update()
    {
        MoverObstaculo();
        VerificarLimite();
    }
    
    /// <summary>
    /// Move o obstáculo para a esquerda
    /// </summary>
    private void MoverObstaculo()
    {
        transform.Translate(Vector3.left * velocidade * Time.deltaTime);
    }
    
    /// <summary>
    /// Destroi o obstáculo quando sai da tela
    /// </summary>
    private void VerificarLimite()
    {
        if (transform.position.x < limiteEsquerda)
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Detecta colisão com o player e chama Game Over
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Notifica o GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            
            // Destroi o player
            Destroy(collision.gameObject);
        }
    }
}
