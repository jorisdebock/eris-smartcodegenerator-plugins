# SmartCodeGenerator Immutable plugin

Using [SmartCodeGenerator](https://github.com/cezarypiatek/SmartCodeGenerator)

## How to use a custom plugin

1. Install `SmartCodeGenerator.Engine` NuGet package
2. Install `Eris.SmartCodeGenerator.ImmutableGenerator` NuGet package
3. Add source code of the marking attribute `ImmutableConstructor` into your codebase
4. Since now you can start marking code with your attribute and after recompilation, the generated code should be available at your disposal.

__IMPORTANT:__ The source code of the marking attribute need to be copied into a consuming project because there should be no runtime dependency between the project that uses the generator plugin and the generator plugin itself. 


```c#
namespace Eris.SmartCodeGenerator.ImmutableGenerator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [Conditional("CodeGeneration")]
    public sealed class ImmutableConstructorAttribute : Attribute
    {
    }
}
```

## Examples

### Constructor

Add the `ImmutableConstructor` attribute to your class, this class should be partial and only contain readonly properties
```c#
[ImmutableConstructor]
public sealed partial class TestObject
{
    public uint Id { get; }
    public string Name { get; }
}
```

After building the project the constructor is generated
```c#
public class Program
{
    public static void Main(string[] args)
    {
        new TestObject(1, "name");
    }
}
```

The code that is generated
```c#
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using Eris.SmartCodeGenerator.ImmutableGenerator;
using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Eris.SmartCodeGenerator.ImmutableGenerator.OnBuildImmutableGenerator", "1.0.0.0")]
    public sealed partial class TestObject
    {
        public TestObject
       (uint id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
```