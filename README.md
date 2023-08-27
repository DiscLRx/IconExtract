# IconExtract

### Usage

`-i` `--input` Path to the input file

`-o` `--output` Name of the output file or directory. By default, it is ***icon***

`-c` `--compress` Compress the icons into one file. When using this argument, the suffix of the output argument should indicate the corresponding compressed file type. For example, you can specify that the compressed file type is ***zip*** by using ***icon.zip*** as the output argument. If the type of the compressed file is not specified, it will be ***7z***.

`-f` `--format` Format of the icon file. The value must be ***png*** or ***ico***

```pwsh
.\IconExtract-win-x64.exe -i input
.\IconExtract-win-x64.exe -i input -o output -c
.\IconExtract-win-x64.exe -i input -f png
```