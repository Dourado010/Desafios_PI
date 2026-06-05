using System;
using System.Collections.Generic;
using System.Linq;

/*
=========================================================
EXPLICAÇÃO GERAL DO PROGRAMA
=========================================================

---------------------------------------------------------
1) CLASSE NODE
---------------------------------------------------------

A classe Node representa um nó da árvore.

O construtor recebe um valor e inicializa a altura com 1,
pois um nó recém-criado é uma árvore de altura 1.

---------------------------------------------------------
2) CLASSE BST
---------------------------------------------------------

Ela possui:

- Root -> raiz da árvore.

Método InsertRec():

Realiza a inserção recursiva.

Funcionamento:
Se o nó atual for nulo, cria um novo nó. Se o valor for menor que a chave atual, vai para a
subárvore esquerda. Caso contrário, vai para a subárvore direita. Retorna o nó atualizado.

Método Insert():

É apenas uma interface pública que chama InsertRec()
começando pela raiz da árvore.

Método Altura():

Calcula a altura de forma recursiva.

Fórmula utilizada:

altura = 1 + max(alturaEsquerda, alturaDireita)

Caso o nó seja nulo, retorna 0.

Método GetAltura():

Calcula a altura da árvore inteira iniciando pela raiz.

---------------------------------------------------------
3) CLASSE AVL
---------------------------------------------------------

---------------------------------------------------------
Height()
---------------------------------------------------------

Retorna a altura armazenada em um nó.

Se o nó for nulo, retorna 0.

---------------------------------------------------------
GetBalance()
---------------------------------------------------------

Calcula o fator de balanceamento.

Fórmula:

FB = altura(esquerda) - altura(direita)

FB = 0   -> perfeitamente balanceado
FB = 1   -> balanceado
FB = -1  -> balanceado

Valores maiores que 1 ou menores que -1 indicam
necessidade de rotação.

---------------------------------------------------------
RotateRight()
---------------------------------------------------------

Executa rotação simples para a direita.

Utilizada no caso LL.

---------------------------------------------------------
RotateLeft()
---------------------------------------------------------

Executa rotação simples para a esquerda.

Utilizada no caso RR.

---------------------------------------------------------
InsertRec() da AVL
---------------------------------------------------------

Primeiro realiza a inserção exatamente como uma BST.

Após inserir:

1. Atualiza a altura do nó.
2. Calcula o fator de balanceamento.
3. Verifica se ocorreu algum dos casos:

LL -> rotação simples à direita
RR -> rotação simples à esquerda
LR -> esquerda + direita
RL -> direita + esquerda

---------------------------------------------------------
4) GERAÇÃO DOS NÚMEROS
---------------------------------------------------------

Método GerarNumerosUnicos()

Utiliza:

- Random
- HashSet<int>

O HashSet garante que números repetidos não sejam
armazenados.

O processo continua até gerar N valores distintos.

Ao final, o conjunto é convertido para List<int>.

---------------------------------------------------------
5) PROGRAMA PRINCIPAL
---------------------------------------------------------

O programa executa em um laço infinito exibindo um menu.

Opções:

1 -> Nova simulação
2 -> Encerrar programa

---------------------------------------------------------
6) EXECUÇÃO DE UMA SIMULAÇÃO
---------------------------------------------------------

O usuário informa:

A -> quantidade de amostras
N -> quantidade de elementos por árvore

Para cada amostra:

Cria uma BST vazia. Cria uma AVL vazia. Gera N números aleatórios distintos.
Insere exatamente os mesmos números nas duas árvores. Calcula a altura da BST.
Calcula a altura da AVL. Acumula os resultados.

---------------------------------------------------------
7) CÁLCULO DAS MÉDIAS
---------------------------------------------------------

Após executar todas as amostras:

mediaBST =
somaBST / A

mediaAVL =
somaAVL / A

mediaGeral =
(somaBST + somaAVL) / (2 * A)

=========================================================
FIM DA EXPLICAÇÃO
=========================================================
*/

public class Node
{
    public int Key;
    public Node Left;
    public Node Right;
    public int Height;

    public Node(int key)
    {
        Key = key;
        Height = 1;
    }
}

