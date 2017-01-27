using System;
using NUnit.Framework;
using System.IO;
using System.Runtime.InteropServices;
using FileLibrary;

namespace UnitTest
{
    [TestFixture]
    public class TreeTests
    {
        [Test]
        public void IsValidConstructor_NotParameter_ReturnException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Tree(new string[] { }));
            StringAssert.Contains("Path cannot be null", ex.Message);
        }

        [Test]
        public void IsValidConstructor_MoreThanOneParameter_ReturnException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Tree(new string[] { "", "" }));
            StringAssert.Contains("There cannot be more than one parameter", ex.Message);
        }

        [Test]
        public void IsValidConstructor_PathDoesNotExist_ReturnException()
        {
            var hash = System.Security.Cryptography.MD5.Create().ComputeHash(new System.Text.UTF8Encoding().GetBytes("1234"));
            var path = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\" + BitConverter.ToString(hash).Replace(" -", string.Empty).ToLower());
            var ex = Assert.Throws<DirectoryNotFoundException>(() => new Tree(new string[] { path }));
            StringAssert.Contains("Directory does not exist", ex.Message);
        }

        [Test]
        public void IsValidConstructor_DirectoryAccessDenied_ReturnException()
        {
            var path = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\System Volume Information\");
            var ex = Assert.Throws<UnauthorizedAccessException>(() => new Tree(new string[] { path }));
            StringAssert.Contains("Access to directory is denied", ex.Message);
        }

        [Test]
        public void IsValidConstructor_BadFormatPath_ReturnException()
        {
            string wrongPath = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                wrongPath = @"/1234/";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                wrongPath = @"D:\1234\";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                wrongPath = @"\1234:\";
            }

            var wrongEx = Assert.Throws<FormatException>(() => new Tree(new string[] { wrongPath }));

            StringAssert.Contains("Incorrect path format", wrongEx.Message);
        }

        [Test]
        public void IsValidConstructor_CorrectFormatPath_ReturnNotNullObject()
        {
            string correctPath = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                correctPath = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Program Files\");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                correctPath = @"/home/";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                correctPath = @"/home/";
            }

            var tree = new Tree(new string[] { correctPath });

            Assert.NotNull(tree);
        }


        [Test]
        public void IsValidConstructor_TooLongPath_ReturnException()
        {
            string path = @"C:\ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka_ProbnaDlugaSciezka";
            var ex = Assert.Throws<PathTooLongException>(() => new Tree(new string[] { path }));
            StringAssert.Contains("Path is longer than the system-defined maximum length", ex.Message);
        }
    }
}
