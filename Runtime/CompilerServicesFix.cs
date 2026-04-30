// This file is needed to support newer features on older target frameworks
// More details: https://github.com/dotnet/roslyn/issues/45510

using System.ComponentModel;
// ReSharper disable UnusedType.Global
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

namespace System.Runtime.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class IsExternalInit { }

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ModuleInitializerAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
	public sealed class RequiredMemberAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
	public sealed class CompilerFeatureRequiredAttribute : Attribute
	{
		public CompilerFeatureRequiredAttribute(string featureName) { }

		public bool IsOptional { get; init; }
	}
}