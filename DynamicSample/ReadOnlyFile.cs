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

        #region overrides

        // These two both do the same thing -- check if the member exists
        //
        // The first is run if you try to call a member without specifying any arguments
        // Its sibling is called if you try to call a member while specifying arguments
        //
        // The binder is how we pass them information about the member
        //
        // The member's name is the string representation of the name of the member
        //
        // For example, in file.Customer, we'd get a string containing "Customer" when we call binder.Name

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetPropertyValue(binder.Name);
            return result == null ? false : true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder,
                                     object[] args,
                                     out object result)
        {
            StringSearchOption StringSearchOption = StringSearchOption.StartsWith;
            bool trimSpaces = true;

            try
            {
                if (args.Length > 0) { StringSearchOption = (StringSearchOption)args[0]; }
            }
            catch
            {
                throw new ArgumentException("StringSearchOption argument must be a StringSearchOption enum value.");
            }

            try
            {
                if (args.Length > 1) { trimSpaces = (bool)args[1]; }
            }
            catch
            {
                throw new ArgumentException("trimSpaces argument must be a Boolean value.");
            }

            result = GetPropertyValue(binder.Name, StringSearchOption, trimSpaces);

            return result == null ? false : true;
        }
        #endregion

        #region meaty custom method

        public List<string> GetPropertyValue(string propertyName,
                                     StringSearchOption StringSearchOption = StringSearchOption.StartsWith,
                                     bool trimSpaces = true)
        {
            StreamReader sr = null;
            List<string> results = new List<string>();
            string line = "";
            string testLine = "";

            try
            {
                sr = new StreamReader(_filePath);

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();

                    // Perform a case-insensitive search by using the specified search options.
                    testLine = line.ToUpper();
                    if (trimSpaces) { testLine = testLine.Trim(); }

                    switch (StringSearchOption)
                    {
                        case StringSearchOption.StartsWith:
                            if (testLine.StartsWith(propertyName.ToUpper())) { results.Add(line); }
                            break;
                        case StringSearchOption.Contains:
                            if (testLine.Contains(propertyName.ToUpper())) { results.Add(line); }
                            break;
                        case StringSearchOption.EndsWith:
                            if (testLine.EndsWith(propertyName.ToUpper())) { results.Add(line); }
                            break;
                    }
                }
            }
            catch
            {
                // Trap any exception that occurs in reading the file and return null.
                results = null;
            }
            finally
            {
                if (sr != null) { sr.Close(); }
            }

            return results;
        }
        #endregion
    }
}
