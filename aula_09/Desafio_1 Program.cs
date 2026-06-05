using System;
using System.Collections.Generic;
using System.Linq;

/*
==========================================================
EXPLICAÇÃO GERAL DO PROGRAMA
==========================================================

O programa permite:

1) Listar todas as cidades e suas conexões
2) Verificar se existe conexão direta entre duas cidades
3) Verificar se existe alguma rota utilizando DFS
4) Encontrar a menor rota utilizando BFS
5) Encerrar o programa

----------------------------------------------------------
FUNCIONAMENTO DO PROGRAMA
----------------------------------------------------------

O método CriarMapa() é executado no início.

Ele preenche o Dictionary com todas as cidades
e suas conexões.

Após essa etapa o grafo está pronto para consultas.

O método Main() exibe continuamente um menu.

O usuário escolhe uma opção. Dependendo da escolha, uma funcionalidade diferente
é executada.

O menu permanece ativo até a opção sair.

A opção "Listar cidades" percorre o Dictionary.

Para cada cidade:

- Exibe o nome da cidade
- Exibe todas as cidades conectadas

Conexão Direta:

O usuário informa duas cidades.

O programa verifica:

grafo[cidade1].Contains(cidade2)

Se verdadeiro:

Existe estrada direta.

Se falso:

Não existe estrada direta.

Exemplo:

São Paulo -> Curitiba

Resultado:

Possuem conexão direta.

Já:

São Paulo -> Porto Alegre

Resultado:

Não possuem conexão direta.

Pois é necessário passar por outras cidades.


DFS:

A função DFS recebe:

- cidade atual
- cidade destino
- conjunto de visitados

Primeiro:

Marca a cidade atual como visitada.

Depois:

Verifica se chegou ao destino.

Se chegou:

Retorna true.

Caso contrário:

Percorre todos os vizinhos.

Para cada vizinho ainda não visitado:

Executa DFS recursivamente.

Se algum caminho encontrar o destino:

Retorna true.

Caso todos os caminhos falhem:

Retorna false.

----------------------------------------------------------
RECONSTRUÇÃO DO CAMINHO
----------------------------------------------------------

Após encontrar o destino:

O programa utiliza o Dictionary pai
para reconstruir a rota.

*/

class Program
{
    static Dictionary<string, List<string>> grafo =
        new Dictionary<string, List<string>>();

    static void Main(string[] args)
    {
        CriarMapa();

        int opcao;

        do
        {
            Console.WriteLine("\n=== Bora Viajar! ===");
            Console.WriteLine("1) Listar cidades");
            Console.WriteLine("2) Conexão direta");
            Console.WriteLine("3) Existe rota? (DFS)");
            Console.WriteLine("4) Menor rota (BFS)");
            Console.WriteLine("5) Sair");
            Console.Write("Escolha: ");

            opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    ListarCidades();
                    break;

                case 2:
                    VerificarConexaoDireta();
                    break;

                case 3:
                    ExisteRotaDFS();
                    break;

                case 4:
                    MenorRotaBFS();
                    break;

                case 5:
                    Console.WriteLine("Boa viagem! 🧳");
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

        } while (opcao != 5);
    }

    static void CriarMapa()
    {
        grafo["São Paulo"] =
            new List<string> { "Rio de Janeiro", "Curitiba", "Belo Horizonte" };

        grafo["Rio de Janeiro"] =
            new List<string> { "São Paulo", "Belo Horizonte", "Vitória" };

        grafo["Belo Horizonte"] =
            new List<string> { "São Paulo", "Rio de Janeiro", "Brasília" };

        grafo["Curitiba"] =
            new List<string> { "São Paulo", "Florianópolis" };

        grafo["Florianópolis"] =
            new List<string> { "Curitiba", "Porto Alegre" };

        grafo["Porto Alegre"] =
            new List<string> { "Florianópolis" };

        grafo["Brasília"] =
            new List<string> { "Belo Horizonte", "Goiânia" };

        grafo["Goiânia"] =
            new List<string> { "Brasília" };

        grafo["Vitória"] =
            new List<string> { "Rio de Janeiro" };

        grafo["Salvador"] =
            new List<string> { "Recife" };

        grafo["Recife"] =
            new List<string> { "Salvador", "Fortaleza" };

        grafo["Fortaleza"] =
            new List<string> { "Recife" };
    }

