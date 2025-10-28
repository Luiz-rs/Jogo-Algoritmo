# 📚 Documentação Completa - Endless Runner 2D

## 🎮 Visão Geral do Projeto

Endless runner 2D desenvolvido em Unity com sistema de dificuldade progressiva. O jogador controla um personagem que troca entre 3 plataformas para desviar de obstáculos que se movem cada vez mais rápido.

### Características Principais:
- ✅ 3 plataformas verticais
- ✅ Controle simples (W/S)
- ✅ Dificuldade progressiva
- ✅ Sistema de pontuação
- ✅ Spawns múltiplos em sequência
- ✅ Velocidade crescente

---

## 📁 Estrutura do Projeto

```
Assets/
├── Scripts/
│   ├── PlayerController.cs      # Controle do personagem
│   ├── Obstaculos.cs            # Comportamento dos obstáculos
│   ├── ObstaculoSpawner.cs      # Geração de obstáculos
│   └── GameManager.cs           # Gerenciamento do jogo
├── Prefabs/
│   └── Obstaculo.prefab         # Prefab do obstáculo
└── Scenes/
    └── SampleScene.unity        # Cena principal
```

---

# 🎯 SCRIPT 1: PlayerController.cs

## Descrição
Controla o movimento do personagem entre as 3 plataformas usando as teclas W (subir) e S (descer).

## Variáveis Principais

### Configuráveis no Inspector:
```csharp
[SerializeField] private float[] posicoesY = { -2f, 1f, 4f };
```
- **Tipo:** Array de float
- **Função:** Define as posições Y das 3 plataformas (baixo, meio, cima)
- **Valores:** -2 (baixo), 1 (meio), 4 (cima)

```csharp
[SerializeField] private float velocidadeTroca = 15f;
```
- **Tipo:** float
- **Função:** Velocidade de transição entre plataformas
- **Valor:** 15 (movimento rápido para compensar dificuldade)

### Variáveis Privadas:
```csharp
private int plataformaAtual = 1;
```
- **Tipo:** int
- **Função:** Índice da plataforma atual (0=baixo, 1=meio, 2=cima)
- **Inicial:** 1 (começa no meio)

```csharp
private float targetY;
```
- **Tipo:** float
- **Função:** Posição Y de destino para movimento suave

## Métodos Principais

### `void Start()`
```csharp
void Start()
{
    targetY = posicoesY[plataformaAtual];
}
```
- **Quando:** Executado uma vez ao iniciar
- **Função:** Inicializa a posição alvo na plataforma do meio

### `void Update()`
```csharp
void Update()
{
    ProcessarInput();
    MoverParaPlataforma();
}
```
- **Quando:** Executado a cada frame
- **Função:** Processa input do jogador e move o personagem

### `ProcessarInput()`
```csharp
private void ProcessarInput()
{
    if (Input.GetKeyDown(KeyCode.W))
        SubirPlataforma();
    else if (Input.GetKeyDown(KeyCode.S))
        DescerPlataforma();
}
```
- **Função:** Detecta teclas W e S pressionadas
- **W:** Chama SubirPlataforma()
- **S:** Chama DescerPlataforma()

### `MoverParaPlataforma()`
```csharp
private void MoverParaPlataforma()
{
    Vector3 posicao = transform.position;
    posicao.y = Mathf.Lerp(posicao.y, targetY, velocidadeTroca * Time.deltaTime);
    transform.position = posicao;
}
```
- **Função:** Move suavemente o player para a plataforma alvo
- **Técnica:** Usa Lerp para interpolação suave
- **Velocidade:** Controlada por `velocidadeTroca`

### `SubirPlataforma()`
```csharp
private void SubirPlataforma()
{
    if (plataformaAtual < posicoesY.Length - 1)
    {
        plataformaAtual++;
        targetY = posicoesY[plataformaAtual];
    }
}
```
- **Função:** Sobe uma plataforma se não estiver no topo
- **Validação:** Verifica se não está na última plataforma
- **Ação:** Incrementa índice e atualiza targetY

