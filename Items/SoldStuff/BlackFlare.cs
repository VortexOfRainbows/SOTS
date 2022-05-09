using SOTS.Projectiles.Otherworld;
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
			Item.damage = 1;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 20;
			Item.maxStack = 999;
			Item.consumable = true;           
			Item.knockBack = 2f;
			Item.value = Item.buyPrice(0, 0, 0, 9);
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<BlackFlareShot>(); 
			Item.shootSpeed =  8f;             
			Item.ammo = AmmoID.Flare;   
            Item.UseSound = SoundID.Item23;
		}
	}
}