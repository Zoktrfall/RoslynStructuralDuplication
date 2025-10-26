using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace RoslynStructuralDuplication;

class Program
{
    private sealed class StructuralDuplicateFeature : CSharpSyntaxRewriter
    {
        private string SuggestUniqueName(string baseName, ISet<string> taken)
        {
            for (int i = 2; i < int.MaxValue; i++)
            {
                var candidate = baseName + i.ToString();
                if (!taken.Contains(candidate))
                    return candidate;
            }
            return baseName + "_dup";
        }
        
        // public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
        // {
        //     var plist = node.ParameterList;
        //     if (plist is null || plist.Parameters.Count != 1)
        //         return base.VisitMethodDeclaration(node);
        //
        //     var originalParam = plist.Parameters[0];
        //     
        //     var taken = new HashSet<string>(
        //         plist.Parameters.Select(p => p.Identifier.ValueText),
        //         StringComparer.Ordinal);
        //
        //     var suggested = SuggestUniqueName(originalParam.Identifier.ValueText, taken);
        //     
        //     var duplicated = originalParam.WithIdentifier(
        //         SyntaxFactory.Identifier(suggested).WithTriviaFrom(originalParam.Identifier));
        //     
        //     var newParams = plist.Parameters.Add(duplicated);
        //     var newPlist  = plist.WithParameters(newParams);
        //
        //     var updatedMethod = node.WithParameterList(newPlist);
        //     return base.VisitMethodDeclaration(updatedMethod);
        // }
        
        public string ProcessSingleParameterMethods(string content)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(content);
            var rootNode = syntaxTree.GetRoot();
            
            var newRoot = Visit(rootNode);
            
            using var workspace = new AdhocWorkspace();
            var formatted = Formatter.Format(newRoot!, workspace);

            return formatted.ToFullString();
        }
    }
    
    static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: RoslynStructuralDuplication.exe <input.cs>");
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

            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Output.cs");
            File.WriteAllText(outputPath, outputFileContent);

            Console.WriteLine($"Output file created at: {outputPath}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return 0;
    }
}