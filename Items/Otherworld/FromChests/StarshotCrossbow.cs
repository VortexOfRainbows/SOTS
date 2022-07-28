using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.Furniture;
using Terraria;
using Terraria.DataStructures;
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
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshot Crossbow");
			Tooltip.SetDefault("Fire a star that scatters arrows in every direction");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.DamageType = DamageClass.Ranged;
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
			Item.shoot = ModContent.ProjectileType<Projectiles.Otherworld.Starshot>();
			Item.shootSpeed = 24f;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.crit = 4;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Otherworld.StarshotCrossbow>(), 0, 0, player.whoAmI, 0, type);
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Otherworld.Starshot>(), damage, knockback, player.whoAmI, 0, type);
			return false;
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<StarlightAlloy>(12).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}