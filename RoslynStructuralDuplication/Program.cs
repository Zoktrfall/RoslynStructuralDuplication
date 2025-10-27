using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace RoslynStructuralDuplication;

class Program
{
    private sealed class StructuralDuplicateFeature : CSharpSyntaxRewriter
    {
        private static string SuggestAnotherName(SyntaxToken id)
        {
            var text = id.Text;
            int j = text.Length;
            while (j > 0 && char.IsDigit(text[j - 1]))
                j--;

            if (j == text.Length)
                text += "2";
            else
                text = text[..j] + (int.Parse(text[j..]) + 1);

            return text;
        }
        public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var parameterList = node.ParameterList;
            if (parameterList.Parameters.Count != 1)
                return base.VisitMethodDeclaration(node);
        
            var originalParameter = parameterList.Parameters[0];
            
            var duplicatedParameter = originalParameter.WithIdentifier(
                SyntaxFactory.Identifier(SuggestAnotherName(originalParameter.Identifier))
                    .WithTriviaFrom(originalParameter.Identifier));
            
            var newParameters = parameterList.Parameters.Add(duplicatedParameter);
            var newParametersList  = parameterList.WithParameters(newParameters);
        
            var updatedMethod = node.WithParameterList(newParametersList);
            return base.VisitMethodDeclaration(updatedMethod);
        }
        
        public string ProcessSingleParameterMethods(string content)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(content);
            var rootNode = syntaxTree.GetRoot();
            
            var newRootNode = Visit(rootNode);
            
            using var workspace = new AdhocWorkspace();
            var formatted = Formatter.Format(newRootNode, workspace);
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
            var duplicateFeature = new StructuralDuplicateFeature();
            
            var inputFileContent = File.ReadAllText(inputFile);
            var outputFileContent = duplicateFeature
                .ProcessSingleParameterMethods(inputFileContent);

            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Output.cs");
            File.WriteAllText(outputPath, outputFileContent);
            Console.WriteLine($"Output file created at: {outputPath}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 3;
        }
        
        return 0;
    }
}