using System;
using System.Collections.Generic;
using System.Linq;

namespace Tappy
{
    public class Utils
    {
        
        /* Constants */
        //Todo: Make this a config file or smth

        public static int numBaseCommonItemsets = 30;
        public static int numUncommonSets = 30;
        public static int numRareSets = 30;
        public static int numEpicSets = 30;
        public static int numLegendarySets = 30;

        
        /// <summary>
        /// Holds the data of a particular loot table for a given tappable & rarity
        /// </summary>
        public class ItemLootTable
        {
            public List<Itemset> itemsets { get; set; }
            public string TableRarity { get; set; }
            public string tappableID { get; set; }
        }
        
        /// <summary>
        /// Holds the data of a specific result for a loot table
        /// </summary>
        public class Itemset
        {
            public Dictionary<string, int> items { get; set; }
        }

      
        /// <summary>
        /// Generates a loot table for a chest tappable that isnt common rarity, as the common rarity chest is the base for everything.
        /// </summary>
        /// <param name="numSets"></param>
        /// the amount of individual drop combos for the rarity
        /// <param name="itemIDs"></param>
        /// the IDs of all the items available to potentially drop
        /// <param name="commonChestItemLootTable"></param>
        /// the loot table for the common chest
        /// <param name="poolRarity"></param>
        /// the rarity we're generating for
        /// <returns></returns>
        public static ItemLootTable genChestItemLootTable(int numSets, string[] itemIDs, ItemLootTable commonChestItemLootTable, string poolRarity)
        {
            ItemLootTable returnedItemLootTable = new ItemLootTable();
            returnedItemLootTable.itemsets ??= new List<Itemset>();
            returnedItemLootTable.TableRarity = poolRarity;
            returnedItemLootTable.tappableID = "genoa:chest_tappable_map";
            for (int i = 0; i < numUncommonSets; i++)
            {
                int numReplacedItems = new Random().Next(1, 2);
                //Generate 2 indices regardless cus its easier
                Random random = new Random();
                int maxIndex = itemIDs.Length - 1;
                int itemA = random.Next(maxIndex);
                int itemB = itemA;
                while (itemA == itemB)
                {
                    itemB = random.Next(maxIndex);
                }
                Console.WriteLine($"[Info] Item indices: {itemA},{itemB}");
                //Get our base set. 
                Itemset baseSet = commonChestItemLootTable.itemsets[i];
                //depending  on if we're doing  one or two
                if (numReplacedItems == 1)
                {
                    var first = baseSet.items.First();
                    var last =  baseSet.items.Last();
                    baseSet.items.Clear();
                    baseSet.items.Add(first.Key, first.Value);
                    baseSet.items.Add(last.Key, last.Value);
                    baseSet.items.Add( itemIDs[itemA], random.Next(2));
                }
                else
                {
                    var first = baseSet.items.First();
                    baseSet.items.Clear();
                    baseSet.items.Add(first.Key, first.Value);
                    baseSet.items.Add( itemIDs[itemA], random.Next(2));
                    baseSet.items.Add( itemIDs[itemB], random.Next(2));
                }
                returnedItemLootTable.itemsets.Add(baseSet);
            }

            return returnedItemLootTable;
        }
    }
}