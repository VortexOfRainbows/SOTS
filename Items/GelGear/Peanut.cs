using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class Peanut : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peanut");
			Tooltip.SetDefault("An oversized peanut\nCan be used in a snowball cannon");
		}public override void SetDefaults()
		{
			item.damage = 12;
			item.width = 26;
			item.height = 26;
			item.maxStack = 999;
			item.consumable = true;       
			item.knockBack = 1.15f;
			item.value = 200;
			item.rare = 1;
			item.shoot = mod.ProjectileType("Peanut");   
			item.shootSpeed = 0.5f;     
			item.ammo = ItemID.Snowball;   
		}
	}
}