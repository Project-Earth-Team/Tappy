using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
/*
 * Tappy
 * A tool to generate tappable loot tables
 */

namespace Tappy
{
    class Program
    {

        
        static void Main(string[] args)
        {
            /*
             * Welcome to ugly hardcoded land
             * Todo: make this not ugly hardcoded land
             * Todo: Add the functions and files to generate additional specific pools for the other tappable types
             */
            
            Console.WriteLine("===Tappable Loot Table Generation System==");
            Console.WriteLine("[Info] Reading all data files for generation");
            string[] commonIds = File.ReadAllLines(@"./common.txt");
            string[] uncommonIds = File.ReadAllLines(@"./uncommon.txt");
            string[] rareIds = File.ReadAllLines(@"./rare.txt");
            string[] epicIds = File.ReadAllLines(@"./epic.txt");
            string[] legendaryIds = File.ReadAllLines(@"./legendary.txt");
            Console.WriteLine("== Stage 1: Base common rarity itemsets [genoa:chest_tappable_map] ==");
            Console.WriteLine("[Info] Generating "+Utils.numBaseCommonItemsets+" itemsets.");
            Utils.ItemLootTable commonChestItemLootTable = new Utils.ItemLootTable();
            commonChestItemLootTable.itemsets ??= new List<Utils.Itemset>();
            commonChestItemLootTable.TableRarity = "Common";
            commonChestItemLootTable.tappableID = "genoa:chest_tappable_map";
            for (int i = 0; i<Utils.numBaseCommonItemsets; i++)
            {
                //Indices 
                int maxIndex = commonIds.Length - 1;
                Random random = new Random();
                int itemA = random.Next(maxIndex);
                int itemB = itemA;
                while (itemA == itemB)
                {
                    itemB = random.Next(maxIndex);
                }
                int itemC = itemA;
                while (itemC == itemA || itemC == itemB)
                {
                    itemC = random.Next(maxIndex);
                }
                //Create itemset
                Console.WriteLine($"[Info] Item indices: {itemA},{itemB},{itemC}");
                Utils.Itemset itemset = new Utils.Itemset();
                itemset.items ??= new Dictionary<string, int>();
                itemset.items.Add(commonIds[itemA], random.Next(1,3));
                itemset.items.Add(commonIds[itemB], random.Next(1,3));
                itemset.items.Add(commonIds[itemC], random.Next(1,3));
                commonChestItemLootTable.itemsets.Add(itemset);
                
            }
            
            //TODO: use the config file i mentioned in Utils.cs to make these not hardcoded paths that only work for me.
            
            //Serialize itemset and write to disk.
            string commonChestItemPoolSerialized = JsonConvert.SerializeObject(commonChestItemLootTable);
            var cfile = new StreamWriter(@"C:\Workspace\Programming\c\minecraft_earth\tools\common_chest.json", true);
            cfile.WriteLine(commonChestItemPoolSerialized+"\n");
            cfile.Close();
            //We now have our base for the others!
            Console.WriteLine("== Stage 2: All chest rarities [genoa:chest_tappable_map] ==");
            Console.WriteLine("[Info] Generating uncommon chest data...");
            Utils.ItemLootTable uncommonChestItemLootTable =
                Utils.genChestItemLootTable(Utils.numUncommonSets, uncommonIds, commonChestItemLootTable, "Uncommon");
            string uncommonChestItemLootTableSer = JsonConvert.SerializeObject(uncommonChestItemLootTable);
            var ufile = new StreamWriter(@"C:\Workspace\Programming\c\minecraft_earth\tools\uncommon_chest.json", true);
            ufile.WriteLine(uncommonChestItemLootTableSer+"\n");
            ufile.Close();
            Console.WriteLine("[Info] Generating rare chest data...");
            Utils.ItemLootTable rareChestItemLootTable =
                Utils.genChestItemLootTable(Utils.numRareSets, rareIds, commonChestItemLootTable, "Rare");
            string rareChestItemLootTableSer = JsonConvert.SerializeObject(uncommonChestItemLootTable);
            var rfile = new StreamWriter(@"C:\Workspace\Programming\c\minecraft_earth\tools\tables\rare_chest.json", true);
            rfile.WriteLine(uncommonChestItemLootTableSer+"\n");
            rfile.Close();
            Console.WriteLine("[Info] Generating epic chest data...");
            Utils.ItemLootTable epicChestItemLootTable =
                Utils.genChestItemLootTable(Utils.numEpicSets, epicIds, commonChestItemLootTable, "Epic");
            string epicChestItemLootTableSer = JsonConvert.SerializeObject(uncommonChestItemLootTable);
            var efile = new StreamWriter(@"C:\Workspace\Programming\c\minecraft_earth\tools\tables\epic_chest.json", true);
            efile.WriteLine(uncommonChestItemLootTableSer+"\n");
            efile.Close();
        }
    }
}