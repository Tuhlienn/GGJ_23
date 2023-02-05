using System.Collections.Generic;
using UnityEngine;
using Tree;

public class AudioManager : MonoBehaviour
{
    private TreeGrowthManager treeManager;

    [SerializeField] private AudioSource background;
    [SerializeField] private AudioSource flower;
    [SerializeField] private AudioSource tick;

    [SerializeField] private List<AudioClip> bloomClips;

    private void Awake()
    {
        treeManager = FindObjectOfType<TreeGrowthManager>();

        background.Play();

        treeManager.OnFlowerEvent += _ => {
            Debug.Log("F");

            if (flower.isPlaying)
                return;

            flower.clip = bloomClips[Random.Range(0, 8)];
            flower.Play();
        };

        treeManager.OnTick += ()=>{
            tick.Play();
        };
    }
}
