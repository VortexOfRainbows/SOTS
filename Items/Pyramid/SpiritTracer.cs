using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class SpiritTracer : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Tracer");
			Tooltip.SetDefault("Fires phantom arrows\nCan hit up to 3 enemies at a time");
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 33;
			Item.ranged = true;
			Item.width = 30;
			Item.height = 68;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = 5;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 7, 25, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;            
			Item.shoot = 1; 
            Item.shootSpeed = 16.5f;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedMatter", 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override int GetVoid(Player player)
		{
			return  6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("TracerArrow"), damage, knockBack, player.whoAmI, 0, type);
			return false; 
		}
	}
}