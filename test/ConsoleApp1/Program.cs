using Eris.SmartCodeGenerator.ImmutableGenerator;
using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new TestObject(1, "name");
        }
    }

    [ImmutableConstructor]
    public sealed partial class TestObject
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