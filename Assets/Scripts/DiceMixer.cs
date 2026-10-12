using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceMixer : MonoBehaviour
{
    [SerializeField] private Rigidbody diePrefab;
    [SerializeField] [Range(0, 10)] private int dieCount;

    private const int ForceMultiplier = 20;
    private const int SpawnRadius = 10;
    private readonly List<Rigidbody> _dice = new();
    private bool _inFlight;
    private int _scoreAim;
    private float _flightTime;

    private readonly (Func<Rigidbody, Vector3> vectorFunc, int score)[] _scores =
    {
        (x => x.transform.forward, 1),
        (x => x.transform.up, 2),
        (x => -x.transform.right, 3),
        (x => x.transform.right, 4),
        (x => -x.transform.up, 5),
        (x => -x.transform.forward, 6)
    };

    public event Action<int> OnDieCountChange;
    public event Action<int> OnScoreAimChange;
    public event Action<int, RoundResult> OnScoreChange;
    
    private void Awake()
    {
        if (diePrefab is null)
        {
            Debug.LogError("DicePrefab is null");
            return;
        }
        
        CreateDice();
        
        OnDieCountChange?.Invoke(dieCount);
        OnScoreAimChange?.Invoke(_scoreAim);
        OnScoreChange?.Invoke(0, RoundResult.None);
    }

    private void Update()
    {
        if (!_inFlight || Time.time - _flightTime < 0.3f)
        {
            return;
        }

        var isInFlight = _dice.Any(x => x.linearVelocity.sqrMagnitude > 1e-4 || x.angularVelocity.sqrMagnitude > 1e-4);
        if (isInFlight)
        {
            return;
        }
        
        CalculateScore();
        _inFlight = false;
    }

    public void ThrowDice()
    {
        if (_inFlight)
        {
            return;
        }

        _flightTime = Time.time;
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

    public void SetDieCount(string value)
    {
        if (!int.TryParse(value, out var parsedValue))
        {
            return;
        }

        dieCount = parsedValue;
        OnDieCountChange?.Invoke(dieCount);
        RecreateDice();
    }
    
    public void SetScoreAim(string value)
    {
        if (!int.TryParse(value, out var parsedValue))
        {
            return;
        }

        _scoreAim = parsedValue;
        OnScoreAimChange?.Invoke(_scoreAim);
    }

    private void RecreateDice()
    {
        foreach (var die in _dice)
        {
            Destroy(die.gameObject);
        }
        _dice.Clear();
        CreateDice();
    }
        
    private void CreateDice()
    {
        var spawnAngle = math.PI2 / dieCount;
        for (var i = 0; i < dieCount; i++)
        {
            var die = Instantiate(diePrefab, transform);
            _dice.Add(die);
            
            var cubeAngle = spawnAngle * i;
            die.transform.localPosition = new Vector3(SpawnRadius * math.sin(cubeAngle), 0, SpawnRadius * math.cos(cubeAngle));
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

        var roundResult = RoundResult.Lose;
        if (totalScore > _scoreAim)
        {
            roundResult = RoundResult.Win;
        }
        else if (totalScore == _scoreAim)
        {
            roundResult = RoundResult.Draw;
        }
        
        OnScoreChange?.Invoke(totalScore, roundResult);
    }
    
    private void OnRoll()
    {
        ThrowDice();
    }
}
