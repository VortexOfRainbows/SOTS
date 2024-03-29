using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;
using SOTS.Projectiles.Laser;
using SOTS.Items.Inferno;
using SOTS.Items.Fragments;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Evil
{
	public class AbyssalFury : VoidItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Evil/AbyssalFuryGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 68;
			Item.height = 68;
			Item.useTime = 11;
			Item.useAnimation = 33;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<LightspeedBlade>(); 
            Item.shootSpeed = 7f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = glowTexture;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Items.Earth.VibrantBlade>(1).AddIngredient<ObsidianEruption>(1).AddIngredient(ItemID.Starfury, 1).AddIngredient(ItemID.EnchantedSword, 1).AddIngredient<DissolvingUmbra>(1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override int GetVoid(Player player)
		{
			return 12;
		}
		int rotate = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 1;  //This defines how many projectiles to shot
            for (int index = 0; index < numberProjectiles; ++index)
			{
				rotate += Main.rand.Next(45);
				Vector2 spawnPos = new Vector2((float)(player.Center.X + (200f * -player.direction * Main.rand.Next(8,13)) + (Main.mouseX + Main.screenPosition.X - player.position.X)), player.Center.Y - 26f * Main.rand.Next(8, 13));
				spawnPos.Y += 0.5f * (player.Center.Y - Main.MouseWorld.Y);
				Vector2 circularPos = new Vector2(Main.rand.Next(64, 196 + 1), 0).RotatedBy(MathHelper.ToRadians(rotate));
				spawnPos += circularPos;
				Vector2 speed = Main.MouseWorld - spawnPos;
				speed.Normalize();
				speed *= velocity.Length();

				Projectile.NewProjectile(source, spawnPos.X, spawnPos.Y, speed.X, speed.Y, type, damage, knockback, Main.myPlayer, 0.0f, Main.MouseWorld.X);
            }
            return false;
		}
	}
}