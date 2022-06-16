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
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 1;
			Item.consumable = false;           
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