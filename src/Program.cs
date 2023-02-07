using DotnetDocumentationGenerator.data;
using MarkdownWriter;
using MarkdownWriter.data;
using System.CommandLine;
using System.Xml.Linq;

namespace DotnetDocumentationGenerator;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        var pathOption = new Option<string>(
            name: "--path",
            description: "The path to the documentation file");

        var rootCommand = new RootCommand("Documentation Generator");
        rootCommand.AddOption(pathOption);

        rootCommand.SetHandler(async (path) =>
        {
            await GenerateDoc(path!);
        }, pathOption);

        return await rootCommand.InvokeAsync(args);
    }

    static async Task GenerateDoc(string path)
    {
        if (!CheckIfFileExists(path))
        {
            Console.WriteLine("File not exists");
            return;
        }
        
        if (!CheckIfIsXML(path))
        {
            Console.WriteLine("Given file is not XML");
            return;
        }
        
        XElement xml = XElement.Load(path);
        var methods = DownloadMethods(xml);

        await CreateMDDoc(methods);
    }

    async static Task CreateMDDoc(List<Method> methods)
    {
        var md = new Markdown("Doc");

        md.H1("Documentation");

        foreach (var method in methods)
        {
            md.H2(method.Name);

            if (method.Summary != string.Empty)
            {
                md.Text(method.Summary);
                md.LineBreak();
            }

            if (method.Params.Count > 0)
            {
                md.Bold("Params");
                var paramList = new List<ListItem>();

                foreach (var param in method.Params)
                {
                    paramList.Add(new ListItem()
                    {
                        Value = $"{param.Name} - {param.Value}"
                    });
                }

                md.UnorderedList(paramList);
            }

            if (method.Return != string.Empty)
            {
                md.Bold("Returns");
                md.LineBreak();
                md.Text(method.Return);
            }
        }

        await md.SaveAndWrite();

        Console.WriteLine($"The documentation file was created at: {Path.Combine(Directory.GetCurrentDirectory())}");
    }

    static List<Method> DownloadMethods(XElement xml)
    {
        var methods = new List<Method>();

        foreach (var data in xml.Descendants("member"))
        {
            var method = new Method();

            var name = data.Attribute("name");
            var summary = data.Element("summary");
            var @params = data.Elements("param");
            var returns = data.Element("returns");

            if (name != null)
                method.Name = name.Value;

            if (summary != null)
                method.Summary = summary.Value.Trim();

            foreach (var param in @params)
            {
                var paramName = param.Attribute("name");
                if (paramName != null)
                    method.Params.Add(new Param()
                    {
                        Name = paramName.Value,
                        Value = param.Value
                    });
            }

            if (returns != null)
                method.Return = returns.Value;

            methods.Add(method);
        }

        return methods;
    }
    
    static bool CheckIfFileExists(string path) => File.Exists(path);
    
    static bool CheckIfIsXML(string path)
    {
        var ext = Path.GetExtension(path);
        if (ext == null)
            return false;
        return ext.Equals(".xml");
    }
}