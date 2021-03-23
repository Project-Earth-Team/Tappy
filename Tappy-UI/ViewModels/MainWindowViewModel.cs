using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
namespace Tappy_UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public void ButtonClicked() {
            Random random = new Random();
            Console.WriteLine("Tappy | Tappable Loot Table Generation System");

            /* Load Data Files */
            Console.WriteLine("Loading data files");
            Directory.CreateDirectory("./inputdata");
            string[] files = Directory.GetFiles("./inputdata", "*.json");
            Directory.CreateDirectory("./outputdata");
            foreach (string filePath in files)
            {
                Utils.TableData tableData = JsonConvert.DeserializeObject<Utils.TableData>(File.ReadAllText(filePath));
                Console.WriteLine("Processing Tappable Data for tappable type " + tableData.tappableID);
                Utils.TappableLootTable resultLootTable = new Utils.TappableLootTable();
                resultLootTable.possibleDropSets ??= new List<List<string>>();
                resultLootTable.tappableID = tableData.tappableID;
                //we generate 100 dropTables (no, not Bobby Tables drop tables)
                for (int i = 0; i < 100; i++)
                {
                    List<string> rewards = new List<string>();
                    //how many do we want
                    int numRewards = random.Next(tableData.minItemsPerTap, tableData.maxItemsPerTap);
                    Console.WriteLine("Set " + i + " Will have " + numRewards + " rewards.");
                    for (int i2 = 0; i2 < numRewards; i2++)
                    {
                        while (true)
                        {
                            //we get a random item from the table
                            int randIndex = random.Next(0, tableData.percentages.Count);
                            var targetItem = tableData.percentages.ElementAt(randIndex);
                            //try and add it based on percentage
                            int roll = random.Next(0, 100);
                            Console.WriteLine("Got role of " + roll + " for item with percentage " + targetItem.Value + " Target item: " + targetItem.Key);
                            if (targetItem.Value <= roll || targetItem.Value == 100)
                            {
                                Console.WriteLine("Added item to rewards");
                                rewards.Add(targetItem.Key);
                                break;
                            }
                        }
                    }
                    //Add our rewards to the main loot table
                    resultLootTable.possibleDropSets.Add(rewards);
                }
                //Now we save it
                string table = JsonConvert.SerializeObject(resultLootTable);
                // Console.WriteLine(table);
                File.WriteAllText("./outputdata/" + resultLootTable.tappableID.Split(":")[1] + ".json", table);
            }
            Console.WriteLine("Done! ");
        }
    }
}
