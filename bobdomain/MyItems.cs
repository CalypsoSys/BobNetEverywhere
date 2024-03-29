﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bobdomain
{
    public static class MyItems
    {
        private const int _items = 5;
        public static object GetMyItems(string dataPath, int id)
        {
            string[] source = new string[] { "One", "Two", "Three", "Four", "Five", };
            List<object> items = new List<object>();
            for (int i = 0; i <= _items; i++)
            {
                items.Add(new
                {
                    Name = string.Format("{0} - {1}", id, i),
                    Source = (i < source.Length ? source[i] : "Undefined"),
                    Type = string.Format("Bob-{0}", i*(id+i)),
                    Created = DateTime.Now.AddDays(i),
                    Status = (i % 2 == 0 ? "ok" : "failed")
                });
            }

            try
            {
                string itemPath = Path.Combine(dataPath, "data", "saved_items.txt");
                using (StreamReader input = new StreamReader(itemPath))
                {
                    string line;
                    int index = _items + 1;
                    while( (line = input.ReadLine()) != null )
                    {
                        var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        items.Add(new
                        {
                            Name = string.Format("{0} - {1} - {2}", parts[0], parts[1], parts[2]),
                            Source = "User Entered",
                            Type = string.Format("Bob-{0}", index),
                            Created = DateTime.Now,
                            Status = "ok"
                        });

                        ++index;
                    }
                }
            }
            catch
            {

            }

            return items;
        }

        public static bool SaveItem(string dataPath, int id, string itemOne, string itemTwo)
        {
            try
            {
                string dataFolder = Path.Combine(dataPath, "data");
                Directory.CreateDirectory(dataFolder);
                string itemPath = Path.Combine(dataFolder, "saved_items.txt");
                using ( StreamWriter output = new StreamWriter(itemPath, true))
                {
                    output.WriteLine("{0},{1},{2}", id, itemOne, itemTwo);
                }

                return true;
            }
            catch(Exception excp)
            {
                return false;
            }

        }
    }
}