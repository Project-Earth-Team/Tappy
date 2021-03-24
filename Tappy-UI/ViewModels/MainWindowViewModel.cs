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

        private string _runNumStr = "100";
        public string runNumString
        {
            get => _runNumStr;
            set
            {
                if (value != null && value.Length > 0 && value[0] == '#' && value.Length <= 5)
                {
                    _runNumStr = value;
                }

            }
        }

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
                for (int i = 0; i < Int64.Parse(_runNumStr); i++)
                {
                    List<string> rewards = new List<string>();
                    //how many do we want
                    int numRewards = random.Next(tableData.minItemsPerTap, tableData.maxItemsPerTap);
                    Console.WriteLine("[Info] Set " + i + " Will have " + numRewards + " rewards.");
                    int sum = 0;
                    foreach (var itemPercentage in tableData.percentages)
                    {
                        sum += itemPercentage.Value;
                    }
                    for (int i2 = 0; i2 <= numRewards; i2++)
                    {
                        int randomRoll = random.Next(0, sum);
                            foreach (var item in tableData.percentages)
                            {
                                if (randomRoll < item.Value)
                                {
                                    rewards.Add(item.Key);
                                    break;
                                }

                                randomRoll -= item.Value;
                            }
                        }
                    Console.WriteLine("[Info] End set " + i+" generation");
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
