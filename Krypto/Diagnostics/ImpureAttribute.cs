namespace Krypto.Diagnostics;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
internal sealed class ImpureAttribute : Attribute;
