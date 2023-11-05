namespace IconExtract {
    public class ExtractHandler {

        private readonly Configuration _configuration;
        private string? _iconDirectory;

        public ExtractHandler(Configuration configuration) {
            _configuration = configuration;
        }

        public async Task<int> Extract() {
            int extractCount;

            await DecompressFile();
            await ConvertFormat();
            if (_configuration.Compress) {
                extractCount = await OutputToCompressedFile();
            } else {
                extractCount = await OutputToDirectory();
            }

            return extractCount;
        }

        private async Task DecompressFile() {
            await new CompressHelper().Decompress(_configuration.InputPath, _configuration.TemporaryOutputDirectory);
        }

        private async Task ConvertFormat() {
            GetIconDir(Path.Combine(_configuration.TemporaryOutputDirectory, ".rsrc"));
            var files = Directory.GetFiles(_iconDirectory);
            foreach (var file in files) {
                var fileNameWithouExtension = Path.GetFileNameWithoutExtension(file);
                var newPath = Path.Combine(_iconDirectory, fileNameWithouExtension);
                File.Move(file, newPath);
                await DoConvert(newPath);
                File.Delete(newPath);
            }
        }

        private async Task DoConvert(string file) {
            switch (_configuration.Format) {
                case "png":
                    await ImageConvert.SaveAsPng(file);
                    break;
                case "ico":
                    File.Move(file, $"{file}.ico");
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private async Task<int> OutputToDirectory() {
            return await Task.Run(() => {
                Directory.Move(_iconDirectory, _configuration.OutputPath);
                Directory.Delete(_configuration.TemporaryOutputDirectory, true);
                return Directory.GetFiles(_configuration.OutputPath).Length;
            });
        }

        private void GetIconDir(string dirPath) {
            var dirs = Directory.GetDirectories(dirPath);
            foreach (var dir in dirs) {
                var info = new DirectoryInfo(dir);
                if ("ICON".Equals(info.Name)) {
                    _iconDirectory = info.FullName;
                    return;
                } else {
                    GetIconDir(dir);
                }
            }
        }

        private async Task<int> OutputToCompressedFile() {
            var programDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.Combine(_configuration.TemporaryOutputDirectory, ".rsrc", "ICON"));
            var icons = Directory.GetFiles(".");
            await Task.Run(() => { new CompressHelper().Compress(_configuration.OutputPath, icons); });

            var files = Directory.GetFiles(".");
            var newFile = files.Except(icons).ToArray();
            Directory.SetCurrentDirectory(programDirectory);

            if (newFile.Length > 0) {
                File.Move(Path.Combine(_configuration.TemporaryOutputDirectory, ".rsrc", "ICON", newFile[0]), _configuration.OutputPath);
            }
            Directory.Delete(_configuration.TemporaryOutputDirectory, true);
            return icons.Length;
        }

    }
}
