using System;
using System.Diagnostics;

namespace Eris.SmartCodeGenerator.ImmutableGenerator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [Conditional("CodeGeneration")]
    public sealed class ImmutableConstructorAttribute : Attribute
    {
    }
}