using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileLibrary
{
    public class DataTreeModel : IDataTree
    {

        public DataTreeModel(string dataWithIndent, FileModel fileModel)
        {
            NameWithIndent = dataWithIndent;
            CreationTime = fileModel.CreationTime;
            Extension = fileModel.Extension;
            FullName = fileModel.FullName;
            Length = fileModel.Length;
            Name = fileModel.Name;
        }

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
