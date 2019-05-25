using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck", order = 51)]
public class Deck : ScriptableObject
{
   public List<Storylet> Storylets;
   
   public List<Storylet> ValidStorylets => Storylets.Where(x => x.MeetsPreconditions()).ToList();

   public Storylet DrawStorylet()
   {
      var weights = ValidStorylets.Select(x => x.Weight).ToArray();
      var selectedIndex = RandomUtility.WeightedRandom(weights);

      return ValidStorylets[selectedIndex];
   }
}
