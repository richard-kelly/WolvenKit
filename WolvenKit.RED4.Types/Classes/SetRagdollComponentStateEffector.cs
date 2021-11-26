using static WolvenKit.RED4.Types.Enums;

namespace WolvenKit.RED4.Types
{
	[REDMeta]
	public partial class SetRagdollComponentStateEffector : gameEffector
	{
		[Ordinal(0)] 
		[RED("state")] 
		public CBool State
		{
			get => GetPropertyValue<CBool>();
			set => SetPropertyValue<CBool>(value);
		}
	}
}