using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Instructions;
using Tree;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button pauseButton;
    [SerializeField] Button stopButton;
    [SerializeField] Button resetButton;
    TreeGrowthManager manager;
    Node[] nodes;

    public bool IsGrowing {get; private set;} = false;
    public bool IsPaused {get; private set;} = false;

    void Awake()
    {
        manager = FindObjectOfType<TreeGrowthManager>();
        manager.OnTreeFinished += Stop;
        nodes = FindObjectsOfType<Node>();

        Debug.Log(playButton);
        playButton.onClick.AddListener(Play);
        stopButton.onClick.AddListener(Stop);
        pauseButton.onClick.AddListener(Pause);
        resetButton.onClick.AddListener(ResetNodes);
    }

    public void Play()
    {
        if(!IsPaused)
            manager.StartNewTree();


        IsGrowing = true;
        IsPaused = false;
        UpdateButtonStates();

        manager.SetRunning(true);
        
        foreach(Node n in nodes)
            n.Locked = true;
    }

    public void Stop()
    {
        IsGrowing = false;
        IsPaused = false;
        UpdateButtonStates();

        manager.SetRunning(false);
        
        foreach(Node n in nodes)
            n.Locked = false;
    }

    public void Pause()
    {
        IsPaused = true;
        manager.SetRunning(false);
        UpdateButtonStates();
    }

    public void ResetNodes()
    {
        foreach(Node n in nodes)
            n.ClearConnections();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!IsGrowing)
                Play();
            else
                Stop();
        }
    }

    void UpdateButtonStates()
    {
        playButton.gameObject.SetActive(!IsGrowing || IsPaused);
        resetButton.gameObject.SetActive(!IsGrowing);
        stopButton.gameObject.SetActive(IsGrowing);
        pauseButton.gameObject.SetActive(IsGrowing && !IsPaused);
    }
}
