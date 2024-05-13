using System;
using UnityEngine;

[RequireComponent(typeof(Material))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private CubeSpawner _cubeSpawner;

    private int _currentSplitplitChance = 100;
    private int _numberChanceReduction = 2;

    public event Action<Cube> MouseButtonPressed;

    public int CurrentSplitChance => _currentSplitplitChance;
    public Color Color => _material.color;

    private void OnMouseUpAsButton()
    {
        MouseButtonPressed?.Invoke(this);
    }

    public void SplitChance()
    {
        _currentSplitplitChance /= _numberChanceReduction;
    }
}