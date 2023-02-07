# Dotnet Documentation Generator

A dotnet tool that generates project markdown documentation based on xml documentation
<br>
> In order for the documentation to be generated, the given functions must have xml documentation
```
///<summary>
///...
```

## Installation
```dotnet tool install --global DotnetDocumentationGenerator```

## Usage
1. To generate documentation, remember to select the option `Build -> Output -> Documentation File` in the project you are interested in.
This option will generate documentation in the form of an XML file.

![XML Documenation generation](https://github.com/KimPiks/Dotnet-Documentation-Generator/blob/main/documenation-tutorial.png)

2. Use command `generate-doc --path "XML-FILE-PATH"`

## Example
```generate-doc --path "C:\Users\kamil\OneDrive\Desktop\ConsoleFeatures\src\bin\Release\net7.0\ConsoleFeatures.xml"```

![Example md doc](https://raw.githubusercontent.com/KimPiks/Dotnet-Documentation-Generator/main/example-md-doc.png)
