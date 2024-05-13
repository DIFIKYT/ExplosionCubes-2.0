using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Explosion _explosionManager;
    [SerializeField] private List<Color> _cubeColors;
    [SerializeField] private List<Cube> _cubes;

    public event Action CubeSplit;

    private int _numberSizeReductions = 2;
    private int _minCountCubesSpawn = 2;
    private int _maxCountCubesSpawn = 6;

    private void OnEnable()
    {
        foreach (Cube cube in _cubes)
            cube.MouseButtonPressed += SplitCube;
    }

    private void OnDisable()
    {
        foreach (Cube cube in _cubes)
            cube.MouseButtonPressed -= SplitCube;
    }

    private Cube SpawnCube(Cube originalCube)
    {
        Cube newCube = Instantiate(originalCube, originalCube.transform.position, originalCube.transform.rotation);

        newCube.transform.localScale = originalCube.transform.localScale / _numberSizeReductions;
        newCube.GetComponent<Renderer>().material.color = SelectColor();
        newCube.SplitChance();

        AddCubeInList(newCube);

        return newCube;
    }

    private void SplitCube(Cube cube)
    {
        List<Cube> _spawnedCubes = new();
        int maxSplitChance = 100;
        int minSplitChance = 1;

        if (UnityEngine.Random.Range(minSplitChance, maxSplitChance) <= cube.CurrentSplitChance)
        {
            int countCubesSpawn = UnityEngine.Random.Range(_minCountCubesSpawn, _maxCountCubesSpawn);

            for (int i = 0; i < countCubesSpawn; i++)
                _spawnedCubes.Add(SpawnCube(cube));

            _explosionManager.ExplodeCreatedCubes(cube.transform.position, _spawnedCubes);
        }
        else
        {
            _explosionManager.Explode(cube.transform.position, cube.transform.localScale);
        }

        _cubes.Remove(cube);
        cube.MouseButtonPressed -= SplitCube;
        _spawnedCubes.Clear();
        Destroy(cube.gameObject);
    }

    private Color SelectColor()
    {
        return _cubeColors[UnityEngine.Random.Range(0, _cubeColors.Count)];
    }

    private void AddCubeInList(Cube cube)
    {
        _cubes.Add(cube);
        cube.MouseButtonPressed += SplitCube;
    }
}