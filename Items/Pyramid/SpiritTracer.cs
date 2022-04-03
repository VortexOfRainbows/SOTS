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
			item.damage = 33;
			item.ranged = true;
			item.width = 30;
			item.height = 68;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = 5;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 7, 25, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;            
			item.shoot = 1; 
            item.shootSpeed = 16.5f;
			item.useAmmo = AmmoID.Arrow;
			item.noMelee = true;
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