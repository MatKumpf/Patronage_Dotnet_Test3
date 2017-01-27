using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileLibrary
{
    public class FileModel
    {
        public DateTime CreationTime
        {
            get;
            set;
        }

        public string Extension
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }

        public ulong Length
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public FileModel()
        {

        }

        public override string ToString()
        {
            return "FullName: " + FullName + "\nName: " + Name + "\nExtension: " + Extension + "\nLenght: " + Length;
        }
    }
}
