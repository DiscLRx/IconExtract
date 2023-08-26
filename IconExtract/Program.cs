using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace IconExtract {
    public class Program {

        private static readonly List<Bitmap> BitmapList = new();

        private static string InputPath = "";

        private static string OutputPath = "output";

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int PrivateExtractIcons(
            string lpszFile, int nIconIndex, int cxIcon, int cyIcon, nint[]? phicon, int[]? piconid, int nIcons, int flags);

        private static void ParseArguments(string[] args) {

            if (args.Length == 0) {
                throw new ArgumentException("Invalid argument");
            }

            if (args.Length == 1) {
                InputPath = args[0];
            } else {
                var argsList = args.ToList();
                var inputFlagIndex = argsList.IndexOf("-i");
                if (inputFlagIndex == -1) {
                    inputFlagIndex = argsList.IndexOf("--input");
                }
                if (inputFlagIndex != -1) {
                    InputPath = argsList[inputFlagIndex + 1];
                } else {
                    throw new ArgumentException("Invalid argument");
                }

                OutputPath = "output";
                var outputFlagIndex = argsList.IndexOf("-o");
                if (outputFlagIndex == -1) {
                    outputFlagIndex = argsList.IndexOf("--output");
                }
                if (outputFlagIndex != -1) {
                    OutputPath = argsList[outputFlagIndex + 1];
                }
            }
        }

        private static bool PathCheck(ref string errMsg) {

            if (!File.Exists(InputPath)) {
                errMsg = $"'{InputPath}' is not a valid file";
                return false;
            }

            if (!Directory.Exists(OutputPath)) {
                try {
                    Directory.CreateDirectory(OutputPath);
                } catch {
                    errMsg = $"'{OutputPath}' is not a valid path";
                    return false;
                }
            }

            return true;
        }

        private static int GetIcons(string path) {
            var iconCount = PrivateExtractIcons(path, 0, 0, 0, null, null, 0, 0);
            if (iconCount == 0) {
                return 0;
            }

            var hIcons = new nint[iconCount];
            int[] ids = new int[iconCount];
            int extractCount = PrivateExtractIcons(path, 0, 256, 256, hIcons, ids, iconCount, 0);

            foreach (var hIcon in hIcons) {
                var icon = Icon.FromHandle(hIcon);
                var bitmap = icon.ToBitmap();
                BitmapList.Add(bitmap);
            }

            return extractCount;
        }

        public static void Main(string[] args) {

            try {
                ParseArguments(args);
            } catch {
                Console.WriteLine("Invalid argument.\nSee https://github.com/DiscLRx/IconExtract/blob/main/README.md for help");
                return;
            }

            string errMsg = "";
            if (!PathCheck(ref errMsg) || !PathCheck(ref errMsg)) {
                Console.WriteLine(errMsg);
                return;
            }

            Console.WriteLine($"Input: {InputPath}");
            Console.WriteLine($"Output: {OutputPath}");

            int extractCount;
            try {
                extractCount = GetIcons(InputPath);
            } catch {
                Console.WriteLine("Failed to extract icon");
                return;
            }

            if (extractCount == 0) {
                Console.WriteLine("No icons were extracted");
                return;
            }

            int fileIndex = 1;
            BitmapList.ForEach(bm => {
                bm.Save($"{Path.Combine(OutputPath, fileIndex.ToString())}.png", ImageFormat.Png);
                fileIndex++;
            });

            var iconQuantifier = extractCount == 1 ? "icon was" : "icons were";
            Console.WriteLine($"{extractCount} {iconQuantifier} extracted");

        }
    }
}
