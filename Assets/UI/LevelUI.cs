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
    [SerializeField] Button stopButton;
    [SerializeField] Button resetButton;
    TreeGrowthManager manager;
    Node[] nodes;

    public bool IsRunning {get; private set;} = false;

    void Awake()
    {
        manager = FindObjectOfType<TreeGrowthManager>();
        nodes = FindObjectsOfType<Node>();

        Debug.Log(playButton);
        playButton.onClick.AddListener(Play);
        stopButton.onClick.AddListener(Stop);
        resetButton.onClick.AddListener(ResetNodes);
    }

    public void Play()
    {
        if(IsRunning)
            return;

        IsRunning = true;
        UpdateButtonStates();

        manager.StartNewTree();
        manager.SetRunning(true);
        
        foreach(Node n in nodes)
            n.Locked = true;
    }

    public void Stop()
    {
        if(!IsRunning)
            return;

        IsRunning = false;
        UpdateButtonStates();

        manager.SetRunning(false);
        
        foreach(Node n in nodes)
            n.Locked = false;
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
            if(!IsRunning)
                Play();
            else
                Stop();
        }
    }

    void UpdateButtonStates()
    {
        playButton.gameObject.SetActive(!IsRunning);
        resetButton.gameObject.SetActive(!IsRunning);
        stopButton.gameObject.SetActive(IsRunning);
    }
}
