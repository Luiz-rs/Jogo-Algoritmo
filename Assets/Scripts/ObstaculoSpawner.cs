using UnityEngine;

/// <summary>
/// Gera obstáculos automaticamente em intervalos aleatórios
/// </summary>
public class ObstaculoSpawner : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    [SerializeField] private GameObject obstaculoPrefab; // Prefab do obstáculo
    [SerializeField] private float[] posicoesY = { -2f, 1f, 4f }; // Posições das plataformas
    [SerializeField] private float posicaoXSpawn = 6f; // Posição X de spawn (direita) - MAIS PERTO!
    [SerializeField] private int obstaculosPorSpawn = 2; // Quantidade de obstáculos em sequência
    [SerializeField] private float espacamentoEntreObstaculos = 2f; // Distância entre obstáculos em sequência
    
    [Header("Configurações de Intervalo")]
    [SerializeField] private float intervaloMinimo = 0.3f; // Tempo mínimo entre spawns
    [SerializeField] private float intervaloMaximo = 0.7f; // Tempo máximo entre spawns
    
    [Header("Configurações de Dificuldade")]
    [SerializeField] private float tempoParaDiminuir = 5f; // Tempo para aumentar dificuldade
    [SerializeField] private float reducaoIntervalo = 0.15f; // Redução do intervalo
    
    [Header("Configurações de Spawns Múltiplos")]
    [SerializeField] private float chanceSpawnDuplo = 30f; // Chance inicial de spawn duplo (0-100)
    [SerializeField] private float incrementoChance = 15f; // Aumento de chance a cada intervalo
    [SerializeField] private float chanceMaxima = 80f; // Chance máxima de spawn duplo
    
    private float proximoSpawn; // Tempo do próximo spawn
    private float tempoDecorrido; // Tempo desde último aumento de dificuldade
    private bool jogoAtivo = true; // Estado do jogo
    
    void Start()
    {
        AgendarProximoSpawn();
    }
    
    void Update()
    {
        if (!jogoAtivo) return;
        
        AumentarDificuldade();
        VerificarSpawn();
    }
    
    /// <summary>
    /// Aumenta a dificuldade reduzindo o intervalo entre spawns e aumentando chance de spawns múltiplos
    /// </summary>
    private void AumentarDificuldade()
    {
        tempoDecorrido += Time.deltaTime;
        
        if (tempoDecorrido >= tempoParaDiminuir)
        {
            tempoDecorrido = 0f;
            
            // Reduz intervalo entre spawns
            intervaloMinimo = Mathf.Max(0.15f, intervaloMinimo - reducaoIntervalo);
            intervaloMaximo = Mathf.Max(0.3f, intervaloMaximo - reducaoIntervalo);
            
            // Aumenta chance de spawns múltiplos
            chanceSpawnDuplo = Mathf.Min(chanceSpawnDuplo + incrementoChance, chanceMaxima);
        }
    }
    
    /// <summary>
    /// Verifica se é hora de spawnar um novo obstáculo
    /// </summary>
    private void VerificarSpawn()
    {
        if (Time.time >= proximoSpawn)
        {
            SpawnarObstaculo();
            AgendarProximoSpawn();
        }
    }
    
    /// <summary>
    /// Cria um ou mais obstáculos em plataformas aleatórias
    /// </summary>
    private void SpawnarObstaculo()
    {
        // Cria obstáculos em sequência (um atrás do outro)
        for (int i = 0; i < obstaculosPorSpawn; i++)
        {
            int indiceAleatorio = Random.Range(0, posicoesY.Length);
            float posX = posicaoXSpawn + (i * espacamentoEntreObstaculos);
            CriarObstaculo(indiceAleatorio, posX);
        }
        
        // Chance de spawn duplo adicional (em plataforma diferente no mesmo X)
        if (Random.Range(0f, 100f) < chanceSpawnDuplo)
        {
            int indiceExtra = Random.Range(0, posicoesY.Length);
            float posXExtra = posicaoXSpawn + (Random.Range(0, obstaculosPorSpawn) * espacamentoEntreObstaculos);
            CriarObstaculo(indiceExtra, posXExtra);
        }
    }
    
    /// <summary>
    /// Cria um obstáculo na plataforma e posição X especificadas
    /// </summary>
    private void CriarObstaculo(int indicePlataforma, float posX)
    {
        float posY = posicoesY[indicePlataforma];
        Vector3 posicao = new Vector3(posX, posY, 0f);
        Instantiate(obstaculoPrefab, posicao, Quaternion.identity);
    }
    
    /// <summary>
    /// Retorna um índice de plataforma diferente do fornecido
    /// </summary>
    private int ObterPlataformaDiferente(int indiceAtual)
    {
        int novoIndice;
        do
        {
            novoIndice = Random.Range(0, posicoesY.Length);
        }
        while (novoIndice == indiceAtual && posicoesY.Length > 1);
        
        return novoIndice;
    }
    
    /// <summary>
    /// Agenda o próximo spawn com intervalo aleatório
    /// </summary>
    private void AgendarProximoSpawn()
    {
        float intervalo = Random.Range(intervaloMinimo, intervaloMaximo);
        proximoSpawn = Time.time + intervalo;
    }
    
    /// <summary>
    /// Para o spawn de obstáculos (chamado no Game Over)
    /// </summary>
    public void PararSpawn()
    {
        jogoAtivo = false;
    }
}
