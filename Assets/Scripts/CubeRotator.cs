using System;
using Unity.Mathematics;
using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private uint cubeCount;
    [SerializeField] [Min(0)] private float radius;
    [SerializeField] [Min(0)] private float rotationSpeed;
    [SerializeField] private bool rotateLeft;

    private float _spawnAngle;

    private void Awake()
    {
        _spawnAngle = math.PI2 / cubeCount;
        for (var i = 0; i < cubeCount; i++)
        {
            var cube = Instantiate(cubePrefab, transform);
            var cubeAngle = _spawnAngle * i;
            cube.transform.localPosition = new Vector3(radius * math.sin(cubeAngle), 0, radius * math.cos(cubeAngle));
        }
    }

    private void FixedUpdate()
    {
        var newY = rotateLeft
            ? transform.eulerAngles.y - rotationSpeed
            : transform.eulerAngles.y + rotationSpeed;
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            newY,
            transform.eulerAngles.z);
    }

    private void Update()
    {
        var i = 0;
        foreach (Transform cube in transform)
        {
            var cubeAngle = _spawnAngle * i;
            cube.localPosition = new Vector3(radius * math.sin(cubeAngle), 0, radius * math.cos(cubeAngle));
            i++;
        }
    }
}
