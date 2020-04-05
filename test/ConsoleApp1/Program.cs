using Eris.SmartCodeGenerator.ImmutableGenerator;
using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new TestObject2(1, "name");
        }
    }

    [ImmutableConstructor]
    public sealed partial class TestObject2
    {
        public uint Id { get; }
        public string Name { get; }
    }
}

namespace Eris.SmartCodeGenerator.ImmutableGenerator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [Conditional("CodeGeneration")]
    public sealed class ImmutableConstructorAttribute : Attribute
    {
    }
}