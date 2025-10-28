using UnityEngine;

/// <summary>
/// Controla o movimento do player entre as plataformas usando W e S
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float[] posicoesY = { -2f, 1f, 4f }; // Posições Y: baixo, meio, cima
    [SerializeField] private float velocidadeTroca = 15f; // Velocidade de transição
    
    private int plataformaAtual = 1; // Índice da plataforma atual (0=baixo, 1=meio, 2=cima)
    private float targetY; // Posição Y de destino
    
    void Start()
    {
        // Inicializa na plataforma do meio
        targetY = posicoesY[plataformaAtual];
    }
    
    void Update()
    {
        ProcessarInput();
        MoverParaPlataforma();
    }
    
    /// <summary>
    /// Processa input do teclado (W para subir, S para descer)
    /// </summary>
    private void ProcessarInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SubirPlataforma();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DescerPlataforma();
        }
    }
    
    /// <summary>
    /// Move suavemente o player para a plataforma alvo
    /// </summary>
    private void MoverParaPlataforma()
    {
        Vector3 posicao = transform.position;
        posicao.y = Mathf.Lerp(posicao.y, targetY, velocidadeTroca * Time.deltaTime);
        transform.position = posicao;
    }
    
    /// <summary>
    /// Sobe uma plataforma se possível
    /// </summary>
    private void SubirPlataforma()
    {
        if (plataformaAtual < posicoesY.Length - 1)
        {
            plataformaAtual++;
            targetY = posicoesY[plataformaAtual];
        }
    }
    
    /// <summary>
    /// Desce uma plataforma se possível
    /// </summary>
    private void DescerPlataforma()
    {
        if (plataformaAtual > 0)
        {
            plataformaAtual--;
            targetY = posicoesY[plataformaAtual];
        }
    }
}
