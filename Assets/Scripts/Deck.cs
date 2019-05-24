using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck", order = 51)]
public class Deck : ScriptableObject
{
   public List<Storylet> Storylets;

   public Storylet DrawStorylet()
   {
      var validStorylets = Storylets.Where(x => x.MeetsPreconditions()).ToArray();
      var weights = validStorylets.Select(x => x.Weight).ToArray();
      var selectedIndex = RandomUtility.WeightedRandom(weights);

      return validStorylets[selectedIndex];
   }
}
