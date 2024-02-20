using UnityEngine;
using Random = UnityEngine.Random;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System;

public class NeuralNetwork : MonoBehaviour
{
    private Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 3);
    public List<Matrix<float>> hiddenLayers = new List<Matrix<float>>();
    private Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 2);

    public List<float> biases = new List<float>();
    public List<Matrix<float>> weights = new List<Matrix<float>>();

    public float fitness;

    public void Init(int hiddenLayerCount, int hiddenNeuronCount)
    {
        ClearValue();

        for (int i = 0; i < hiddenLayerCount + 1; i++)
        {
            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
            hiddenLayers.Add(f);
            biases.Add(Random.Range(-1f, 1f));

            // Weights
            if(i == 0)
            {
                Matrix<float> inputForFirstHiddenLayer = Matrix<float>.Build.Dense(3, hiddenNeuronCount);
                weights.Add(inputForFirstHiddenLayer);
            }

            Matrix<float> hiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
            weights.Add(hiddenToHidden);
        }

        Matrix<float> outputWeight = Matrix<float>.Build.Dense(hiddenNeuronCount, 2);
        weights.Add(outputWeight);
        biases.Add(Random.Range(-1f, 1f));

        RandomiseWeight();
    }

    public NeuralNetwork InitCopy(int hiddenLayerCount, int hiddenNeuronCount)
    {
        NeuralNetwork neuralNetwork = new NeuralNetwork();

        // Weight
        List<Matrix<float>> newWeights = new List<Matrix<float>>();
        for(int i = 0; i < weights.Count; i++)
        {
            Matrix<float> currentWeight = Matrix<float>.Build.Dense(weights[i].RowCount, weights[i].ColumnCount);
            for(int j = 0; j < currentWeight.RowCount; j++)
            {
                for(int k = 0; k < currentWeight.ColumnCount; k++)
                {
                    currentWeight[j, k] = weights[i][j, k];
                }
            }

            newWeights.Add(currentWeight);
        }

        // Biases
        List<float> newBiases = new List<float>();
        newBiases.AddRange(biases);

        // Copy
        neuralNetwork.weights = newWeights;
        neuralNetwork.biases = newBiases;
        neuralNetwork.InitHidden(hiddenLayerCount, hiddenNeuronCount);
        
        return neuralNetwork;
    }

    public Output RunNetwork(float a, float b, float c)
    {
        inputLayer[0, 0] = a;
        inputLayer[0, 1] = b;
        inputLayer[0, 2] = c;

        inputLayer = inputLayer.PointwiseTanh();

        hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();
        for (int i = 1; i < hiddenLayers.Count; i++)
            hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        outputLayer = ((hiddenLayers[hiddenLayers.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]).PointwiseTanh();

        return new Output(Sigmoid(outputLayer[0, 0]), (float)Math.Tanh(outputLayer[0, 1]));
    }

    private void ClearValue()
    {
        inputLayer.Clear();
        hiddenLayers.Clear();
        outputLayer.Clear();
        weights.Clear();
        biases.Clear();
    }

    private void RandomiseWeight()
    {
        for(int i = 0; i < weights.Count; i++)
        {
            for(int j = 0; j  < weights[i].RowCount; j++)
            {
                for(int k = 0; k < weights[i].ColumnCount; k++)
                {
                    weights[i][j, k] = Random.Range(-1f, 1f);
                }
            }
        }
    }

    private void InitHidden(int hiddenLayerCount, int hiddenNeuronCount)
    {
        inputLayer.Clear();
        hiddenLayers.Clear();
        outputLayer.Clear();

        for(int i = 0; i < hiddenLayerCount; i++)
        {
            Matrix<float> newHiddenLayer = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
            hiddenLayers.Add(newHiddenLayer);
        }
    }

    private float Sigmoid(float s)
    {
        return 1 / 1 + Mathf.Exp(-s);
    }
}