### `DescerPlataforma()`
```csharp
private void DescerPlataforma()
{
    if (plataformaAtual > 0)
    {
        plataformaAtual--;
        targetY = posicoesY[plataformaAtual];
    }
}
```
- **Função:** Desce uma plataforma se não estiver no fundo
- **Validação:** Verifica se não está na primeira plataforma
- **Ação:** Decrementa índice e atualiza targetY

## Fluxo de Execução
1. **Start:** Inicializa posição na plataforma do meio
2. **Update (cada frame):**
   - Verifica input (W/S)
   - Move suavemente para plataforma alvo
3. **Input detectado:**
   - Atualiza índice da plataforma
   - Define nova posição alvo
   - Movimento suave acontece no próximo Update

---

# 🚧 SCRIPT 2: Obstaculos.cs

## Descrição
Controla o movimento dos obstáculos da direita para esquerda e detecta colisão com o player.

## Variáveis Principais

### Configuráveis no Inspector:
```csharp
[SerializeField] private float limiteEsquerda = -10f;
```
- **Tipo:** float
- **Função:** Posição X onde o obstáculo é destruído
- **Valor:** -10 (fora da tela à esquerda)

### Variáveis Privadas:
```csharp
private float velocidade;
```
- **Tipo:** float
- **Função:** Velocidade de movimento do obstáculo
- **Origem:** Obtida do GameManager ao spawnar

## Métodos Principais

### `void Start()`
```csharp
void Start()
{
    if (GameManager.Instance != null)
        velocidade = GameManager.Instance.GetVelocidadeAtual();
    else
        velocidade = 5f;
}
```
- **Quando:** Executado ao spawnar o obstáculo
- **Função:** Obtém velocidade atual do GameManager
- **Fallback:** Usa 5 se GameManager não existir

### `void Update()`
```csharp
void Update()
{
    MoverObstaculo();
    VerificarLimite();
}
```
- **Quando:** Executado a cada frame
- **Função:** Move obstáculo e verifica se saiu da tela

### `MoverObstaculo()`
```csharp
private void MoverObstaculo()
{
    transform.Translate(Vector3.left * velocidade * Time.deltaTime);
}
```
- **Função:** Move obstáculo para a esquerda
- **Direção:** Vector3.left (eixo X negativo)
- **Velocidade:** Multiplicada por Time.deltaTime para frame-independence

### `VerificarLimite()`
```csharp
private void VerificarLimite()
{
    if (transform.position.x < limiteEsquerda)
        Destroy(gameObject);
}
```
- **Função:** Destroi obstáculo quando sai da tela
- **Condição:** Posição X menor que limiteEsquerda
- **Ação:** Remove objeto da memória

### `OnTriggerEnter2D()`
```csharp
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
        Destroy(collision.gameObject);
    }
}
```
- **Quando:** Detecta colisão com trigger
- **Validação:** Verifica se colidiu com objeto tag "Player"
- **Ações:**
  1. Notifica GameManager (Game Over)
  2. Destroi o player

## Fluxo de Execução
1. **Start:** Obtém velocidade do GameManager
2. **Update (cada frame):**
   - Move para esquerda
   - Verifica se saiu da tela
3. **Colisão detectada:**
   - Chama Game Over
   - Destroi player

---

# 🎲 SCRIPT 3: ObstaculoSpawner.cs

## Descrição
Gera obstáculos automaticamente em intervalos aleatórios com dificuldade progressiva.

## Variáveis de Configuração

### Spawn:
```csharp
[SerializeField] private GameObject obstaculoPrefab;
```
- **Tipo:** GameObject
- **Função:** Prefab do obstáculo a ser instanciado

