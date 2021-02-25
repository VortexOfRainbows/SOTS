using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Otherworld.FromChests
{
	public class CataclysmMusketPouch : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysm Musket Pouch");
			Tooltip.SetDefault("Grants access to infinite cataclysm bullets\nCataclysm bullets travel faster and more erratically than normal bullets\nThey will also incur 20% damage to an enemy surrounding the initially hit enemy\nWhen above 50% void, bullets will be supercharged at the cost of some void\nSupercharged bullets travel instantly, and gain increased arcing capabilies\nDecreases void regen by 1 while in the inventory");
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
			item.rare = ItemRarityID.LightPurple; 
			item.shoot = mod.ProjectileType("CataclysmBullet");  
			item.shootSpeed = 1f;           
			item.ammo = AmmoID.Bullet;
		}
		public void UpdateShoot(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (voidPlayer.voidMeter > voidPlayer.voidMeterMax2 * 0.5f)
			{
				item.shoot = mod.ProjectileType("ChargedCataclysmBullet");
			}
			else
			{
				item.shoot = mod.ProjectileType("CataclysmBullet");
			}
		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen -= 0.1f;
		}
		public override bool BeforeConsumeAmmo(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			UpdateShoot(player);
			if (voidPlayer.voidMeter > voidPlayer.voidMeterMax2 * 0.5f)
			{
				voidPlayer.voidMeter -= 0.75f;
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EndlessMusketPouch, 1);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 8);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}