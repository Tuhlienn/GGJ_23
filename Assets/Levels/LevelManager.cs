using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hex;
using Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private int levelBuildIndexFrom;
        [SerializeField] private int levelBuildIndexTo;

        private int _currentLevelIndex;
        private TreeGrowthManager _growthManager;
        private HexGrid _hexGrid;

        private bool _levelCompleted;
        private readonly List<HexVector> _goalsReached = new();

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _currentLevelIndex = levelBuildIndexFrom;
        }

        private void Start()
        {
            Initialize();
        }

        private async void LoadNextLevel()
        {
            if (_currentLevelIndex == levelBuildIndexTo)
                return;

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            _currentLevelIndex += 1;
            await SceneManager.LoadSceneAsync(_currentLevelIndex);

            Initialize();
        }

        private void Initialize()
        {
            _growthManager = FindObjectOfType<TreeGrowthManager>();
            _hexGrid = FindObjectOfType<HexGrid>();

            _growthManager.OnFlowerEvent += OnFlowerAdded;
            _growthManager.OnTreeFinished += OnTreeFinished;
        }

        private void OnTreeFinished()
        {
            if (!_levelCompleted)
                return;

            _goalsReached.Clear();
            _levelCompleted = false;
            LoadNextLevel();
        }

        private void OnFlowerAdded(BranchNode flowerNode)
        {
            if (flowerNode == null || !_hexGrid.HasGoalAtPosition(flowerNode.Position))
                return;

            _goalsReached.Add(flowerNode.Position);
            if (_hexGrid.AllGoalsReached(_goalsReached))
                _levelCompleted = true;
        }
    }
}
