using System;

namespace Models
{
    [Serializable]
    public class SharedLocation
    {
        public Coordinates coordinates;
         
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}