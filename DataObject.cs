using System;
using System.Collections.Generic;
using System.Text;

namespace HurtowniaBazDanych
{

    public abstract class DataObject
    {
        private static Dictionary<string, int> idTracker = new Dictionary<string, int>();
        public int id { get; set; }
        public string signature { get; set; }

        public DataObject(string signature)
        {
            this.signature = signature;

            if (!idTracker.ContainsKey(signature))
            {
                idTracker.Add(signature, 0);
            }

            id = idTracker[signature];
            idTracker[signature]++;
        }

        public abstract String ToString(string spacer = "");
    }
}