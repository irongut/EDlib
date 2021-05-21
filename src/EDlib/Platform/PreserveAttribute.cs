using System;
using System.ComponentModel;

namespace EDlib.Platform
{
	/// <summary>
	///   <para>Linker control attribute.</para>
	///   <para>To preserve the whole type, use the syntax <c>[Preserve (AllMembers = true)]</c> on the type.<br/>
	///			To preserve a member if the containing type was preserved, use <c>[Preserve (Conditional=true)]</c> on the member.</para>
	///   <para>See: https://docs.microsoft.com/en-gb/xamarin/ios/deploy-test/linker </para>
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class PreserveAttribute : Attribute
	{
		/// <summary>Set <c>true</c> to preserve the whole type.</summary>
		public bool AllMembers;

		/// <summary>Set <c>true</c> to preserve an individual type member.</summary>
		public bool Conditional;

		/// <summary>Preserve attribute constructor.</summary>
		/// <param name="allMembers">Set <c>true</c> to preserve the whole type.</param>
		/// <param name="conditional">Set <c>true</c> to preserve an individual type member.</param>
		public PreserveAttribute(bool allMembers, bool conditional)
		{
			AllMembers = allMembers;
			Conditional = conditional;
		}

		/// <summary>Preserve attribute default constructor.</summary>
		public PreserveAttribute() { }
	}
}
