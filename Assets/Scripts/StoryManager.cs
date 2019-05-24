using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private Deck deck;
    
    private Storylet currentStorylet;
    private StoryUI storyUI;

    private void Awake()
    {
        storyUI = FindObjectOfType<StoryUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ContinueStory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ContinueStory()
    {
        currentStorylet = deck.DrawStorylet();
        
        storyUI.UpdateChoices(currentStorylet.Choices, this);
        storyUI.UpdateArt(currentStorylet.CardArt);
        storyUI.UpdateText(currentStorylet.Content);
    }

    public void SelectChoice(int choiceIndex)
    {
        currentStorylet.PickChoice(choiceIndex);
        ContinueStory();
    }
}
