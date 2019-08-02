using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class BackAttachment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Back Attachment");
			Tooltip.SetDefault("Used to attach stuff to your back");
		}
		public override void SetDefaults()
		{

			item.width = 32;
			item.height = 32;
			item.value = 250000;
			item.rare = 8;

			item.maxStack = 4;

		}
	}
}