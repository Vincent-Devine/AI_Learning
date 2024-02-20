using UnityEngine;
using Random = UnityEngine.Random;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.IO;
using System;

public class GeneticManager : MonoBehaviour
{
    public static GeneticManager Instance;

    [Header("References")]
    [SerializeField] private Car car;

    [Header("Controls")]
    [SerializeField] private int initialPopulation = 85;
    [Range(0.0f, 1.0f)][SerializeField] private float mutationRate;

    [Header("Crossover Controls")]
    [SerializeField] private int bestPlannerAgentSelection = 8;
    [SerializeField] private int worstPlannerAgentSelection = 3;
    [SerializeField] private int numberToCrossover;

    private List<int> geneticPool = new List<int>();
    private int naturallySelected;
    private NeuralNetwork[] population;

    [Header("Debug")]
    [SerializeField] private int currentGeneration;
    [SerializeField] private int currentGenome = 0;

    private bool useTrainGenome = false;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CreatePopulation();
    }

    private void CreatePopulation()
    {
        population = new NeuralNetwork[initialPopulation];
        FillPopulationWithRandomValues(population, 0);
        ResetToCurrentGenome();
    }

    public void Death(float fitness, NeuralNetwork neuralNetwork)
    {
        if(useTrainGenome)
        {
            car.Reset();
            return;
        }

        if(currentGenome < population.Length - 1)
        {
            population[currentGenome].fitness = fitness;
            currentGenome++;
            ResetToCurrentGenome();
        }
        else
        {
            Repopulate();
        }
    }

    public void SaveCurrentGenome()
    {
        if (useTrainGenome)
            return;

        Debug.Log("Save current genome!");
        NeuralNetworkSave.SaveData(population[currentGenome]);
    }

    public void RestartTrainAI()
    {
        Debug.Log("Restart training!");
        CreatePopulation();
        currentGeneration = 0;
        currentGenome = 0;
        useTrainGenome = false;
    }

    public void SetupTrainAI()
    {
        NeuralNetwork neuralNetworkTrain = NeuralNetworkSave.LoadData(car.GetLayers(), car.GetNeurons());
        Debug.Log("Use train AI");
        useTrainGenome = true;
        car.ResetWithNetwork(neuralNetworkTrain);
    }

    private void FillPopulationWithRandomValues(NeuralNetwork[] population, int statingIndex)
    {
        while (statingIndex < initialPopulation)
        {
            population[statingIndex] = new NeuralNetwork();
            population[statingIndex].Init(car.GetLayers(), car.GetNeurons());
            statingIndex++;
        }
    }

    private void ResetToCurrentGenome()
    {
        car.ResetWithNetwork(population[currentGenome]);
    }

    private void Repopulate()
    {
        geneticPool.Clear();
        currentGeneration++;
        naturallySelected = 0;

        SortPopulation();

        NeuralNetwork[] newPopulation = PickBestPopulation();

        Crossover(newPopulation);
        Mutate(newPopulation);

        FillPopulationWithRandomValues(newPopulation, naturallySelected);

        population = newPopulation;
        currentGenome = 0;
        ResetToCurrentGenome();
    }

    private void SortPopulation()
    {
        // Bubble sort
        for(int i = 0; i < population.Length; i++)
        {
            for(int j = i; j < population.Length; j++)
            {
                if (population[i].fitness < population[j].fitness)
                {
                    NeuralNetwork temp = population[i];
                    population[i] = population[j];
                    population[j] = temp;
                }
            }
        }
    }

    private NeuralNetwork[] PickBestPopulation()
    {
        NeuralNetwork[] newPopulation = new NeuralNetwork[initialPopulation];

        for(int i = 0; i < bestPlannerAgentSelection; i++)
        {
            newPopulation[naturallySelected] = population[i].InitCopy(car.GetLayers(), car.GetNeurons());
            newPopulation[naturallySelected].fitness = 0;
            naturallySelected++;

            int f = Mathf.RoundToInt(population[i].fitness * 10);

            for (int j = 0; j < f; j++)
                geneticPool.Add(i);
        }

        for(int i = 0; i < worstPlannerAgentSelection; i++)
        {
            int last = population.Length - 1;
            last -= i;

            int f = Mathf.RoundToInt(population[last].fitness * 10);
            for (int j = 0; j < f; j++)
                geneticPool.Add(last);
        }

        return newPopulation;
    }

    private void Crossover(NeuralNetwork[] newPopulation)
    {
        for(int i = 0; i < numberToCrossover; i += 2)
        {
            int AIndex = i;
            int BIndex = i + 1;

            if(geneticPool.Count >= 1)
            {
                for(int j = 0; j < 100; j++)
                {
                    AIndex = geneticPool[Random.Range(0, geneticPool.Count)];
                    BIndex = geneticPool[Random.Range(0, geneticPool.Count)];

                    if (AIndex != BIndex)
                        break;
                }
            }

            NeuralNetwork child1 = new NeuralNetwork();
            NeuralNetwork child2 = new NeuralNetwork();

            child1.Init(car.GetLayers(), car.GetNeurons());
            child2.Init(car.GetLayers(), car.GetNeurons());

            child1.fitness = 0;
            child2.fitness = 0;

            for(int k = 0; k < child1.weights.Count; k++)
            {
                if (Random.Range(0f, 1f) < .5f)
                {
                    child1.weights[k] = population[AIndex].weights[k];
                    child2.weights[k] = population[BIndex].weights[k];
                }
                else
                {
                    child1.weights[k] = population[BIndex].weights[k];
                    child2.weights[k] = population[AIndex].weights[k];
                }
            }

            for (int k = 0; k < child1.biases.Count; k++)
            {
                if (Random.Range(0f, 1f) < .5f)
                {
                    child1.biases[k] = population[AIndex].biases[k];
                    child2.biases[k] = population[BIndex].biases[k];
                }
                else
                {
                    child1.biases[k] = population[BIndex].biases[k];
                    child2.biases[k] = population[AIndex].biases[k];
                }
            }

            newPopulation[naturallySelected] = child1;
            naturallySelected++;

            newPopulation[naturallySelected] = child2;
            naturallySelected++;
        }
    }

    private void Mutate(NeuralNetwork[] neuralNetworks)
    {
        for(int i = 0; i < naturallySelected; i++)
        {
            for(int j = 0; j < neuralNetworks[i].weights.Count; j++)
            {
                if(Random.Range(0f, 1f) < mutationRate)
                    neuralNetworks[i].weights[j] = MutateMatrix(neuralNetworks[i].weights[j]);
            }
        }
    }

    private Matrix<float> MutateMatrix(Matrix<float> initialMatrix)
    {
        int randomPoint = Random.Range(1, (initialMatrix.RowCount * initialMatrix.ColumnCount) / 7);
        Matrix<float> mutateMatrix = initialMatrix;

        for(int i = 0; i < randomPoint; i++)
        {
            int randomColumn = Random.Range(0, mutateMatrix.ColumnCount);
            int randomRow = Random.Range(0, mutateMatrix.RowCount);

            mutateMatrix[randomRow, randomColumn] = Mathf.Clamp(mutateMatrix[randomRow , randomColumn] + Random.Range(-1f, 1f), -1f, 1f);
        }

        return mutateMatrix;
    }
}
