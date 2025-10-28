using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia pontuação, game over e reinício do jogo
/// Implementa padrão Singleton para acesso global
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Configurações")]
    [SerializeField] private float pontosPorSegundo = 10f; // Velocidade de pontuação
    
    [Header("Configurações de Dificuldade")]
    [SerializeField] private float velocidadeInicial = 12f; // Velocidade inicial dos obstáculos
    [SerializeField] private float velocidadeMaxima = 25f; // Velocidade máxima dos obstáculos
    [SerializeField] private float incrementoVelocidade = 2f; // Aumento de velocidade a cada intervalo
    [SerializeField] private float intervaloAumento = 3f; // Tempo entre aumentos de velocidade
    
    private float pontuacao = 0f; // Pontuação atual
    private bool jogoAtivo = true; // Estado do jogo
    private float velocidadeAtual; // Velocidade atual dos obstáculos
    private float tempoProximoAumento; // Tempo do próximo aumento de velocidade
    
    void Awake()
    {
        ConfigurarSingleton();
        InicializarDificuldade();
    }
    
    /// <summary>
    /// Inicializa o sistema de dificuldade progressiva
    /// </summary>
    private void InicializarDificuldade()
    {
        velocidadeAtual = velocidadeInicial;
        tempoProximoAumento = Time.time + intervaloAumento;
    }
    
    void Update()
    {
        if (jogoAtivo)
        {
            AtualizarPontuacao();
            AtualizarDificuldade();
        }
        else
        {
            VerificarReinicio();
        }
    }
    
    /// <summary>
    /// Configura o padrão Singleton (apenas uma instância)
    /// </summary>
    private void ConfigurarSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Aumenta a pontuação ao longo do tempo
    /// </summary>
    private void AtualizarPontuacao()
    {
        pontuacao += pontosPorSegundo * Time.deltaTime;
    }
    
    /// <summary>
    /// Aumenta a velocidade dos obstáculos progressivamente
    /// </summary>
    private void AtualizarDificuldade()
    {
        if (Time.time >= tempoProximoAumento && velocidadeAtual < velocidadeMaxima)
        {
            velocidadeAtual += incrementoVelocidade;
            velocidadeAtual = Mathf.Min(velocidadeAtual, velocidadeMaxima);
            tempoProximoAumento = Time.time + intervaloAumento;
        }
    }
    
    /// <summary>
    /// Retorna a velocidade atual dos obstáculos
    /// </summary>
    public float GetVelocidadeAtual()
    {
        return velocidadeAtual;
    }
    
    /// <summary>
    /// Verifica se o jogador pressionou R para reiniciar
    /// </summary>
    private void VerificarReinicio()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarJogo();
        }
    }
    
    /// <summary>
    /// Chamado quando o player colide com obstáculo
    /// </summary>
    public void GameOver()
    {
        if (!jogoAtivo) return;
        
        jogoAtivo = false;
        
        // Para o spawner de obstáculos
        ObstaculoSpawner spawner = FindObjectOfType<ObstaculoSpawner>();
        if (spawner != null)
        {
            spawner.PararSpawn();
        }
    }
    
    /// <summary>
    /// Reinicia o jogo recarregando a cena
    /// </summary>
    private void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    /// <summary>
    /// Desenha a UI simples na tela
    /// </summary>
    void OnGUI()
    {
        DesenharPontuacao();
        
        if (!jogoAtivo)
        {
            DesenharGameOver();
        }
    }
    
    /// <summary>
    /// Desenha a pontuação no canto superior esquerdo
    /// </summary>
    private void DesenharPontuacao()
    {
        GUI.skin.label.fontSize = 24;
        GUI.Label(new Rect(10, 10, 300, 30), $"Pontuação: {Mathf.FloorToInt(pontuacao)}");
    }
    
    /// <summary>
    /// Desenha a mensagem de Game Over
    /// </summary>
    private void DesenharGameOver()
    {
        // Título Game Over
        GUI.skin.label.fontSize = 36;
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 50), "GAME OVER!");
        
        // Instrução de reinício
        GUI.skin.label.fontSize = 20;
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 30), "Pressione R para Reiniciar");
    }
}
