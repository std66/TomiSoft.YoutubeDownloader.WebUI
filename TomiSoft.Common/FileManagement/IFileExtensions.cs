using System.IO;

namespace TomiSoft.Common.FileManagement {
    public static class IFileExtensions {
        public static string ReadAllText(this IFile File) {
            using (TextReader tr = File.GetTextReader())
                return tr.ReadToEnd();
        }
    }
}
