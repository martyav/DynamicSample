using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;

namespace DynamicSample
{
    public enum StringSearchOption
    {
        StartsWith,
        Contains,
        EndsWith
    }

    class ReadOnlyFile : DynamicObject
    {
        private string _filePath;

        public ReadOnlyFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File path does not exist.");
            }

            _filePath = filePath;
        }
    }
}
