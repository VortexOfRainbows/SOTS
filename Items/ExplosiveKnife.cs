using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Items.GhostTown;

namespace SOTS.Items
{
	public class ExplosiveKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Knife");
			Tooltip.SetDefault("'Quite a deadly combination'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ThrowingKnife);
			item.damage = 15;
			item.useTime = 17;
			item.useAnimation = 17;
			item.thrown = true;
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<Projectiles.ExplosiveKnife>(); 
            item.shootSpeed = 12f;
			item.consumable = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 15);
			recipe.AddIngredient(ItemID.Grenade, 15);
			recipe.AddIngredient(ModContent.ItemType<AncientSteelBar>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 15);
			recipe.AddRecipe();
		}
	}
}