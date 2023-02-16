using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Otherworld;
using SOTS.Items.Otherworld.Furniture;

namespace SOTS.Items.Otherworld.FromChests
{
	public class CataclysmMusketPouch : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 1;
			Item.consumable = true;           
			Item.knockBack = 1f;
            Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightRed; 
			Item.shoot = ModContent.ProjectileType<CataclysmBullet>();  
			Item.shootSpeed = 1f;           
			Item.ammo = AmmoID.Bullet;
		}
		public void UpdateShoot()
		{
			if (Item.favorited)
			{
				Item.shoot = ModContent.ProjectileType<ChargedCataclysmBullet>();
			}
			else
			{
				Item.shoot = ModContent.ProjectileType<CataclysmBullet>();
			}
		}
		public override bool BeforeConsumeAmmo(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			UpdateShoot();
			if (Item.favorited)
			{
				voidPlayer.voidMeter -= 0.75f;
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.EndlessMusketPouch, 1).AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 8).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}