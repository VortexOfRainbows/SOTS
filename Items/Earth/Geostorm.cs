using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Earth;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Earth
{
	public class Geostorm : VoidItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/GeostormGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geostorm");
			Tooltip.SetDefault("Bombards your cursor with crystals");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Magic;
			Item.width = 26;
			Item.height = 38;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<GeostormCrystal>();
            Item.shootSpeed = 5.5f; //arbitrary
			Item.noMelee = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = glowTexture;
			}
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 cursorPos = Main.MouseWorld;
			Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, 0, 0, type, damage, knockback, player.whoAmI, -1);
			return false;
		}
		public override int GetVoid(Player player)
		{
			return 15;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VibrantBar>(), 8).AddTile(TileID.Anvils).Register();
		}
	}
}