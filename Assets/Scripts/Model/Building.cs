using System;

namespace Models
{
    [Serializable]
    public class Building
    {

        public string Name;

        public string Code;

        public Coordinates coordinates;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}