```csharp
[SerializeField] private float[] posicoesY = { -2f, 1f, 4f };
```
- **Tipo:** Array de float
- **Função:** Posições Y das 3 plataformas

```csharp
[SerializeField] private float posicaoXSpawn = 6f;
```
- **Tipo:** float
- **Função:** Posição X onde obstáculos aparecem
- **Valor:** 6 (perto do player para pouco tempo de reação)

```csharp
[SerializeField] private int obstaculosPorSpawn = 2;
```
- **Tipo:** int
- **Função:** Quantidade de obstáculos em sequência
- **Valor:** 2 (spawna 2 obstáculos em fila)

```csharp
[SerializeField] private float espacamentoEntreObstaculos = 2f;
```
- **Tipo:** float
- **Função:** Distância entre obstáculos em sequência
- **Valor:** 2 unidades

### Intervalo:
```csharp
[SerializeField] private float intervaloMinimo = 0.3f;
[SerializeField] private float intervaloMaximo = 0.7f;
```
- **Tipo:** float
- **Função:** Tempo mínimo e máximo entre spawns
- **Valores:** 0.3-0.7 segundos (muito rápido!)

### Dificuldade:
```csharp
[SerializeField] private float tempoParaDiminuir = 5f;
```
- **Tipo:** float
- **Função:** Tempo para aumentar dificuldade
- **Valor:** 5 segundos

```csharp
[SerializeField] private float reducaoIntervalo = 0.15f;
```
- **Tipo:** float
- **Função:** Quanto reduz do intervalo
- **Valor:** 0.15 segundos

### Spawns Múltiplos:
```csharp
[SerializeField] private float chanceSpawnDuplo = 30f;
[SerializeField] private float incrementoChance = 15f;
[SerializeField] private float chanceMaxima = 80f;
```
- **Tipo:** float
- **Função:** Sistema de chance de spawns extras
- **Valores:** Começa em 30%, aumenta 15% até 80%

## Variáveis Privadas
```csharp
private float proximoSpawn;
private float tempoDecorrido;
private bool jogoAtivo = true;
```

## Métodos Principais

### `void Start()`
```csharp
void Start()
{
    AgendarProximoSpawn();
}
```
- **Função:** Agenda o primeiro spawn

### `void Update()`
```csharp
void Update()
{
    if (!jogoAtivo) return;
    AumentarDificuldade();
    VerificarSpawn();
}
```
- **Função:** Atualiza dificuldade e verifica spawns

### `AumentarDificuldade()`
```csharp
private void AumentarDificuldade()
{
    tempoDecorrido += Time.deltaTime;
    
    if (tempoDecorrido >= tempoParaDiminuir)
    {
        tempoDecorrido = 0f;
        intervaloMinimo = Mathf.Max(0.15f, intervaloMinimo - reducaoIntervalo);
        intervaloMaximo = Mathf.Max(0.3f, intervaloMaximo - reducaoIntervalo);
        chanceSpawnDuplo = Mathf.Min(chanceSpawnDuplo + incrementoChance, chanceMaxima);
    }
}
```
- **Função:** Aumenta dificuldade a cada 5 segundos
- **Ações:**
  1. Reduz intervalo entre spawns
  2. Aumenta chance de spawns duplos
  3. Limita valores mínimos/máximos

### `VerificarSpawn()`
```csharp
private void VerificarSpawn()
{
    if (Time.time >= proximoSpawn)
    {
        SpawnarObstaculo();
        AgendarProximoSpawn();
    }
}
```
- **Função:** Verifica se é hora de spawnar
- **Condição:** Tempo atual >= tempo agendado

