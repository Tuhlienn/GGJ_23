using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using Cysharp.Threading.Tasks;
using HexGrid;
using Tree;
using Unity.Mathematics;
using UnityEngine;

public class PlantVisualizer : MonoBehaviour
{
    private static readonly int GrowthProgress = Shader.PropertyToID("_GrowthProgress");

    [Header("Dependencies")]
    [SerializeField] private TreeGrowthManager treeGrowthManager;

    [Header("Settings")]
    [SerializeField] private float animationDuration;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color rootColor;

    [Header("Assets")]
    [SerializeField] private Sprite straight;
    [SerializeField] private Sprite leftTurn;
    [SerializeField] private Sprite rightTurn;
    [SerializeField] private Sprite end;
    [SerializeField] private Material stemMaterial;

    private readonly List<GameObject> _stems = new();

    private CancellationTokenSource _animationSource = new();
    private uint _count;
    private static readonly int StartColor = Shader.PropertyToID("_StartColor");
    private static readonly int EndColor = Shader.PropertyToID("_EndColor");

    private void Awake()
    {
        treeGrowthManager.OnNodesUpdate += TreeGrowthManagerOnOnNodesUpdate;
        treeGrowthManager.OnNodesReset += TreeGrowthManagerOnOnNodesReset;
    }

    private void TreeGrowthManagerOnOnNodesReset()
    {
        _animationSource.Cancel();
        _animationSource.Dispose();

        foreach (GameObject stem in _stems)
            Destroy(stem);

        _stems.Clear();

        _animationSource = new CancellationTokenSource();
        _count = 0;
    }

    private void TreeGrowthManagerOnOnNodesUpdate(List<(BranchNode current, BranchNode next)> updates)
    {
        foreach ((BranchNode current, BranchNode next) in updates)
            OnVisualizeGrowth(next, current);
    }

    private void OnVisualizeGrowth(BranchNode current, BranchNode previous)
    {
        Vector3 position = previous.Position.ToWorldPosition();
        position.z = _count++ * 0.01f;

        var stem = new GameObject($"Stem ({previous.Position})")
        {
            transform =
            {
                position = position,
                rotation = Quaternion.AngleAxis(GetRotationAngleInDegrees(previous.EntryDirection), Vector3.forward)
            }
        };

        var stemRenderer = stem.AddComponent<SpriteRenderer>();
        stemRenderer.material = stemMaterial;
        stemRenderer.sprite = GetStemSprite(current, previous.EntryDirection);

        var mpb = new MaterialPropertyBlock();

        stemRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(StartColor, previous.Type == BranchNode.NodeType.Root ? rootColor : defaultColor);
        mpb.SetColor(EndColor, defaultColor);
        stemRenderer.SetPropertyBlock(mpb);

        _stems.Add(stem);

        AnimateStem(_animationSource.Token).Forget();

        async UniTaskVoid AnimateStem(CancellationToken cancellationToken)
        {
            float startTime = Time.time;

            while (Time.time - startTime <= animationDuration && !cancellationToken.IsCancellationRequested && stemRenderer != null)
            {
                float t = math.saturate((Time.time - startTime) / animationDuration);

                stemRenderer.GetPropertyBlock(mpb);
                mpb.SetFloat(GrowthProgress, t);
                stemRenderer.SetPropertyBlock(mpb);

                await UniTask.NextFrame(PlayerLoopTiming.PostLateUpdate, cancellationToken);
            }

            stemRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(GrowthProgress, 1.0f);
            stemRenderer.SetPropertyBlock(mpb);
        }
    }

    private static float GetRotationAngleInDegrees(HexVector previousDirection)
    {
        if (previousDirection == HexVector.Up)
            return 0.0f;

        if (previousDirection == HexVector.UpLeft)
            return 60.0f;

        if (previousDirection == HexVector.DownLeft)
            return 120.0f;

        if (previousDirection == HexVector.UpRight)
            return -60.0f;

        if (previousDirection == HexVector.DownRight)
            return -120.0f;

        if (previousDirection == HexVector.Down)
            return 180.0f;

        throw new ArgumentOutOfRangeException();
    }

    private Sprite GetStemSprite(BranchNode current, HexVector previousDirection)
    {
        if (current == null)
            return end;

        if (current.EntryDirection == previousDirection)
            return straight;

        if (current.EntryDirection == previousDirection.RotateLeft())
            return leftTurn;

        if (current.EntryDirection == previousDirection.RotateRight())
            return rightTurn;

        throw new ArgumentOutOfRangeException();
    }
}
