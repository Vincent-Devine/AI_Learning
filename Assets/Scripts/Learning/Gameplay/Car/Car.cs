using UnityEngine;
using UnityEngine.Jobs;

[RequireComponent(typeof(NeuralNetwork))]
public class Car : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 startRotation;
    private Vector3 lastPosition;

    [Range(-1f, 1f)] [SerializeField] private float acceleration;
    [Range(-1f, 1f)] [SerializeField] private float turning;

    private float timeSinceStart = 0f;
    private float totalDistanceTravelled;
    private float avgSpeed;

    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 input;

    [Header("Fitness")]
    [SerializeField] private float overallFitness;
    [Range(0f, 2f)] [SerializeField] private float distanceMultiplier = 1.4f;   // How importance the distance is to the fitness function
    [Range(0f, 2f)] [SerializeField] private float avgSpeedMultiplier = 0.2f;   // How importance the speed is to the fitness function
    [Range(0f, 2f)] [SerializeField] private float sencorMultiplier = 0.1f;     // How importance the sencor is to the fitness function

    // Input to your neural network (distance)
    private float aSensor;
    private float bSensor;
    private float cSensor;

    // Neural Network
    private NeuralNetwork neuralNetwork;
    [Header("Network options")]
    [SerializeField] private int layers = 1;
    [SerializeField] private int neurons = 3;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        neuralNetwork = GetComponent<NeuralNetwork>();

        neuralNetwork.Init(layers, neurons);
    }

    private void FixedUpdate()
    {
        InputSensors();
        lastPosition = transform.position;

        Output output = neuralNetwork.RunNetwork(aSensor, bSensor, cSensor);
        acceleration = output.acceleration;
        turning = output.turning;

        MoveCar(acceleration, turning);

        timeSinceStart += Time.deltaTime;

        CalculateFitness();
    }

    public void Reset()
    {
        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        avgSpeed = 0f;
        lastPosition = startPosition;
        overallFitness = 0f;
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
        acceleration = 0f;
        turning = 0f;
    }

    public void ResetWithNetwork(NeuralNetwork neuralNetwork)
    {
        this.neuralNetwork = neuralNetwork;
        Reset();
    }

    private void MoveCar(float acceleration, float rotation)
    {
        input = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, acceleration * accelerationSpeed * Time.deltaTime), 0.02f);
        input = transform.TransformDirection(input);
        transform.position += input;

        transform.eulerAngles += new Vector3(0f, (rotation * 90f) * rotationSpeed, 0f);
    }

    private void CalculateFitness()
    {
        totalDistanceTravelled += Vector3.Distance(transform.position, lastPosition);
        avgSpeed = totalDistanceTravelled / timeSinceStart;

        overallFitness = (totalDistanceTravelled * distanceMultiplier)
            + (avgSpeed * avgSpeedMultiplier)
            + (aSensor * bSensor * cSensor / 3 * sencorMultiplier);

        if (timeSinceStart > 20f && overallFitness < 40f)
            Death();
    }

    private void InputSensors()
    {
        Vector3 a = transform.forward + transform.right;
        Vector3 b = transform.forward;
        Vector3 c = transform.forward - transform.right;

        Ray ray = new Ray(transform.position + Vector3.up, a);
        RaycastHit hit;
        float maxDistance = 20f;

        if(Physics.Raycast(ray, out hit))
            aSensor = hit.distance / maxDistance;

        ray.direction = b;
        if(Physics.Raycast(ray,out hit))
            bSensor = hit.distance / maxDistance;

        ray.direction = c;
        if(Physics.Raycast(ray,out hit))
            cSensor = hit.distance / maxDistance;
    }

    private void Death()
    {
        GeneticManager.Instance.Death(overallFitness, neuralNetwork);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
            Death();
    }

    public int GetLayers() { return layers; }
    public int GetNeurons() { return neurons; }
}
