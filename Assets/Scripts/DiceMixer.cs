using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceMixer : MonoBehaviour
{
    [SerializeField] private GameObject diePrefab;
    [SerializeField] [Range(0, 10)] private int dieCount;

    private const int ForceMultiplier = 20;
    private const int SpawnRadius = 10;
    private Rigidbody[] _dice;
    private bool _inFlight;

    private readonly (Func<Rigidbody, Vector3> vectorFunc, int score)[] _scores =
    {
        (x => x.transform.forward, 1),
        (x => x.transform.up, 2),
        (x => -x.transform.right, 3),
        (x => x.transform.right, 4),
        (x => -x.transform.up, 5),
        (x => -x.transform.forward, 6)
    };
    
    private void Awake()
    {
        if (diePrefab is null)
        {
            Debug.LogError("DicePrefab is null");
            return;
        }
        
        CreateDice();
    }

    private void Update()
    {
        var isInFlight = _dice.Any(x => x.linearVelocity.sqrMagnitude > 1e-4 && x.angularVelocity.sqrMagnitude > 1e-4);
        if (isInFlight != _inFlight && !isInFlight)
        {
            CalculateScore();
        }
        _inFlight = isInFlight;
    }

    private void CreateDice()
    {
        _dice = new Rigidbody[dieCount];
        
        var spawnAngle = math.PI2 / dieCount;
        for (var i = 0; i < dieCount; i++)
        {
            var die = Instantiate(diePrefab, transform);
            _dice[i] = die.GetComponent<Rigidbody>();
            
            var cubeAngle = spawnAngle * i;
            die.transform.localPosition = new Vector3(SpawnRadius * math.sin(cubeAngle), 0, SpawnRadius * math.cos(cubeAngle));
        }
    }

    private void ThrowDice()
    {
        _inFlight = true;
        foreach (var die in _dice)
        {
            die.AddForce(new Vector3(
                    ForceMultiplier * Random.Range(-1f, 1f),
                    ForceMultiplier * Random.Range(-1f, 1f),
                    ForceMultiplier * Random.Range(-1f, 1f)),
                ForceMode.Impulse);
            die.AddTorque(new Vector3(
                    ForceMultiplier * Random.Range(-1f, 1f),
                    ForceMultiplier * Random.Range(-1f, 1f),
                    ForceMultiplier * Random.Range(-1f, 1f)),
                ForceMode.Impulse);
        }
    }

    private void CalculateScore()
    {
        var totalScore = 0;
        foreach (var die in _dice)
        {
            foreach (var possibleScore in _scores)
            {
                if (possibleScore.vectorFunc(die).ApproximatelyEqual(transform.up))
                {
                    totalScore += possibleScore.score;
                    break;
                }
            }   
        }
        
        Debug.Log($"Total score: {totalScore}");
    }
    
    private void OnRoll()
    {
        if (!_inFlight)
        {
            ThrowDice();
        }
    }
}
