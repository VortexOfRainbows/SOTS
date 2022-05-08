using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class StarshotCrossbow : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarshotCrossbowGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[Item.type].Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshot Crossbow");
			Tooltip.SetDefault("Fire a star that scatters arrows in every direction");
		}
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.ranged = true;
			Item.width = 46;
			Item.height = 20;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = mod.ProjectileType("Starshot");
			Item.shootSpeed = 24f;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.crit = 4;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("StarshotCrossbow"), 0, 0, player.whoAmI, 0, type);
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Starshot"), damage, knockBack, player.whoAmI, 0, type);
			return false;
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarlightAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}