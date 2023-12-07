using static WolvenKit.RED4.Types.Enums;

namespace WolvenKit.RED4.Types
{
	public partial class enteventsToggleImpulseDestruction : redEvent
	{
		[Ordinal(0)] 
		[RED("enable")] 
		public CBool Enable
		{
			get => GetPropertyValue<CBool>();
			set => SetPropertyValue<CBool>(value);
		}

		public enteventsToggleImpulseDestruction()
		{
			Enable = true;

			PostConstruct();
		}

		partial void PostConstruct();
	}
}