### `SpawnarObstaculo()`
```csharp
private void SpawnarObstaculo()
{
    // Cria obstáculos em sequência
    for (int i = 0; i < obstaculosPorSpawn; i++)
    {
        int indiceAleatorio = Random.Range(0, posicoesY.Length);
        float posX = posicaoXSpawn + (i * espacamentoEntreObstaculos);
        CriarObstaculo(indiceAleatorio, posX);
    }
    
    // Chance de spawn duplo adicional
    if (Random.Range(0f, 100f) < chanceSpawnDuplo)
    {
        int indiceExtra = Random.Range(0, posicoesY.Length);
        float posXExtra = posicaoXSpawn + (Random.Range(0, obstaculosPorSpawn) * espacamentoEntreObstaculos);
        CriarObstaculo(indiceExtra, posXExtra);
    }
}
```
- **Função:** Cria obstáculos em sequência
- **Loop:** Cria 2 obstáculos em fila
- **Posicionamento:** X = 6, 8 (espaçamento de 2)
- **Chance Extra:** 30-80% de spawnar obstáculo adicional

### `CriarObstaculo()`
```csharp
private void CriarObstaculo(int indicePlataforma, float posX)
{
    float posY = posicoesY[indicePlataforma];
    Vector3 posicao = new Vector3(posX, posY, 0f);
    Instantiate(obstaculoPrefab, posicao, Quaternion.identity);
}
```
- **Função:** Instancia obstáculo na posição especificada
- **Parâmetros:** Índice da plataforma e posição X

### `AgendarProximoSpawn()`
```csharp
private void AgendarProximoSpawn()
{
    float intervalo = Random.Range(intervaloMinimo, intervaloMaximo);
    proximoSpawn = Time.time + intervalo;
}
```
- **Função:** Define tempo do próximo spawn
- **Intervalo:** Aleatório entre min e max

### `PararSpawn()`
```csharp
public void PararSpawn()
{
    jogoAtivo = false;
}
```
- **Função:** Para spawns (chamado no Game Over)

## Fluxo de Execução
1. **Start:** Agenda primeiro spawn
2. **Update (cada frame):**
   - Aumenta dificuldade a cada 5s
   - Verifica se é hora de spawnar
3. **Spawn:**
   - Cria 2 obstáculos em sequência
   - 30-80% de chance de criar mais um
   - Agenda próximo spawn

---

# 🎮 SCRIPT 4: GameManager.cs

## Descrição
Gerencia pontuação, dificuldade progressiva, game over e reinício do jogo. Implementa padrão Singleton.

## Padrão Singleton
```csharp
public static GameManager Instance { get; private set; }
```
- **Tipo:** GameManager estático
- **Função:** Permite acesso global ao GameManager
- **Uso:** `GameManager.Instance.GameOver()`

## Variáveis de Configuração

### Pontuação:
```csharp
[SerializeField] private float pontosPorSegundo = 10f;
```
- **Tipo:** float
- **Função:** Velocidade de aumento da pontuação
- **Valor:** 10 pontos por segundo

### Dificuldade:
```csharp
[SerializeField] private float velocidadeInicial = 12f;
[SerializeField] private float velocidadeMaxima = 25f;
[SerializeField] private float incrementoVelocidade = 2f;
[SerializeField] private float intervaloAumento = 3f;
```
- **velocidadeInicial:** Velocidade inicial dos obstáculos (12)
- **velocidadeMaxima:** Velocidade máxima (25)
- **incrementoVelocidade:** Aumento a cada intervalo (2)
- **intervaloAumento:** Tempo entre aumentos (3 segundos)

## Variáveis Privadas
```csharp
private float pontuacao = 0f;
private bool jogoAtivo = true;
private float velocidadeAtual;
private float tempoProximoAumento;
```

## Métodos Principais

### `void Awake()`
```csharp
void Awake()
{
    ConfigurarSingleton();
    InicializarDificuldade();
}
```
- **Quando:** Antes do Start
- **Função:** Configura Singleton e inicializa dificuldade

### `ConfigurarSingleton()`
```csharp
private void ConfigurarSingleton()
{
    if (Instance == null)
        Instance = this;
    else
        Destroy(gameObject);
}
```
- **Função:** Garante apenas uma instância do GameManager
- **Lógica:** Se já existe, destroi o novo

