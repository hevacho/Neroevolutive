using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuralNetWork
{
    public List<List<Neuron>> layers = new List<List<Neuron>>();

    public NeuralNetWork(int[] layerStructure)
    {
        for (int i = 1; i < layerStructure.Length; i++)
        {
            List<Neuron> layer = new List<Neuron>();
            for (int j = 0; j < layerStructure[i]; j++)
            {
                layer.Add(new Neuron(layerStructure[i - 1]));
            }
            layers.Add(layer);
        }
    }

    public float[] FeedForward(float[] inputs)
    {
        float[] outputs = inputs;

        foreach (var layer in layers)
        {
            float[] newOutputs = new float[layer.Count]; //output de una capa
            for (int i = 0; i < layer.Count; i++)
            {
                newOutputs[i] = layer[i].Activate(outputs); //output de cada neurona
            }
            outputs = newOutputs;
        }

        return outputs;
    }
}
