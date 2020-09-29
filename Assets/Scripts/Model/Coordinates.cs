using System;

namespace Models
{
    [Serializable]
    public class Coordinates
    {

        public string Latitude;

        public string Longitude;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}