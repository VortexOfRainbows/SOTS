using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class PharaohsCane : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pharaoh's Cane");
			Tooltip.SetDefault("Extremely valuable but useless for fighting\nWho would ever want such a thing?");
		}
		public override void SetDefaults()
		{

			item.damage = 6;
			item.melee = true;
			item.width = 36;
			item.height = 26;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 1;
			item.knockBack = 1.8f;
			item.value = Item.sellPrice(0, 35, 95, 72);
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            

		}

		
	}
}