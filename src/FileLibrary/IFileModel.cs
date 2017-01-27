using System;

namespace FileLibrary
{
    interface IFileModel
    {
        string Name { get; set; }
        string FullName { get; set; }
        DateTime CreationTime { get; set; }
        ulong Length { get; set; }
        string Extension { get; set; }
    }
}
