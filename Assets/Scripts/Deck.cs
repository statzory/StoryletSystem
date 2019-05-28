using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck", order = 51)]
public class Deck : ScriptableObject
{
   [SerializeField] private List<Storylet> storylets;
   [SerializeField] private List<float> weights;

   private List<Storylet> _validStorylets;
   private List<float> _validWeights;

   public List<Storylet> Storylets => storylets;
   public List<float> Weights => weights;

   public List<Storylet> ValidStorylets
   {
      get
      {
         UpdateValidStorylets();

         return _validStorylets;
      }
   }
   public List<float> ValidWeights
   {
      get
      {
         UpdateValidStorylets();

         return _validWeights;
      }
   }

   public Storylet DrawStorylet()
   {
      var selectedIndex = RandomUtility.WeightedRandom(ValidWeights.ToArray());
      var storylet = ValidStorylets[selectedIndex];
      return storylet;
   }

   private void OnEnable()
   {
      _validStorylets = new List<Storylet>();
      _validWeights = new List<float>();
   }

   private void UpdateValidStorylets()
   {
      _validStorylets.Clear();
      _validWeights.Clear();
      
      for (var i = 0; i < storylets.Count; ++i)
      {
         if (storylets[i].MeetsPreconditions())
         {
            _validStorylets.Add(storylets[i]);
            _validWeights.Add(weights[i]);
         }
      }
   }
}
