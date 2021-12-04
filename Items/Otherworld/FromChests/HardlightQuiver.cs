using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Otherworld;
using SOTS.Items.Otherworld.Furniture;

namespace SOTS.Items.Otherworld.FromChests
{
	public class HardlightQuiver : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Quiver");
			Tooltip.SetDefault("Grants access to infinite hardlight arrows\nHardlight arrows travel faster and are not affected by gravity\nWhen favorited, arrows will be supercharged at the cost of some void\nSupercharged arrows travel instantly, and gain slight homing at longer ranges");
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
			item.rare = ItemRarityID.LightRed;
			item.shoot = ModContent.ProjectileType<HardlightArrow>();  
			item.shootSpeed = 1f;           
			item.ammo = AmmoID.Arrow;   
		}
		public void UpdateShoot()
		{
			if (item.favorited)
			{
				item.shoot = ModContent.ProjectileType<ChargedHardlightArrow>();
			}
			else
			{
				item.shoot = ModContent.ProjectileType<HardlightArrow>();
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
			recipe.AddIngredient(ItemID.EndlessQuiver, 1);
			recipe.AddIngredient(ModContent.ItemType<HardlightAlloy>(), 8);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}