using System.IO;

namespace Mutify.Helpers
{
    public static class PathHelper
    {
        public static string GetResourcePath()
        {
            var root = Constants.RootPath;
            return Path.Combine(root, Constants.Resource.ResourceFolder);
        }

        public static string GetAudioPath()
        {
            var root = Constants.RootPath;
            var resourcePath = Path.Combine(root, Constants.Resource.ResourceFolder);
            return Path.Combine(resourcePath, Constants.Resource.AudioFolder);
        }

        public static string GetVideoPath()
        {
            var root = Constants.RootPath;
            var resourcePath = Path.Combine(root, Constants.Resource.ResourceFolder);
            return Path.Combine(resourcePath, Constants.Resource.VideoFolder);
        }
    }
}