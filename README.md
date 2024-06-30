# XsdTools
Small tool to automatically download heavily linked XSD schemas and prepare the files in a way, so they 
can be easily fed to the xsd.exe in order to generate C# classes.

Why? Because I really can't be bothered to do that manually.

## Usage
From the help output of the tool:

```
Description:
  XsdTool

Usage:
  XsdTools [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  f, fetch <InputXsd>            Given an xsd schema, fetches all linked (imported) schemas.
  gc, generate-classes <Folder>  Generate classes given a folder with xsd schemas.

```

### Command fetch, f
```
Description:
  Given an xsd schema, fetches all linked (imported) schemas.

Usage:
  XsdTools fetch <InputXsd> [options]

Arguments:
  <InputXsd>  The 'root' xsd file for which related schemas should be fetched

Options:
  --clean         If set, the output directory will be deleted before generation.
  -?, -h, --help  Show help and usage information
```

### Command generate-classes, gc
```
Description:
  Generate classes given a folder with xsd schemas.

Usage:
  XsdTools generate-classes <Folder> [options]

Arguments:
  <Folder>  Specify the folder to process with the xsd tool.

Options:
  -?, -h, --help  Show help and usage information
```