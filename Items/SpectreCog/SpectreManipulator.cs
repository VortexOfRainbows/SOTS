using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpectreCog
{
	public class SpectreManipulator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Manipulator");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 48;
			item.value = 125;
			item.rare = 9;
			item.maxStack = 4;
		}
	}
}