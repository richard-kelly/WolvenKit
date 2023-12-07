using static WolvenKit.RED4.Types.Enums;

namespace WolvenKit.RED4.Types
{
	public partial class VendorInventoryItemData : WrappedInventoryItemData
	{
		[Ordinal(11)] 
		[RED("IsVendorItem")] 
		public CBool IsVendorItem
		{
			get => GetPropertyValue<CBool>();
			set => SetPropertyValue<CBool>(value);
		}

		[Ordinal(12)] 
		[RED("IsEnoughMoney")] 
		public CBool IsEnoughMoney
		{
			get => GetPropertyValue<CBool>();
			set => SetPropertyValue<CBool>(value);
		}

		[Ordinal(13)] 
		[RED("IsBuybackStack")] 
		public CBool IsBuybackStack
		{
			get => GetPropertyValue<CBool>();
			set => SetPropertyValue<CBool>(value);
		}

		[Ordinal(14)] 
		[RED("IsDLCAddedActiveItem")] 
		public CBool IsDLCAddedActiveItem
		{
			get => GetPropertyValue<CBool>();
			set => SetPropertyValue<CBool>(value);
		}

		public VendorInventoryItemData()
		{
			PostConstruct();
		}

		partial void PostConstruct();
	}
}
