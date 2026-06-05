using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
==========================================================
EXPLICAÇÃO GERAL DO PROGRAMA
==========================================================

O programa possui um menu interativo que permite:

1) Digitar um novo texto
2) Buscar uma palavra específica
3) Comparar textos anteriores
4) Encerrar o programa

----------------------------------------------------------

2) List<Dictionary<string,int>>

Utilizei para guardar o histórico dos textos digitados.

Cada posição da lista representa um texto analisado.

Exemplo:

historico[0] -> primeiro texto
historico[1] -> segundo texto
historico[2] -> terceiro texto

Isso permite comparar textos digitados em momentos
diferentes da execução.

----------------------------------------------------------

3) HashSet<string>

Utilizei para guardar palavras sem repetição.

Exemplo:

{ "casa", "carro", "gato" }

Mesmo que a palavra apareça várias vezes,
ela será armazenada apenas uma vez.

A operação IntersectWith() calcula a interseção
entre dois conjuntos.

----------------------------------------------------------
FUNCIONAMENTO DO PROGRAMA
----------------------------------------------------------

PASSO 1 - MENU PRINCIPAL

O método Main() executa um laço infinito (while true)
que exibe o menu para o usuário.

Dependendo da opção escolhida, o programa executa
uma funcionalidade diferente.

----------------------------------------------------------

PASSO 2 - LEITURA DO TEXTO

Quando o usuário escolhe a opção "Novo Texto",
o método LerTextoEContar() é executado.

O usuário pode digitar várias linhas.

A leitura termina quando uma linha vazia é digitada.

----------------------------------------------------------

PASSO 3 - NORMALIZAÇÃO DO TEXTO

Antes da contagem, o texto é tratado para evitar
problemas com diferenças de escrita.

São feitas duas etapas:

Conversão para minúsculas e remoção de pontuação

Assim palavras iguais serão contadas corretamente.

----------------------------------------------------------

PASSO 4 - DIVISÃO EM PALAVRAS

Após limpar o texto, o programa utiliza Split()
para separar as palavras.

----------------------------------------------------------

PASSO 5 - CONTAGEM DE FREQUÊNCIA

Cada palavra encontrada é inserida no Dictionary.

Se a palavra já existir:
    frequência++

Se ainda não existir:
    frequência = 1

----------------------------------------------------------

PASSO 6 - EXIBIÇÃO DOS RESULTADOS

Após finalizar a leitura do texto, o programa exibe:

- Total de palavras
- Quantidade de palavras distintas
- As 10 palavras mais frequentes

Para isso utiliza LINQ:

OrderByDescending()

Ordena da maior frequência para a menor.

ThenBy()

Desempata em ordem alfabética.

Take(10)

Seleciona apenas as 10 primeiras posições.

----------------------------------------------------------

PASSO 7 - BUSCA DE PALAVRAS

A opção "Buscar Palavra" permite consultar
uma palavra específica.

O usuário digita uma palavra.

O programa utiliza:

ContainsKey()

para verificar se ela existe no Dictionary. Caso exista, exibe a quantidade de ocorrências.
Caso contrário, informa que a palavra não aparece.

----------------------------------------------------------

PASSO 8 - COMPARAÇÃO DE TEXTOS

A opção "Comparar Textos" utiliza os dois últimos
textos armazenados no histórico.

O programa:
Obtém as palavras do primeiro texto. Obtém as palavras do segundo texto.
Cria dois HashSets. Executa IntersectWith().

O resultado são todas as palavras presentes
nos dois textos simultaneamente.
*/

public class Program
{
    // Lista para guardar os textos anteriores
    static List<Dictionary<string, int>> historico = new List<Dictionary<string, int>>();

    public static void Main()
    {
        Dictionary<string, int> frequenciaAtual = null;

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1) Novo texto");
            Console.WriteLine("2) Buscar palavra");
            Console.WriteLine("3) Comparar textos");
            Console.WriteLine("4) Sair");
            Console.Write("Escolha: ");

            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    frequenciaAtual = LerTextoEContar();
                    historico.Add(frequenciaAtual);
                    MostrarResultado(frequenciaAtual);
                    break;

                case "2":
                    BuscarPalavra(frequenciaAtual);
                    break;

                case "3":
                    CompararTextos();
                    break;

                case "4":
                    Console.WriteLine("Tchau!");
                    return;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    // ===============================
    // LÊ TEXTO E CONTA PALAVRAS
    // ===============================
    static Dictionary<string, int> LerTextoEContar()
    {
        Dictionary<string, int> frequencia = new Dictionary<string, int>();

        Console.WriteLine("\nDigite o texto (linha vazia para encerrar):");

        while (true)
        {
            string linha = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(linha))
                break;

            // Deixa tudo minúsculo
            linha = linha.ToLower();

            // Remove pontuação
            StringBuilder limpa = new StringBuilder();

            foreach (char c in linha)
            {
                if (char.IsLetterOrDigit(c) || c == ' ')
                    limpa.Append(c);
            }

            // Divide em palavras
            string[] palavras = limpa.ToString().Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries
            );

            // Conta frequência
            foreach (string palavra in palavras)
            {
                if (frequencia.ContainsKey(palavra))
                    frequencia[palavra]++;
                else
                    frequencia[palavra] = 1;
            }
        }

        return frequencia;
    }

    // ===============================
    // MOSTRA RESULTADO
    // ===============================
    static void MostrarResultado(Dictionary<string, int> frequencia)
    {
        int totalPalavras = frequencia.Values.Sum();

        Console.WriteLine("\n=== Resultado ===");
        Console.WriteLine($"Total de palavras: {totalPalavras}");
        Console.WriteLine($"Palavras distintas: {frequencia.Count}");

        Console.WriteLine("\nTop 10 palavras mais frequentes:");

        var top10 = frequencia
            .OrderByDescending(p => p.Value)
            .ThenBy(p => p.Key)
            .Take(10)
            .ToList();

        int posicao = 1;

        foreach (var item in top10)
        {
            Console.WriteLine(
                $"{posicao}. \"{item.Key}\" - {item.Value} ocorrencia(s)"
            );

            posicao++;
        }
    }

    // ===============================
    // BUSCAR PALAVRA
    // ===============================
    static void BuscarPalavra(Dictionary<string, int> frequencia)
    {
        if (frequencia == null)
        {
            Console.WriteLine("Nenhum texto foi digitado ainda.");
            return;
        }

        Console.Write("Qual palavra? ");
        string palavra = Console.ReadLine().ToLower();

        if (frequencia.ContainsKey(palavra))
        {
            Console.WriteLine(
                $"\"{palavra}\" aparece {frequencia[palavra]} vez(es)"
            );
        }
        else
        {
            Console.WriteLine($"A palavra \"{palavra}\" não aparece.");
        }
    }

    // ===============================
    // COMPARAR TEXTOS
    // ===============================
    static void CompararTextos()
    {
        if (historico.Count < 2)
        {
            Console.WriteLine("É necessário pelo menos 2 textos.");
            return;
        }

        Dictionary<string, int> texto1 = historico[historico.Count - 2];
        Dictionary<string, int> texto2 = historico[historico.Count - 1];

        HashSet<string> palavras1 = new HashSet<string>(texto1.Keys);
        HashSet<string> palavras2 = new HashSet<string>(texto2.Keys);

        palavras1.IntersectWith(palavras2);

        Console.WriteLine("\nPalavras em comum:");

        foreach (string palavra in palavras1)
        {
            Console.WriteLine(palavra);
        }
    }
}