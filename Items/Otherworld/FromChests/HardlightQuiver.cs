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
			Item.damage = 6;
			Item.ranged = true;
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 1;
			Item.consumable = false;           
			Item.knockBack = 0.2f;
            Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.shoot = ModContent.ProjectileType<HardlightArrow>();  
			Item.shootSpeed = 1f;           
			Item.ammo = AmmoID.Arrow;   
		}
		public void UpdateShoot()
		{
			if (Item.favorited)
			{
				Item.shoot = ModContent.ProjectileType<ChargedHardlightArrow>();
			}
			else
			{
				Item.shoot = ModContent.ProjectileType<HardlightArrow>();
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EndlessQuiver, 1);
			recipe.AddIngredient(ModContent.ItemType<HardlightAlloy>(), 8);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}