namespace IconExtract {
    public class EnvironmentPreparer {

        private readonly Configuration _configuration;

        public EnvironmentPreparer(Configuration configuration) {
            _configuration = configuration;
        }

        public void PathCheck() {
            TmpPathCheck();
            InputPathCheck();
            OutputPathCheck();
        }

        private void TmpPathCheck() {
            if (Directory.Exists(_configuration.TemporaryOutputDirectory)) {
                _configuration.TemporaryOutputDirectory += new Random().NextInt64(10000, 100000);
            }
        }

        private void InputPathCheck() {
            if (!File.Exists(_configuration.InputPath)) {
                throw new InvalidPathException($"'{_configuration.InputPath}' is not a valid file");

            }
        }

        private void OutputPathCheck() {
            try {
                if (_configuration.Compress) {
                    OutputFileCheck();
                } else {
                    OutputDirectoryCheck();
                }
            } catch {
                throw new InvalidPathException($"'{_configuration.OutputPath}' is not a valid path");
            }
        }

        private void OutputFileCheck() {
            if (File.Exists(_configuration.OutputPath)) {
                File.Delete(_configuration.OutputPath);
            }
            File.Create(_configuration.OutputPath).Close();
            File.Delete(_configuration.OutputPath);
        }

        private void OutputDirectoryCheck() {
            if (Directory.Exists(_configuration.OutputPath)) {
                Directory.Delete(_configuration.OutputPath, true);
            }
            Directory.CreateDirectory(_configuration.OutputPath);
            Directory.Delete(_configuration.OutputPath, true);
        }

    }

    public class InvalidPathException : Exception {

        public InvalidPathException() { }

        public InvalidPathException(string message) : base(message) { }

    }

}
