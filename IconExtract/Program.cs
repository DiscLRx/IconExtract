using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace IconExtract {
        public class Program {

        private static List<Bitmap> bitmapList { get; } = new();

        [DllImport("User32.dll")]
        private static extern int PrivateExtractIcons(
            string lpszFile, int nIconIndex, int cxIcon, int cyIcon, IntPtr[] phicon, int[] piconid, int nIcons, int flags);

        private static void GetIcons(string path) {
            var iconCount = PrivateExtractIcons(path, 0, 0, 0, null, null, 0, 0);
            IntPtr[]? hIcons = new IntPtr[iconCount];
            int[] ids = new int[iconCount];
            PrivateExtractIcons(path, 0, 256, 256, hIcons, ids, iconCount, 0);

            foreach (var hIcon in hIcons) {
                var icon = Icon.FromHandle(hIcon);
                var bitmap = icon.ToBitmap();
                bitmapList.Add(bitmap);
            }

        }

        public static void Main(string[] args) {

            if (args.Length == 0) {
                return;
            }

            var argsList = args.ToList();
            var inputFlagIndex = argsList.IndexOf("-i");
            var inputPath = argsList[inputFlagIndex + 1];

            var outputPath = "output";
            var outputFlagIndex = argsList.IndexOf("-o");
            if (outputFlagIndex != -1) {
                outputPath = argsList[outputFlagIndex + 1];
            }

            GetIcons(inputPath);

            if (!Directory.Exists(outputPath)) {
                Directory.CreateDirectory(outputPath);
            }

            int fileIndex = 1;
            bitmapList.ForEach(bm => {
                bm.Save($"{Path.Combine(outputPath, fileIndex.ToString())}.png", ImageFormat.Png);
                fileIndex++;
            });

        }

    }
}
