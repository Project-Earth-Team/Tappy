using System;
using System.Collections.Generic;
using System.Linq;

namespace Tappy
{
    public class Utils{
        public class TableData
        {
            public string tappableID { get; set; }
            public int maxItemsPerTap { get; set; }
            public int minItemsPerTap { get; set; }
            public Dictionary<string, int> percentages { get; set; }
        }

        public class TappableLootTable
        {
            public string tappableID { get; set; }
            public List<List<string>> possibleDropSets { get; set; }
        }
    }
}