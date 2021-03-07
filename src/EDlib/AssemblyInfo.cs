using EDlib;
using System;

[assembly: LinkerSafe]

namespace EDlib
{
    /// <summary>
    /// This attribute allows you to mark your assemblies as safe to link.<br />When present the linker will process the assembly even if you’re using the "SDK Assemblies Only" option.
    /// </summary>
    public class LinkerSafeAttribute : Attribute
    {
    }
}
