/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data
{
    sealed class SkyrimPluginListProvider : IPluginListProvider
    {
        readonly List<PluginListEntry> plugins = new List<PluginListEntry>();
        public IEnumerable<PluginListEntry> Plugins { get { return plugins; } }

        public SkyrimPluginListProvider(IDataFileProvider dataFileProvider, string defaultPluginFilePath)
        {
            using (var reader = new StreamReader(dataFileProvider.GetPluginListFile(defaultPluginFilePath).Open()))
            {
                string line;
                /*plugins.Add(new PluginListEntry()
                {
                    Filename = "Skyrim.esm"
                });
                plugins.Add(new PluginListEntry()
                {
                    Filename = "Update.esm"
                });
                plugins.Add(new PluginListEntry()
                {
                    Filename = "Dawnguard.esm"
                });
                plugins.Add(new PluginListEntry()
                {
                    Filename = "Hearthfires.esm"
                });
                plugins.Add(new PluginListEntry()
                {
                    Filename = "Dragonborn.esm"
                });*/
                var list = new List<string>();
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("#") || line.Length == 0)
                        continue;

                    var fileLine = line;
                    if (fileLine.StartsWith("*") && fileLine.Length > 1)
                    {
                        fileLine = fileLine.Substring(1);
                    }
                    list.Add(fileLine);
                }
                if (list.Contains("Skyrim.esm")) list.Remove("Skyrim.esm");
                if (list.Contains("Update.esm")) list.Remove("Update.esm");
                if (list.Contains("Dawnguard.esm")) list.Remove("Dawnguard.esm");
                if (list.Contains("Hearthfires.esm")) list.Remove("Hearthfires.esm");
                if (list.Contains("Dragonborn.esm")) list.Remove("Dragonborn.esm");
                list.Insert(0, "Skyrim.esm");
                list.Insert(1, "Update.esm");
                list.Insert(2, "Dawnguard.esm");
                list.Insert(3, "Hearthfires.esm");
                list.Insert(4, "Dragonborn.esm");
                foreach(var fileLine in list)
                {
                    plugins.Add(new PluginListEntry()
                    {
                        Filename = fileLine
                    });
                }
            }
        }
    }
}
