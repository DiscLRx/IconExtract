using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;


namespace IconExtract {



    internal class CompressHelper {

        private string _compressExecFile;

        public CompressHelper() {

            bool _is64Bit = Environment.Is64BitProcess;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                _compressExecFile = $"7z-win-{(_is64Bit ? "x64" : "x86")}.exe";
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                _compressExecFile = $"7zz-linux-{(_is64Bit ? "x64" : "x86")}";
            } else {
                throw new NotSupportedException("Only Windows and Linux are supported");
            }

            _compressExecFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "7z", _compressExecFile);

        }

        public Task Decompress(string input, string output) {

            var process = new Process();
            process.StartInfo.FileName = _compressExecFile;
            process.StartInfo.ArgumentList.Add("x");
            process.StartInfo.ArgumentList.Add(input);
            process.StartInfo.ArgumentList.Add($"-o{output}");
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();
            return Task.CompletedTask;
        }

        public Task Compress(string target, params string[] files) {

            var filesArg = new StringBuilder();
            foreach (var file in files) {
                filesArg.Append(' ');
                filesArg.Append(file);
            }
            var process = new Process() {
                StartInfo = new(_compressExecFile, $"a {target}{filesArg}")
            };
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();

            return Task.CompletedTask;
        }

    }
}
