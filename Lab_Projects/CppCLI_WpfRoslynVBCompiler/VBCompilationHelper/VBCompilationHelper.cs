using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;

namespace VBCompilationHelper
{
    public class CompilationResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public Assembly? LoadedAssembly { get; set; }
        public Type? FirstClassType { get; set; }
        public object? Instance { get; set; }
        public MethodInfo[]? Methods { get; set; }
        public FieldInfo[]? Fields { get; set; }
    }

    public static class VBCompiler
    {
        public static CompilationResult CompileVBCode(string code, OutputKind outputKind)
        {
            var result = new CompilationResult();

            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    result.Errors.Add("No code to compile!");
                    return result;
                }

                var sourceText = SourceText.From(code, Encoding.UTF8);
                var syntaxTree = VisualBasicSyntaxTree.ParseText(sourceText);

                var references = new List<MetadataReference>
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(File).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Encoding).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Microsoft.VisualBasic.Strings).Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
                };


                var compilation = VisualBasicCompilation.Create(
                    assemblyName: "DynamicVBAssembly",
                    syntaxTrees: new[] { syntaxTree },
                    references: references,
                    options: new VisualBasicCompilationOptions(outputKind)
                );

                using var peStream = new MemoryStream();
                EmitResult emitResult = compilation.Emit(peStream);

                if (!emitResult.Success)
                {
                    foreach (var diagnostic in emitResult.Diagnostics)
                    {
                        if (diagnostic.Severity == DiagnosticSeverity.Error)
                        {
                            result.Errors.Add(diagnostic.ToString());
                        }
                    }
                    return result;
                }

                peStream.Seek(0, SeekOrigin.Begin);
                byte[] assemblyBytes = peStream.ToArray();
                var loadedAssembly = Assembly.Load(assemblyBytes);
                result.LoadedAssembly = loadedAssembly;

                var firstClass = loadedAssembly.GetTypes().FirstOrDefault(t => t.IsClass);
                if (firstClass == null)
                {
                    result.Errors.Add("No public classes in compilated assembly!");
                    return result;
                }

                result.FirstClassType = firstClass;
                result.Instance = Activator.CreateInstance(firstClass);

                result.Methods = firstClass.GetMethods(
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
                );

                result.Fields = firstClass.GetFields(
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
                );

                result.Success = true;

                if (result.Methods.Length == 0 && result.Fields.Length == 0)
                {
                    result.Errors.Add("Kompilacja OK. Brak publicznych członków instancyjnych.");
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Wyjątek: {ex.Message}");
            }

            return result;
        }
    }
}