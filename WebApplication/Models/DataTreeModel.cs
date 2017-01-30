using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileLibrary;

namespace WebApplication.Models
{
    public class DataTreeModel : IDataTree
    {
        public DateTime CreationTime
        {
            get;
            set;
        }

        public string NameWithIndent
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

        public ulong? Length
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
