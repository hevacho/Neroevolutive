using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jump;
    public float timePulsed = 0f;
    public float lifeTime = 0f;
    public bool isAlive = true;
    
    int[] estructura = new int[] { 5, 8, 1 }; // 5 entradas, 1 capas ocultas, 1 salida
    public NeuralNetWork red;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InicializarRed();

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            lifeTime += Time.deltaTime;

            if (Input.GetKey(KeyCode.Space) && isGrounded())
            {
                timePulsed += 10f * Time.deltaTime;
                timePulsed = Mathf.Min(timePulsed, 6f); // Límite máximo

            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * timePulsed * jump);
                timePulsed = 0;
            }

            float[] inputs = ObtenerInputs();
            float[] outputs = red.FeedForward(inputs);

            Debug.Log(outputs[0]);

            if (outputs[0] > 0.5f && isGrounded())
            {
                timePulsed += 10f * Time.deltaTime;
                timePulsed = Mathf.Min(timePulsed, 6f); // Límite máximo
            }
            else
            {
                rb.AddForce(Vector2.up * timePulsed * jump);
                timePulsed = 0;

            }
        }

    }

    float[] ObtenerInputs()
    {
        if(EnemySpawner.enemySpawned != null)
        {
            Mathf.Abs(this.gameObject.transform.position.x - EnemySpawner.enemySpawned.transform.position.x);

        }
        float distancia = (EnemySpawner.enemySpawned != null ? Mathf.Abs(this.gameObject.transform.position.x - EnemySpawner.enemySpawned.transform.position.x) : 18) / 18; 
        float alturaMin = (EnemySpawner.enemySpawned != null ? EnemySpawner.enemySpawned.GetComponent<Enemy>().minSpace : 0) / 8;
        float alturaMax = (EnemySpawner.enemySpawned != null ? EnemySpawner.enemySpawned.GetComponent<Enemy>().maxSpace : 8) / 8;
        float velocidad = GameManager.velocity / 100;

        return new float[]
        {
            isGrounded() ? 1f : 0f,
            distancia,
            alturaMin,
            alturaMax,
            velocidad 
        };
    }



    public bool isGrounded()
    {
        return this.transform.localPosition.y <= - 3.50f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Debug.Log($"ha chocado y ha muerdo depues de {lifeTime:F2} segundos");
            isAlive = false;
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void InicializarRed()
    {
        red = new NeuralNetWork(estructura);
    }

}
