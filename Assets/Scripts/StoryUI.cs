using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryUI : MonoBehaviour
{
    [SerializeField]
    private Image storyArt;
    
    [SerializeField]
    private Text storyContent;

    [SerializeField]
    private GameObject choicePanel;

    [SerializeField]
    private Button buttonPrefab;

    private string storyString;
    private List<Button> choiceButtons;

    private void Awake()
    {
        choiceButtons = new List<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string newText)
    {
        storyString = newText;
        StartCoroutine(DisplayStoryText());
    }

    public void UpdateArt(Sprite newArt)
    {
        storyArt.sprite = newArt;
    }

    public void UpdateChoices(Storylet.Choice[] choices, StoryManager manager)
    {
        foreach (var choice in choiceButtons)
        {
            Destroy(choice.gameObject);
        }
        
        choiceButtons.Clear();
        
        for (var i = 0; i < choices.Length; ++i)
        {
            Button choice = Instantiate(buttonPrefab, choicePanel.transform, false);
            choice.transform.Translate(Vector3.zero);

            Text choiceText = choice.GetComponentInChildren<Text>();
            choiceText.text = choices[i].ChoiceText;

            var choiceIndex = i;
            choice.onClick.AddListener(delegate { manager.SelectChoice(choiceIndex); });
            
            choiceButtons.Add(choice);
        }
    }

    IEnumerator DisplayStoryText()
    {
        storyContent.text = "";
        choicePanel.SetActive(false);
        
        foreach (var character in storyString)
        {
            storyContent.text += character;
            yield return new WaitForSeconds(0.05f);
        }

        choicePanel.SetActive(true);
    }
}
