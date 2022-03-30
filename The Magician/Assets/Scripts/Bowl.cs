using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class Bowl : MonoBehaviour
    {
        [SerializeField] UnityEvent onComplete; 
        public void AddedPebble()
        {
            // Might call this in a different function but testing for now
            onComplete.Invoke();            
        }
    }
}
