﻿//
// Copyright (c) Microsoft.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
namespace Microsoft.WindowsAzure.Build.Tasks
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A simple Microsoft Build task used to generate a list of test assemblies to be
    /// used for testing Azure PowerShell.
    /// </summary>
    public class FilterTask : Task
    {
        /// <summary>
        /// Gets or sets the files changed in a given pull request.
        /// </summary>
        [Required]
        public string[] FilesChanged { get; set; }

        /// <summary>
        ///  Gets or sets the path to the files-to-test-assemblies map.
        /// </summary>
        [Required]
        public string MapFilePath { get; set; }

        /// <summary>
        /// Gets or set the TargetModule, e.g. Storage
        /// </summary>
        public string TargetModule { get; set; } 
        
        /// <summary>
        /// Gets or sets the test assemblies output produced by the task.
        /// </summary>
        [Output]
        public string[] Output { get; set; }

        /// <summary>
        /// Executes the task to generate a list of test assemblies
        /// based on file changes from a specified Pull Request.
        /// The output it produces is said list.
        /// </summary>
        /// <returns> Returns a value indicating wheter the success status of the task. </returns>
        public override bool Execute()
        {
            if (MapFilePath == null)
            {
                throw new ArgumentNullException("The MapFilePath cannot be null.");
            }

            if (!File.Exists(MapFilePath))
            {
                throw new FileNotFoundException("The MapFilePath provided could not be found. Please provide a valid MapFilePath.");
            }

            var mappingsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText(MapFilePath));

            if (FilesChanged != null && FilesChanged.Length > 0)
            {
                Console.WriteLine($"Filter according to {FilesChanged.Length} file(s) in FilesChanged");
                var filesChangedSet = new HashSet<string>(FilesChanged);
                Output = SetGenerator.Generate(filesChangedSet, mappingsDictionary).ToArray();
            }
            else if(!string.IsNullOrWhiteSpace(TargetModule))
            {
                Console.WriteLine($"Filter module {TargetModule}");
                var modules = (TargetModule.Equals("Accounts"))? new string[] { "Accounts"} : new string[] { "Accounts" , TargetModule};
                Output = SetGenerator.Generate(modules, mappingsDictionary).ToArray();
            }
            else
            {
                Console.WriteLine($"Skip filter and load all from ${MapFilePath}");
                var set = new HashSet<string>();
                mappingsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText(MapFilePath));
                foreach (KeyValuePair<string, string[]> pair in mappingsDictionary)
                {
                    set.UnionWith(pair.Value);
                }

                Output = set.ToArray();
            }

            return true;
        }
    }
}
