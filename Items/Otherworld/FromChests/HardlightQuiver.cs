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
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
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
			CreateRecipe(1).AddIngredient(ItemID.EndlessQuiver, 1).AddIngredient(ModContent.ItemType<HardlightAlloy>(), 8).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}