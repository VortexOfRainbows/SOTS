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
			DisplayName.SetDefault("Cataclysm Musket Pouch");
			Tooltip.SetDefault("Grants access to infinite cataclysm bullets\nCataclysm bullets travel faster and more erratically than normal bullets\nThey will also incur 20% damage to an enemy surrounding the initially hit enemy\nWhen favorited, bullets will be supercharged at the cost of some void\nSupercharged bullets travel instantly, and gain increased arcing capabilies");
		}public override void SafeSetDefaults()
		{
			item.damage = 8;
			item.ranged = true;
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.consumable = false;           
			item.knockBack = 1f;
            item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.LightRed; 
			item.shoot = ModContent.ProjectileType<CataclysmBullet>();  
			item.shootSpeed = 1f;           
			item.ammo = AmmoID.Bullet;
		}
		public void UpdateShoot()
		{
			if (item.favorited)
			{
				item.shoot = ModContent.ProjectileType<ChargedCataclysmBullet>();
			}
			else
			{
				item.shoot = ModContent.ProjectileType<CataclysmBullet>();
			}
		}
		public override bool BeforeConsumeAmmo(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			UpdateShoot();
			if (item.favorited)
			{
				voidPlayer.voidMeter -= 0.75f;
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EndlessMusketPouch, 1);
			recipe.AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 8);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}