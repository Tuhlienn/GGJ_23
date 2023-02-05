using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Hex;
using Tree;
using UnityEngine;
using UnityEngine.Rendering;

public class PlantVisualizer : MonoBehaviour
{
    private static readonly int GrowthProgress = Shader.PropertyToID("_GrowthProgress");
    private static readonly int StartColor = Shader.PropertyToID("_StartColor");
    private static readonly int EndColor = Shader.PropertyToID("_EndColor");

    [Header("Dependencies")]
    [SerializeField] private TreeGrowthManager treeGrowthManager;

    [Header("Settings")]
    // [SerializeField] private float animationDuration;
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
    private int _count;

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
        var stem = new GameObject($"Stem ({previous.Position})")
        {
            transform =
            {
                position = previous.Position.ToWorldPosition(),
                rotation = Quaternion.AngleAxis(GetRotationAngleInDegrees(previous.EntryDirection), Vector3.forward)
            }
        };

        var stemRenderer = stem.AddComponent<SpriteRenderer>();
        stemRenderer.material = stemMaterial;
        stemRenderer.sprite = GetStemSprite(current, previous.EntryDirection);
        stemRenderer.sortingOrder = _count--;

        var mpb = new MaterialPropertyBlock();

        stemRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(GrowthProgress, 0.0f);
        mpb.SetColor(StartColor, previous.Type == BranchNode.NodeType.Root ? rootColor : defaultColor);
        mpb.SetColor(EndColor, defaultColor);
        stemRenderer.SetPropertyBlock(mpb);

        _stems.Add(stem);

        AnimateStem(_animationSource.Token).Forget();

        async UniTaskVoid AnimateStem(CancellationToken cancellationToken)
        {
            float t = 0;
            await DOTween.To(() => t, x => t = x, 1.0f, treeGrowthManager.TickTime).SetEase(Ease.OutQuad)
                .OnUpdate(SetGrowthProgressProp)
                .ToUniTask(TweenCancelBehaviour.Kill, cancellationToken);

            void SetGrowthProgressProp()
            {
                if (stemRenderer == null)
                    return;

                stemRenderer.GetPropertyBlock(mpb);
                mpb.SetFloat(GrowthProgress, t);
                stemRenderer.SetPropertyBlock(mpb);
            }
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
