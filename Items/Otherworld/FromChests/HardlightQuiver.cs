using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Otherworld.FromChests
{
	public class HardlightQuiver : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Quiver");
			Tooltip.SetDefault("Grants access to infinite hardlight arrows\nHardlight arrows travel faster and are not affected by gravity\nWhen above 50% void, arrows will be supercharged at the cost of some void\nSupercharged arrows travel instantly, and gain slight homing at longer ranges");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 6;
			item.ranged = true;
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.consumable = false;           
			item.knockBack = 0.2f;
            item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.LightPurple; 
			item.shoot = mod.ProjectileType("HardlightArrow");  
			item.shootSpeed = 1f;           
			item.ammo = AmmoID.Arrow;   
		}
		public void UpdateShoot(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (voidPlayer.voidMeter > voidPlayer.voidMeterMax2 * 0.5f)
			{
				item.shoot = mod.ProjectileType("ChargedHardlightArrow");
			}
			else
			{
				item.shoot = mod.ProjectileType("HardlightArrow");
			}
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
			recipe.AddIngredient(ItemID.EndlessQuiver, 1);
			recipe.AddIngredient(null, "HardlightAlloy", 8);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}