using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class SandstoneEdge : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandstone Edge");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.damage = 19;
			item.melee = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 50;
			item.useAnimation = 25;
			item.useStyle = 1;
			item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = 4;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("SandyCloud"); 
            item.shootSpeed = 8;

		}

		
	}
}