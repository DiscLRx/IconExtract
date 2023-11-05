using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace IconExtract {
    public class ImageConvert {

        private static IImage Load(string file) {
            return PlatformImage.FromStream(File.Open(file, FileMode.Open));
        }

        public static async Task SaveAsPng(string file) {
            using var stream = File.Open($"{file}.png", FileMode.OpenOrCreate);
            await Load(file).SaveAsync(stream, ImageFormat.Png);
        }

    }
}
