using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class BlackFlare : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Black Flare");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.damage = 1;
			item.ranged = true;
			item.width = 8;
			item.height = 20;
			item.maxStack = 999;
			item.consumable = true;           
			item.knockBack = 2f;
			item.value = Item.buyPrice(0, 0, 0, 9);
			item.rare = ItemRarityID.Orange;
			item.shoot = mod.ProjectileType("BlackFlareShot"); 
			item.shootSpeed =  8f;             
			item.ammo = AmmoID.Flare;   
            item.UseSound = SoundID.Item23;
		}
	}
}