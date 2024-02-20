using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class NeuralNetworkSave
{
    private const string FILE_NAME = "CarsData.txt";
    private const string FOLDER_NAME = "/Data/";
    private const int INPUT_COUNT = 3;
    private const int OUTPUT_COUNT = 2;

    public static void SaveData(NeuralNetwork neuralNetwork)
    {
        string dataToSave = neuralNetwork.fitness.ToString() + "\n";
        dataToSave += string.Join(".", neuralNetwork.biases) + "\n";

        // weights
        for (int i  = 0; i < neuralNetwork.weights.Count; i++)
        {
            for(int j = 0; j < neuralNetwork.weights[i].RowCount; j++)
            {
                for(int k = 0; k < neuralNetwork.weights[i].ColumnCount; k++)
                {
                    dataToSave += neuralNetwork.weights[i][j, k].ToString() + ".";
                }
                dataToSave += "\n";
            }
        }

        // hidden layer
        for (int i = 0; i < neuralNetwork.hiddenLayers.Count; i++)
        {
            for (int j = 0; j < neuralNetwork.hiddenLayers[i].RowCount; j++)
            {
                for (int k = 0; k < neuralNetwork.hiddenLayers[i].ColumnCount; k++)
                {
                    dataToSave += neuralNetwork.hiddenLayers[i][j, k].ToString() + ".";
                }
                dataToSave += "\n";
            }
        }

        File.WriteAllText(Application.dataPath + FOLDER_NAME + FILE_NAME, dataToSave);
    }

    public static NeuralNetwork LoadData(int hiddenLayerCount, int hiddenNeuronCount)
    {
        if(!File.Exists(Application.dataPath + FOLDER_NAME + FILE_NAME))
        {
            Debug.Log("No train AI file!");
            return null;
        }

        NeuralNetwork neuralNetwork = new NeuralNetwork();

        string data = File.ReadAllText(Application.dataPath + FOLDER_NAME + FILE_NAME);
        string[] lines = data.Split('\n');
        int currentLine = 0;

        neuralNetwork.fitness = float.Parse(lines[currentLine]);
        currentLine++;
        neuralNetwork.biases = new List<float>(Array.ConvertAll(lines[currentLine].Split('.'), float.Parse));
        currentLine++;

        // Input weight
        Matrix<float> weight = Matrix<float>.Build.Dense(INPUT_COUNT, hiddenNeuronCount);
        for (int i = 0; i < INPUT_COUNT; i++)
        {
            string[] values = lines[currentLine].Split(".");
            currentLine++;
            for(int j = 0; j < hiddenNeuronCount; j++)
                weight[i, j] = float.Parse(values[j]);
        }
        neuralNetwork.weights.Add(weight);

        // Hidden layer weight
        for (int i = 0; i < hiddenLayerCount + 1; i++)
        {
            weight = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
            for(int j = 0; j < hiddenNeuronCount; j++)
            {
                string[] values = lines[currentLine].Split(".");
                currentLine++;
                for (int k = 0; k < hiddenNeuronCount; k++)
                {
                    weight[j, k] = float.Parse(values[k]);
                }
            }
            neuralNetwork.weights.Add(weight);
        }

        // Output weight
        weight = Matrix<float>.Build.Dense(hiddenNeuronCount, OUTPUT_COUNT);
        for (int i = 0; i < hiddenNeuronCount; i++)
        {
            string[] values = lines[currentLine].Split(".");
            currentLine++;
            for (int j = 0; j < OUTPUT_COUNT; j++)
                weight[i, j] = float.Parse(values[j]);
        }
        neuralNetwork.weights.Add(weight);

        // Hidden layer
        Matrix<float> hiddenLayers = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
        for (int i = 0; i < hiddenLayerCount + 1; i++)
        {
            string[] values = lines[currentLine].Split(".");
            currentLine++;
            for (int j = 0; j < hiddenNeuronCount; j++)
                hiddenLayers[0, j] = float.Parse(values[j]);
            
            neuralNetwork.hiddenLayers.Add(hiddenLayers);
        }


        return neuralNetwork;
    }
}
