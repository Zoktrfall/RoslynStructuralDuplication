using Microsoft.CodeAnalysis.CSharp;

namespace RoslynStructuralDuplication;

class Program
{
    private sealed class StructuralDuplicateFeature : CSharpSyntaxRewriter
    {
        public string ProcessSingleParameterMethods(string content)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(content);
            var rootNode = syntaxTree.GetRoot();
            
            Console.WriteLine(rootNode.Kind());  
            
            foreach (var node in rootNode.DescendantNodes())
                Console.WriteLine(node.Kind());

            return "";
        }
    }
    
    static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: RoslynStructuralDuplication.exe <input.cs> ");
            return 1;
        }
        
        var inputFile = args[0];
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"File {inputFile} does not exist.");
            return 2;
        }

        try
        {
            var feature = new StructuralDuplicateFeature();
            
            var inputFileContent = File.ReadAllText(inputFile);
            var outputFileContent = feature
                .ProcessSingleParameterMethods(inputFileContent);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return 0;
    }
}