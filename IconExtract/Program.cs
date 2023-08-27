namespace IconExtract {
    public class Program {

        private static readonly Configuration Configuration = new();

        public static async Task Main(string[] args) {

            try {
                new ArgumentParser(args, Configuration).Parse();
            } catch {
                Console.WriteLine("Invalid argument.\nSee https://github.com/DiscLRx/IconExtract/blob/main/README.md for help");
                return;
            }

            try {
                new EnvironmentPreparer(Configuration).PathCheck();
            } catch (InvalidPathException ex) {
                Console.WriteLine(ex.Message);
                return;
            } catch {
                Console.WriteLine("Invalid path");
                return;
            }

            Console.WriteLine($"Input: {Configuration.InputPath}");
            Console.WriteLine($"Output: {Configuration.OutputPath}");

            int extractCount;
            try {
                extractCount = await new ExtractHandler(Configuration).Extract();
            } catch (Exception ex) {
                Console.WriteLine("Failed to extract icon");
                Console.WriteLine(ex);
                return;
            }

            if (extractCount == 0) {
                Console.WriteLine("No icons were extracted");
            } else {
                var iconQuantifier = extractCount == 1 ? "icon was" : "icons were";
                Console.WriteLine($"{extractCount} {iconQuantifier} extracted");
            }

        }

    }

    public class Configuration {

        public string InputPath { get; set; } = "";

        public string OutputPath { get; set; } = "icon";

        public bool Compress { get; set; } = false;

        public string Format { get; set; } = "png";

        public string TemporaryOutputDirectory { get; set; } = "tmp_output_dir";

    }
}