// ================= BST =================
public class BST
{
    public Node Root;

    public Node InsertRec(Node node, int value)
    {
        if (node == null)
            return new Node(value);

        if (value < node.Key)
            node.Left = InsertRec(node.Left, value);
        else
            node.Right = InsertRec(node.Right, value);

        return node;
    }

    public void Insert(int value)
    {
        Root = InsertRec(Root, value);
    }

    public int Altura(Node node)
    {
        if (node == null) return 0;

        int esq = Altura(node.Left);
        int dir = Altura(node.Right);

        return 1 + Math.Max(esq, dir);
    }

    public int GetAltura()
    {
        return Altura(Root);
    }
}

// ================= AVL =================
public class AVL
{
    public Node Root;

    int Height(Node n)
    {
        return n == null ? 0 : n.Height;
    }

    int GetBalance(Node n)
    {
        return n == null ? 0 : Height(n.Left) - Height(n.Right);
    }

    Node RotateRight(Node y)
    {
        Node x = y.Left;
        Node T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        y.Height = 1 + Math.Max(Height(y.Left), Height(y.Right));
        x.Height = 1 + Math.Max(Height(x.Left), Height(x.Right));

        return x;
    }

    Node RotateLeft(Node x)
    {
        Node y = x.Right;
        Node T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        x.Height = 1 + Math.Max(Height(x.Left), Height(x.Right));
        y.Height = 1 + Math.Max(Height(y.Left), Height(y.Right));

        return y;
    }

    Node InsertRec(Node node, int key)
    {
        if (node == null)
            return new Node(key);

        if (key < node.Key)
            node.Left = InsertRec(node.Left, key);
        else if (key > node.Key)
            node.Right = InsertRec(node.Right, key);
        else
            return node;

        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));

        int balance = GetBalance(node);

        // LL
        if (balance > 1 && key < node.Left.Key)
            return RotateRight(node);

        // RR
        if (balance < -1 && key > node.Right.Key)
            return RotateLeft(node);

        // LR
        if (balance > 1 && key > node.Left.Key)
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }

        // RL
        if (balance < -1 && key < node.Right.Key)
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }

    public void Insert(int key)
    {
        Root = InsertRec(Root, key);
    }

    public int Altura(Node node)
    {
        if (node == null) return 0;
        return 1 + Math.Max(Altura(node.Left), Altura(node.Right));
    }

    public int GetAltura()
    {
        return Altura(Root);
    }
}

// ================= PROGRAMA =================
public class Program
{
    static List<int> GerarNumerosUnicos(int N)
    {
        Random rand = new Random();
        HashSet<int> set = new HashSet<int>();

        while (set.Count < N)
        {
            set.Add(rand.Next(1, 1000));
        }

        return set.ToList();
    }

    public static void Main()
    {
        while (true)
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Menu: 1) nova simulação ou 2) sair");
            string op = Console.ReadLine();

            if (op == "2")
            {
                Console.WriteLine("Tchau!");
                break;
            }

            Console.Write("Digite a quantidade de amostras: ");
            int A = int.Parse(Console.ReadLine());

            Console.Write("Digite a quantidade de elementos: ");
            int N = int.Parse(Console.ReadLine());

            double somaBST = 0;
            double somaAVL = 0;

            for (int i = 0; i < A; i++)
            {
                BST bst = new BST();
                AVL avl = new AVL();

                List<int> numeros = GerarNumerosUnicos(N);

                foreach (int num in numeros)
                {
                    bst.Insert(num);
                    avl.Insert(num);
                }

                int alturaBST = bst.GetAltura();
                int alturaAVL = avl.GetAltura();

                somaBST += alturaBST;
                somaAVL += alturaAVL;
            }

            double mediaBST = somaBST / A;
            double mediaAVL = somaAVL / A;
            double mediaGeral = (somaBST + somaAVL) / (2 * A);

            Console.WriteLine("\nExperimento:");
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Altura média geral:     {mediaGeral:F2}");
            Console.WriteLine($"Altura média BST comum: {mediaBST:F2}");
            Console.WriteLine($"Altura média AVL:       {mediaAVL:F2}");
            Console.WriteLine("----------------------------------\n");
        }
    }
}