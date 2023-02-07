namespace DotnetDocumentationGenerator.data
{
    public class Method
    {
        public string Name { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<Param> Params { get; set; } = new List<Param>();
        public string Return { get; set; } = string.Empty; 
    }
}
