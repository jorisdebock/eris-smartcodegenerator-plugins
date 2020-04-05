using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SmartCodeGenerator.Sdk;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eris.SmartCodeGenerator.ImmutableGenerator
{
    [CodeGenerator(typeof(ImmutableConstructorAttribute))]
    public class OnBuildImmutableConstructorGenerator : EnrichingCodeGeneratorProxy
    {
        protected override Task<SyntaxList<MemberDeclarationSyntax>> GenerateMembersAsync(CSharpSyntaxNode memberNode, AttributeData markerAttribute, TransformationContext context, CancellationToken cancellationToken)
        {
            //Debugger.Launch();

            var classDeclaration = memberNode as ClassDeclarationSyntax;

            if (!classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                Console.Error.WriteLine("Immutable class should be partial!");
                return Task.FromResult(new SyntaxList<MemberDeclarationSyntax>());
            }

            var parameterList = ParameterList();
            var statementList = List<StatementSyntax>();

            foreach (var member in classDeclaration.Members)
            {
                if (!(member is PropertyDeclarationSyntax propertyDeclaration))
                {
                    Console.Error.WriteLine("Skipping member which is not a property - " + member);
                    continue;
                }

                if (!propertyDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword) ||
                   !propertyDeclaration.AccessorList.Accessors.Any(SyntaxKind.GetAccessorDeclaration) ||
                    propertyDeclaration.AccessorList.Accessors.Any(SyntaxKind.SetAccessorDeclaration))
                {
                    Console.Error.WriteLine("Immutable property should be public and get only! - " + propertyDeclaration);
                    continue;
                }

                var propertyName = propertyDeclaration.Identifier.Text;
                var parameterName = propertyName.ToCamelCase();

                var parameter = Parameter(Identifier(parameterName))
                    .WithType(propertyDeclaration.Type);

                parameterList = parameterList.AddParameters(parameter);

                statementList = statementList.Add(
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(propertyName),
                            IdentifierName(parameterName))));
            }

            var memberDeclarations = SingletonList<MemberDeclarationSyntax>(
                ClassDeclaration(classDeclaration.Identifier)
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.SealedKeyword),
                            Token(SyntaxKind.PartialKeyword)
                        ))
                    .WithMembers(List(new MemberDeclarationSyntax[] {
                        ConstructorDeclaration(classDeclaration.Identifier)
                        .WithModifiers(
                            TokenList(
                                Token(SyntaxKind.PublicKeyword)
                            ))
                        .WithParameterList(parameterList)
                        .WithBody(Block(statementList))
                    })));

            return Task.FromResult(memberDeclarations);
        }
    }

    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
            => new StringBuilder()
                .Append(value[0].ToString().ToLowerInvariant())
                .Append(value, 1, value.Length - 1)
                .ToString();
    }
}