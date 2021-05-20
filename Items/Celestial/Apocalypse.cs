using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Celestial
{
	public class Apocalypse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apocalypse");
			Tooltip.SetDefault("Release green thunder towards your cursor\nGreen thunder chains off enemies for 90% damage\nFirerate increases after each use\nProvides a light source while in the inventory\n'Power straight from the underworld'");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 330;
			item.magic = true;
			item.width = 30;
			item.height = 30;
            item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("GreenLightning"); 
			item.shootSpeed = 1;
			item.knockBack = 5;
			item.UseSound = SoundID.Item92;
			item.noUseGraphic = true;
		}
		public override void UpdateInventory(Player player)
		{
			Lighting.AddLight(player.Center, 1.25f, 1.25f, 1.25f);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(null, "GreenJellyfishStaff", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			for(int i = 0; i < 2; i++)
			{
				Vector2 speed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(-1.0f + 2 * i));
				Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, player.whoAmI, 0, 6f);
			}
			return false; 
		}
		public override void GetVoid(Player player)
		{
			voidMana = 8;
		}
	}
}