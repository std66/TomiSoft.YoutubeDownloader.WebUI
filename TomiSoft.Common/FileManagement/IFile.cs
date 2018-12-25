﻿namespace TomiSoft.Common.FileManagement {
    public interface IFile {
        bool Exists { get; }
        string DirectoryName { get; }
        bool Delete();
        IFile Rename(string NewName);
    }
}
