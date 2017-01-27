using System;
using NUnit.Framework;
using System.IO;
using FileLibrary;

namespace UnitTest
{
    [TestFixture]
    public class FileServiceTests
    {
        [Test]
        public void IsValidGetFiles_TooLongFilePath_ReturnException()
        {
            string filePath = @"C:\ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka.doc";
            var ex = Assert.Throws<PathTooLongException>(() => FileService.GetFiles(filePath));
            StringAssert.Contains("Path is longer than the system-defined maximum length", ex.Message);
        }

        [Test]
        public void IsValidGetFiles_PathDoesNotExist_ReturnException()
        {
            var hash = System.Security.Cryptography.MD5.Create().ComputeHash(new System.Text.UTF8Encoding().GetBytes("1234"));
            var path = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\" + BitConverter.ToString(hash).Replace(" -", string.Empty).ToLower());
            var ex = Assert.Throws<DirectoryNotFoundException>(() => FileService.GetFiles(path));
            StringAssert.Contains("Directory does not exist", ex.Message);
        }

        [Test]
        public void IsValidGetFiles_DirectoryAccessDenied_ReturnException()
        {
            var path = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\System Volume Information\");
            var ex = Assert.Throws<UnauthorizedAccessException>(() => FileService.GetFiles(path));
            StringAssert.Contains("Access to directory is denied", ex.Message);
        }

        [Test]
        public void IsValidFileMetadata_FileDoesNotExist_ReturnException()
        {
            var hash = System.Security.Cryptography.MD5.Create().ComputeHash(new System.Text.UTF8Encoding().GetBytes("1234"));
            var path = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\" + BitConverter.ToString(hash).Replace(" -", string.Empty).ToLower() + ".doc");
            var ex = Assert.Throws<FileNotFoundException>(() => FileService.FileMetadata(path));
            StringAssert.Contains("File does not exist", ex.Message);
        }

        //[Test]
        [Ignore("Nie do końca elegancko wykonany test.")]
        public void IsValidFileMetadata_FileAccessDenied_ReturnException()
        {
            // Dla testu utworzono nowego użytkownika oraz plik txt na pulpicie, a następnie z innego konta próbowano otrzymać informacje o tym pliku
            // Jest to według mnie nie do końca eleganckie rozwiązanie dlatego tymczasowo został on wyłączony
            // Mam problem z nadawaniem uprawnień dla plików i folderów w .NET Core ponieważ znane mi metody Directory.SetAccessControl oraz File.SetAccessControl w nim nie występują
            var path = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Users\Nowy\Desktop\Test.txt");
            var ex = Assert.Throws<UnauthorizedAccessException>(() => FileService.FileMetadata(path));
            StringAssert.Contains("Access to file is denied", ex.Message);
        }
    }
}
