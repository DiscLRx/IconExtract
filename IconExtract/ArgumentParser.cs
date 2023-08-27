namespace IconExtract {
    public class ArgumentParser {

        private List<string> _arguments;

        private readonly Configuration _configuration;

        public ArgumentParser(string[] args, Configuration configuration) {
            _arguments = args.ToList();
            _configuration = configuration;
        }

        public void Parse() {

            if (_arguments.Count <= 1) {
                throw new ArgumentException("Invalid argument");
            }

            ParseInput();
            ParseOutput();
            ParseCompress();
            ParseFormat();
        }

        private void ParseInput() {
            var inputFlagIndex = _arguments.IndexOf("-i");
            if (inputFlagIndex == -1) {
                inputFlagIndex = _arguments.IndexOf("--input");
            }
            if (inputFlagIndex != -1) {
                _configuration.InputPath = _arguments[inputFlagIndex + 1];
            } else {
                throw new ArgumentException("Invalid argument");
            }
        }

        private void ParseOutput() {
            var outputFlagIndex = _arguments.IndexOf("-o");
            if (outputFlagIndex == -1) {
                outputFlagIndex = _arguments.IndexOf("--output");
            }
            if (outputFlagIndex != -1) {
                _configuration.OutputPath = _arguments[outputFlagIndex + 1];
            }
        }

        private void ParseCompress() {
            if (_arguments.IndexOf("-c") != -1 || _arguments.IndexOf("--compress") != -1) {
                _configuration.Compress = true;
            }
        }

        private void ParseFormat() {
            var formatFlagIndex = _arguments.IndexOf("-f");
            if (formatFlagIndex == -1) {
                formatFlagIndex = _arguments.IndexOf("--format");
            }
            if (formatFlagIndex != -1) {
                _configuration.Format = _arguments[formatFlagIndex + 1].ToLower();
            }

            var supportFormatList = new List<string>() {
                "png", "ico"
            };
            if(!supportFormatList.Contains(_configuration.Format)) {
                throw new ArgumentException("Invalid argument");
            }
        }

    }
}
