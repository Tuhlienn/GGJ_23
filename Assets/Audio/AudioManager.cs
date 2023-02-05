using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree;

public class AudioManager : MonoBehaviour
{
    private TreeGrowthManager treeManager;

    [SerializeField] private AudioSource background;
    [SerializeField] private AudioSource flower;
    [SerializeField] private AudioSource tick;


    void Awake()
    {
        treeManager = FindObjectOfType<TreeGrowthManager>();

        background.Play();
        treeManager.OnFlowerEvent += (BranchNode b)=>{
            Debug.Log("F");
            if(!flower.isPlaying)
                flower.Play();
        };
        treeManager.OnTick += ()=>{
            tick.Play();
        };
    }
}
