using UnityEngine;

[System.Serializable]
public class Neuron
{
    public float[] weights;
    public float bias;

    public Neuron(int numInputs)
    {
        weights = new float[numInputs];
        bias = Random.Range(-1f, 1f);
        for(int i = 0; i< weights.Length; i++)
        {
            weights[i] = Random.Range(-1f, 1f);
        }
    }

    public float Activate(float[] inputs)
    {
        float sum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i] * inputs[i];
        }
        sum += bias;

        return Sigmoid(sum);
    }

    private float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

}
