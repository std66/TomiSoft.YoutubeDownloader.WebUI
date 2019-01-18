using TomiSoft.Common.FileManagement.Permissions;

namespace TomiSoft.Common.FileManagement {
    public interface IFileManager {
        IFile GetFile(string Path);
        bool TryCreateFile(string Path, out IFile Result);
        bool TryCreateTextFile(string Path, string Content, FileCreationPermission CreationPermission, out IFile Result);
    }
}
