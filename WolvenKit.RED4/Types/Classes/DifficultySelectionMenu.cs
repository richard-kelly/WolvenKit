using static WolvenKit.RED4.Types.Enums;

namespace WolvenKit.RED4.Types
{
	public partial class DifficultySelectionMenu : gameuiBaseCharacterCreationController
	{
		[Ordinal(6)] 
		[RED("difficultyTitle")] 
		public inkTextWidgetReference DifficultyTitle
		{
			get => GetPropertyValue<inkTextWidgetReference>();
			set => SetPropertyValue<inkTextWidgetReference>(value);
		}

		[Ordinal(7)] 
		[RED("textureTop")] 
		public inkImageWidgetReference TextureTop
		{
			get => GetPropertyValue<inkImageWidgetReference>();
			set => SetPropertyValue<inkImageWidgetReference>(value);
		}

		[Ordinal(8)] 
		[RED("textureBottom")] 
		public inkImageWidgetReference TextureBottom
		{
			get => GetPropertyValue<inkImageWidgetReference>();
			set => SetPropertyValue<inkImageWidgetReference>(value);
		}

		[Ordinal(9)] 
		[RED("hoverAnimation")] 
		public CName HoverAnimation
		{
			get => GetPropertyValue<CName>();
			set => SetPropertyValue<CName>(value);
		}

		[Ordinal(10)] 
		[RED("difficulty0")] 
		public inkWidgetReference Difficulty0
		{
			get => GetPropertyValue<inkWidgetReference>();
			set => SetPropertyValue<inkWidgetReference>(value);
		}

		[Ordinal(11)] 
		[RED("difficulty1")] 
		public inkWidgetReference Difficulty1
		{
			get => GetPropertyValue<inkWidgetReference>();
			set => SetPropertyValue<inkWidgetReference>(value);
		}

		[Ordinal(12)] 
		[RED("difficulty2")] 
		public inkWidgetReference Difficulty2
		{
			get => GetPropertyValue<inkWidgetReference>();
			set => SetPropertyValue<inkWidgetReference>(value);
		}

		[Ordinal(13)] 
		[RED("difficulty3")] 
		public inkWidgetReference Difficulty3
		{
			get => GetPropertyValue<inkWidgetReference>();
			set => SetPropertyValue<inkWidgetReference>(value);
		}

		[Ordinal(14)] 
		[RED("animationProxy")] 
		public CHandle<inkanimProxy> AnimationProxy
		{
			get => GetPropertyValue<CHandle<inkanimProxy>>();
			set => SetPropertyValue<CHandle<inkanimProxy>>(value);
		}

		[Ordinal(15)] 
		[RED("lastShownPart")] 
		public CName LastShownPart
		{
			get => GetPropertyValue<CName>();
			set => SetPropertyValue<CName>(value);
		}

		[Ordinal(16)] 
		[RED("lastAtlas")] 
		public redResourceReferenceScriptToken LastAtlas
		{
			get => GetPropertyValue<redResourceReferenceScriptToken>();
			set => SetPropertyValue<redResourceReferenceScriptToken>(value);
		}

		[Ordinal(17)] 
		[RED("translationAnimationCtrl")] 
		public CWeakHandle<inkTextReplaceAnimationController> TranslationAnimationCtrl
		{
			get => GetPropertyValue<CWeakHandle<inkTextReplaceAnimationController>>();
			set => SetPropertyValue<CWeakHandle<inkTextReplaceAnimationController>>(value);
		}

		[Ordinal(18)] 
		[RED("localizedText")] 
		public CString LocalizedText
		{
			get => GetPropertyValue<CString>();
			set => SetPropertyValue<CString>(value);
		}

		[Ordinal(19)] 
		[RED("c_atlas1")] 
		public redResourceReferenceScriptToken C_atlas1
		{
			get => GetPropertyValue<redResourceReferenceScriptToken>();
			set => SetPropertyValue<redResourceReferenceScriptToken>(value);
		}

		[Ordinal(20)] 
		[RED("c_atlas2")] 
		public redResourceReferenceScriptToken C_atlas2
		{
			get => GetPropertyValue<redResourceReferenceScriptToken>();
			set => SetPropertyValue<redResourceReferenceScriptToken>(value);
		}

		[Ordinal(21)] 
		[RED("isInputLocked")] 
		public CBool IsInputLocked
		{
			get => GetPropertyValue<CBool>();
			set => SetPropertyValue<CBool>(value);
		}

		public DifficultySelectionMenu()
		{
			DifficultyTitle = new inkTextWidgetReference();
			TextureTop = new inkImageWidgetReference();
			TextureBottom = new inkImageWidgetReference();
			Difficulty0 = new inkWidgetReference();
			Difficulty1 = new inkWidgetReference();
			Difficulty2 = new inkWidgetReference();
			Difficulty3 = new inkWidgetReference();
			LastAtlas = new redResourceReferenceScriptToken { Resource = new CResourceAsyncReference<CResource>(@"base\gameplay\gui\fullscreen\main_menu\difficulty_level.inkatlas") };
			C_atlas1 = new redResourceReferenceScriptToken { Resource = new CResourceAsyncReference<CResource>(@"base\gameplay\gui\fullscreen\main_menu\difficulty_level.inkatlas") };
			C_atlas2 = new redResourceReferenceScriptToken { Resource = new CResourceAsyncReference<CResource>(@"base\gameplay\gui\fullscreen\main_menu\difficulty_level1.inkatlas") };

			PostConstruct();
		}

		partial void PostConstruct();
	}
}