    static void ListarCidades()
    {
        Console.WriteLine("\nCidades e conexões:");

        foreach (var cidade in grafo)
        {
            Console.Write(cidade.Key + ": ");

            Console.WriteLine("[" +
                string.Join(", ", cidade.Value) + "]");
        }
    }

    static void VerificarConexaoDireta()
    {
        Console.Write("Cidade 1: ");
        string cidade1 = Console.ReadLine();

        Console.Write("Cidade 2: ");
        string cidade2 = Console.ReadLine();

        if (!grafo.ContainsKey(cidade1) ||
            !grafo.ContainsKey(cidade2))
        {
            Console.WriteLine("Cidade inválida.");
            return;
        }

        if (grafo[cidade1].Contains(cidade2))
        {
            Console.WriteLine(
                $"{cidade1} e {cidade2} possuem conexão direta!");
        }
        else
        {
            Console.WriteLine(
                $"{cidade1} e {cidade2} NÃO possuem conexão direta.");
        }
    }

    static void ExisteRotaDFS()
    {
        Console.Write("Origem: ");
        string origem = Console.ReadLine();

        Console.Write("Destino: ");
        string destino = Console.ReadLine();

        if (!grafo.ContainsKey(origem) ||
            !grafo.ContainsKey(destino))
        {
            Console.WriteLine("Cidade inválida.");
            return;
        }

        HashSet<string> visitados = new HashSet<string>();

        Console.Write("DFS visitando: ");

        bool encontrou = DFS(origem, destino, visitados);

        Console.WriteLine();

        if (encontrou)
        {
            Console.WriteLine(
                $"Rota encontrada! É possível ir de {origem} até {destino}.");
        }
        else
        {
            Console.WriteLine(
                $"Rota NÃO encontrada. Não é possível ir de {origem} até {destino}.");
        }
    }

    static bool DFS(
        string atual,
        string destino,
        HashSet<string> visitados)
    {
        visitados.Add(atual);

        Console.Write(atual);

        if (atual != destino)
        {
            Console.Write(" -> ");
        }

        if (atual == destino)
        {
            return true;
        }

        foreach (string vizinho in grafo[atual])
        {
            if (!visitados.Contains(vizinho))
            {
                bool encontrou =
                    DFS(vizinho, destino, visitados);

                if (encontrou)
                {
                    return true;
                }
            }
        }

        return false;
    }

    static void MenorRotaBFS()
    {
        Console.Write("Origem: ");
        string origem = Console.ReadLine();

        Console.Write("Destino: ");
        string destino = Console.ReadLine();

        if (!grafo.ContainsKey(origem) ||
            !grafo.ContainsKey(destino))
        {
            Console.WriteLine("Cidade inválida.");
            return;
        }

        Queue<string> fila = new Queue<string>();

        HashSet<string> visitados =
            new HashSet<string>();

        Dictionary<string, string> pai =
            new Dictionary<string, string>();

        fila.Enqueue(origem);
        visitados.Add(origem);

        bool encontrou = false;

        while (fila.Count > 0)
        {
            string atual = fila.Dequeue();

            if (atual == destino)
            {
                encontrou = true;
                break;
            }

            foreach (string vizinho in grafo[atual])
            {
                if (!visitados.Contains(vizinho))
                {
                    visitados.Add(vizinho);

                    fila.Enqueue(vizinho);

                    pai[vizinho] = atual;
                }
            }
        }

        if (!encontrou)
        {
            Console.WriteLine(
                $"Não existe rota entre {origem} e {destino}.");
            return;
        }

        List<string> caminho = new List<string>();

        string cidadeAtual = destino;

        while (cidadeAtual != origem)
        {
            caminho.Add(cidadeAtual);

            cidadeAtual = pai[cidadeAtual];
        }

        caminho.Add(origem);

        caminho.Reverse();

        Console.WriteLine(
            "\nMenor rota (BFS): " +
            string.Join(" -> ", caminho));

        Console.WriteLine(
            "Paradas: " + (caminho.Count - 1));
    }
}