### `InicializarDificuldade()`
```csharp
private void InicializarDificuldade()
{
    velocidadeAtual = velocidadeInicial;
    tempoProximoAumento = Time.time + intervaloAumento;
}
```
- **Função:** Define velocidade inicial e agenda primeiro aumento

### `void Update()`
```csharp
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
```
- **Jogo Ativo:** Atualiza pontuação e dificuldade
- **Jogo Inativo:** Verifica se jogador quer reiniciar

### `AtualizarPontuacao()`
```csharp
private void AtualizarPontuacao()
{
    pontuacao += pontosPorSegundo * Time.deltaTime;
}
```
- **Função:** Aumenta pontuação continuamente
- **Cálculo:** 10 pontos/segundo * deltaTime

### `AtualizarDificuldade()`
```csharp
private void AtualizarDificuldade()
{
    if (Time.time >= tempoProximoAumento && velocidadeAtual < velocidadeMaxima)
    {
        velocidadeAtual += incrementoVelocidade;
        velocidadeAtual = Mathf.Min(velocidadeAtual, velocidadeMaxima);
        tempoProximoAumento = Time.time + intervaloAumento;
    }
}
```
- **Função:** Aumenta velocidade a cada 3 segundos
- **Incremento:** +2 por vez
- **Limite:** Máximo 25

### `GetVelocidadeAtual()`
```csharp
public float GetVelocidadeAtual()
{
    return velocidadeAtual;
}
```
- **Função:** Retorna velocidade atual para obstáculos
- **Uso:** Chamado por Obstaculos.cs ao spawnar

### `GameOver()`
```csharp
public void GameOver()
{
    if (!jogoAtivo) return;
    jogoAtivo = false;
    
    ObstaculoSpawner spawner = FindObjectOfType<ObstaculoSpawner>();
    if (spawner != null)
        spawner.PararSpawn();
}
```
- **Função:** Finaliza o jogo
- **Ações:**
  1. Marca jogo como inativo
  2. Para spawner de obstáculos

### `VerificarReinicio()`
```csharp
private void VerificarReinicio()
{
    if (Input.GetKeyDown(KeyCode.R))
        ReiniciarJogo();
}
```
- **Função:** Verifica tecla R para reiniciar

### `ReiniciarJogo()`
```csharp
private void ReiniciarJogo()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
```
- **Função:** Recarrega a cena atual
- **Efeito:** Reseta tudo ao estado inicial

### `OnGUI()`
```csharp
void OnGUI()
{
    DesenharPontuacao();
    if (!jogoAtivo)
        DesenharGameOver();
}
```
- **Função:** Desenha UI na tela
- **Sempre:** Mostra pontuação
- **Game Over:** Mostra mensagem

### `DesenharPontuacao()`
```csharp
private void DesenharPontuacao()
{
    GUI.skin.label.fontSize = 24;
    GUI.Label(new Rect(10, 10, 300, 30), $"Pontuação: {Mathf.FloorToInt(pontuacao)}");
}
```
- **Função:** Desenha pontuação no canto superior esquerdo
- **Tamanho:** Fonte 24
- **Posição:** (10, 10)

### `DesenharGameOver()`
```csharp
private void DesenharGameOver()
{
    GUI.skin.label.fontSize = 36;
    GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 50), "GAME OVER!");
    
    GUI.skin.label.fontSize = 20;
    GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 30), "Pressione R para Reiniciar");
}
```
- **Função:** Desenha mensagem de Game Over centralizada
- **Título:** Fonte 36
- **Instrução:** Fonte 20

## Fluxo de Execução
1. **Awake:** Configura Singleton e inicializa velocidade
2. **Update (jogo ativo):**
   - Aumenta pontuação continuamente
   - Aumenta velocidade a cada 3s
