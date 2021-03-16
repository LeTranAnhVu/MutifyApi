using System.IO;

namespace Mutify
{
    public class Constants
    {
        public static readonly string RootPath = Directory.GetCurrentDirectory();
        public static class Resource
        {
            public static readonly string ResourceFolder = "Resources";
            public static readonly string AudioFolder = "Audio";
            public static readonly string VideoFolder = "Video";
        }
    }
}