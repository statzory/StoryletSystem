using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class StringIntDictionary : SerializableDictionary<string, int> {}

[CreateAssetMenu(fileName = "New StoryState", menuName = "Story Story", order = 51)]
public class StoryState : ScriptableObject
{
   [SerializeField]
   private StringIntDictionary qualities;

   public delegate void QualityUpdatedHandler(string qualityName, int newValue);
   
   private class QualityObserver
   {
      public QualityObserver(string quality, QualityUpdatedHandler handler)
      {
         Quality = quality;
         Handler = handler;
      }
      
      public string Quality;
      public QualityUpdatedHandler Handler;
   }

   private List<QualityObserver> observers;

   private void OnEnable()
   {
      observers = new List<QualityObserver>();
   }

   public int this[string key]
   {
      get
      {
         if (!qualities.ContainsKey(key))
         {
            Debug.LogError("Cannot get quality because quality doesn't exist");
         }

         return qualities[key];
      }

      set
      {
         if (!qualities.ContainsKey(key))
         {
            Debug.LogError("Cannot set quality because quality doesn't exist");
         }

         if (value != qualities[key])
         {
            foreach (var observer in observers)
            {
               if (string.CompareOrdinal(observer.Quality, key) == 0)
               {
                  observer.Handler.Invoke(key, value);
               }
            }
         }

         qualities[key] = value;
      }
   }

   public bool HasQuality(string quality)
   {
      return qualities.ContainsKey(quality);
   }

   public void SubscribeToQuality(string quality, QualityUpdatedHandler handler)
   {
      observers.Add(new QualityObserver(quality, handler));
   }

   public void UnsubscribeFromQuality(QualityUpdatedHandler handler)
   {
      observers = observers.Except(observers.Where(x => x.Handler == handler)).ToList();
   }
}
