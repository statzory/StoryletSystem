using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


[CreateAssetMenu(fileName = "New Storylet", menuName = "Storylet", order = 51)]
public class Storylet : ScriptableObject
{
    public enum PreconditionType { equalTo, notEqualTo, greaterThan, lessThan, greaterThanOrEqualTo, lessThanOrEqualTo }
    public enum PostconditionType { setTo, add, subtract }

    [Serializable]
    public class Precondition
    {
        public string Key;
        public PreconditionType Type;
        public int Value;

        public bool IsMet(StoryState story)
        {
            switch (Type)
            {
                case PreconditionType.equalTo:
                    if (story[Key] != Value)
                        return false;
                    break;
                
                case PreconditionType.greaterThan:
                    if (story[Key] <= Value)
                        return false;
                    break;
                
                case PreconditionType.lessThan:
                    if (story[Key] >= Value)
                        return false;
                    break;
                
                case PreconditionType.notEqualTo:
                    if (story[Key] == Value)
                        return false;
                    break;
                
                case PreconditionType.greaterThanOrEqualTo:
                    if (story[Key] < Value)
                        return false;
                    break;
                
                case PreconditionType.lessThanOrEqualTo:
                    if (story[Key] > Value)
                        return false;
                    break;
            }

            return true;
        }
    }
    
    [Serializable]
    public class Postcondition
    {
        public string Key;
        public PostconditionType Type;
        public int Value;
    }

    [Serializable]
    public class Choice
    {
        public string ChoiceText;
        public Postcondition[] Postconditions;
    }

    [SerializeField]
    private StoryState story;
    
    public Sprite CardArt;

    public bool Repeatable;
    
    public Precondition[] Preconditions;
    
    [SerializeField]
    [TextArea]
    public string content;
    
    public Choice[] Choices;
    public string Content
    {
        get
        {
            var pattern = @"\{([^}]*)\}";
            var tempContent = string.Copy(content);
            foreach (Match match in Regex.Matches(tempContent, pattern))
            {
                if (story.HasQuality(match.Groups[1].Value))
                {
                    tempContent = tempContent.Replace(match.Value, 
                        story[match.Groups[1].Value].ToString());
                }
            }

            return tempContent;
        }
    }

    private int timesVisited;
    

    private void OnEnable()
    {
        timesVisited = 0;
    }

    public bool MeetsPreconditions()
    {
        if (!Repeatable && timesVisited > 0)
            return false;

        var allMet = Preconditions.All(x => x.IsMet(story));
        return allMet;
    }

    public void PickChoice(int choiceIndex)
    {
        if (choiceIndex >= Choices.Length || choiceIndex < 0)
        {
            throw new ArgumentOutOfRangeException("choiceIndex", "choiceIndex out of bounds");
        }
        
        foreach (var postcondition in Choices[choiceIndex].Postconditions)
        {
            switch (postcondition.Type)
            {
                case PostconditionType.setTo:
                    story[postcondition.Key] = postcondition.Value;
                    break;
                    
                case PostconditionType.add:
                    story[postcondition.Key] += postcondition.Value;
                    break;
                    
                case PostconditionType.subtract:
                    story[postcondition.Key] -= postcondition.Value;
                    break;
            }
        }

        ++timesVisited;
    }

}
