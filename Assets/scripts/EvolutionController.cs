using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvolutionController : MonoBehaviour
{
    public GameObject playerPrefab;
    public int numJugadores = 5;

    private List<PlayerJump> jugadores = new List<PlayerJump>();
    public int generacion = 1;

    void Start()
    {
        CrearPrimeraGeneracion();
        StartCoroutine(Evolucionar());
    }

    void CrearPrimeraGeneracion()
    {
        for (int i = 0; i < numJugadores; i++)
        {
            GameObject nuevoJugador = Instantiate(playerPrefab);
            jugadores.Add(nuevoJugador.GetComponent<PlayerJump>());
        }
    }

    IEnumerator Evolucionar()
    {
        while (true)
        {
            // Esperar hasta que todos estén muertos
            yield return new WaitUntil(() => jugadores.TrueForAll(j => !j.isAlive));

            Debug.Log($"=== Generación {generacion} terminada ===");

            // Ordenar por puntuación
            jugadores.Sort((a, b) => b.lifeTime.CompareTo(a.lifeTime));

            // Seleccionar mejores
            int top = Mathf.Max(1, jugadores.Count / 10);
            List<PlayerJump> elite = jugadores.GetRange(0, top);

            // Crear nueva generación
            List<PlayerJump> nuevaGeneracion = new List<PlayerJump>();

            for (int i = 0; i < numJugadores; i++)
            {
                PlayerJump padre1 = elite[Random.Range(0, elite.Count)];
                PlayerJump padre2 = elite[Random.Range(0, elite.Count)];

                GameObject nuevoGO = Instantiate(playerPrefab);
                PlayerJump hijo = nuevoGO.GetComponent<PlayerJump>();
                hijo.InicializarRed();

                CruzarRedesNeuronales(padre1, padre2, hijo);
                Mutar(hijo.red);

                nuevaGeneracion.Add(hijo);
            }

            // Eliminar generación anterior
            foreach (PlayerJump p in jugadores)
                Destroy(p.gameObject);

            jugadores = nuevaGeneracion;
            generacion++;

            Debug.Log($"--- Generación {generacion} creada ---");
        }
    }

    void CruzarRedesNeuronales(PlayerJump p1, PlayerJump p2, PlayerJump hijo)
    {
        for (int l = 0; l < p1.red.layers.Count; l++)
        {
            for (int n = 0; n < p1.red.layers[l].Count; n++)
            {
                Neuron neuronaHijo = hijo.red.layers[l][n];
                Neuron neuronaPadre1 = p1.red.layers[l][n];
                Neuron neuronaPadre2 = p2.red.layers[l][n];

                for (int w = 0; w < neuronaHijo.weights.Length; w++)
                {
                    neuronaHijo.weights[w] = Random.value < 0.5f ? neuronaPadre1.weights[w] : neuronaPadre2.weights[w];
                }

                neuronaHijo.bias = Random.value < 0.5f ? neuronaPadre1.bias : neuronaPadre2.bias;
            }
        }
    }

    void Mutar(NeuralNetWork red)
    {
        float tasaMutacion = 0.05f;

        foreach (var capa in red.layers)
        {
            foreach (var neurona in capa)
            {
                for (int i = 0; i < neurona.weights.Length; i++)
                {
                    if (Random.value < tasaMutacion)
                        neurona.weights[i] += Random.Range(-0.5f, 0.5f);
                }

                if (Random.value < tasaMutacion)
                    neurona.bias += Random.Range(-0.5f, 0.5f);
            }
        }
    }
}