3. **Update (game over):**
   - Verifica tecla R
4. **OnGUI:** Desenha UI

---

# 📊 Sistema de Dificuldade Progressiva

## Progressão de Velocidade
| Tempo | Velocidade | Incremento |
|-------|-----------|------------|
| 0s | 12 | - |
| 3s | 14 | +2 |
| 6s | 16 | +2 |
| 9s | 18 | +2 |
| 12s | 20 | +2 |
| 15s | 22 | +2 |
| 18s | 24 | +2 |
| 21s | 25 (MAX) | +1 |

## Progressão de Spawns
| Tempo | Intervalo | Chance Duplo |
|-------|-----------|--------------|
| 0s | 0.3-0.7s | 30% |
| 5s | 0.15-0.55s | 45% |
| 10s | 0.15-0.4s | 60% |
| 15s | 0.15-0.3s (MAX) | 75% |
| 20s+ | 0.15-0.3s | 80% (MAX) |

## Dificuldade Máxima
Atingida em aproximadamente **21 segundos**:
- Velocidade: 25 unidades/segundo
- Intervalo: 0.15-0.3 segundos
- Chance duplo: 80%
- Obstáculos por spawn: 2 em sequência

---

# 🎯 Configuração na Unity

## Player
1. **Rigidbody2D:**
   - Gravity Scale: 0
   - Freeze Rotation Z: ✓
   - Freeze Position X: ✓

2. **Collider2D:**
   - Is Trigger: ✓
   - Ajustar tamanho ao sprite

3. **Tag:** "Player"

4. **Script:** PlayerController.cs

## Obstáculo (Prefab)
1. **Sprite Renderer:** Sprite do obstáculo

2. **Collider2D:**
   - Is Trigger: ✓
   - Ajustar tamanho

3. **Script:** Obstaculos.cs

## ObstaculoSpawner (GameObject vazio)
1. **Script:** ObstaculoSpawner.cs

2. **Configurações:**
   - Obstaculo Prefab: Arrastar prefab
   - Posicoes Y: -2, 1, 4
   - Posicao X Spawn: 6
   - Obstaculos Por Spawn: 2
   - Espacamento: 2

## GameManager (GameObject vazio)
1. **Script:** GameManager.cs

2. **Configurações:**
   - Pontos Por Segundo: 10
   - Velocidade Inicial: 12
   - Velocidade Maxima: 25

---

# 🎮 Controles

| Tecla | Ação |
|-------|------|
| **W** | Subir plataforma |
| **S** | Descer plataforma |
| **R** | Reiniciar (após Game Over) |

---

# 🔧 Ajustes de Dificuldade

## Para Tornar Mais Fácil:
- ↓ Velocidade Inicial (GameManager)
- ↑ Intervalo entre spawns (ObstaculoSpawner)
- ↓ Chance Spawn Duplo
- ↓ Obstaculos Por Spawn

## Para Tornar Mais Difícil:
- ↑ Velocidade Inicial
- ↓ Intervalo entre spawns
- ↑ Chance Spawn Duplo
- ↑ Obstaculos Por Spawn
- ↓ Posicao X Spawn (mais perto)

---

# 📈 Boas Práticas Implementadas

## Código Limpo
- ✅ Nomes descritivos em português
- ✅ Comentários XML em todos os métodos
- ✅ Separação de responsabilidades
- ✅ Métodos pequenos e focados

## Padrões de Design
- ✅ Singleton (GameManager)
- ✅ Separação de concerns
- ✅ Configuração via Inspector

## Performance
- ✅ Destruição de objetos fora da tela
- ✅ Uso de Time.deltaTime
- ✅ Verificações otimizadas

## Manutenibilidade
- ✅ Código modular
- ✅ Fácil ajuste de parâmetros
- ✅ Documentação completa

---

**Desenvolvido para Unity 2D - Endless Runner**
**Documentação criada em: 28/10/2025**
