using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class animAnimNode_BoolConstant : animAnimNode_BoolValue
	{
		private CBool _value;

		[Ordinal(11)] 
		[RED("value")] 
		public CBool Value
		{
			get => GetProperty(ref _value);
			set => SetProperty(ref _value, value);
		}

		public animAnimNode_BoolConstant(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}