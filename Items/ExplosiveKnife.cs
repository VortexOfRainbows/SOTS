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
			Item.CloneDefaults(ItemID.ThrowingKnife);
			Item.damage = 15;
			Item.useTime = 17;
			Item.useAnimation = 17;
			Item.thrown = true;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<Projectiles.ExplosiveKnife>(); 
            Item.shootSpeed = 12f;
			Item.consumable = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return true; 
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 15);
			recipe.AddIngredient(ItemID.Grenade, 15);
			recipe.AddIngredient(ModContent.ItemType<AncientSteelBar>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 15);
			recipe.AddRecipe();
		}
	}
}