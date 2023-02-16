using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using SOTS.Projectiles.Otherworld;
using SOTS.Items.Earth;
using SOTS.Items.Otherworld.Furniture;

namespace SOTS.Items.Otherworld.FromChests
{
	public class StarcoreAssaultRifle : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 23;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 70;
            Item.height = 30;
            Item.useTime = 5; 
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 2f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StarcoreBullet>(); 
            Item.shootSpeed = 4f;
			Item.reuseDelay = 10;
			Item.noUseGraphic = true;
		}
		int projectileNum = 0;
		int highestProjectileNum = 0;
		public override bool BeforeUseItem(Player player)
		{
			projectileNum = 0;
			return base.BeforeUseItem(player);
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.player[Main.myPlayer];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarcoreAssaultRifleEffect").Value;
			for (int k = 0; k < 2; k++)
				Main.spriteBatch.Draw(texture, position, frame, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), 0f, origin, scale, SpriteEffects.None, 0f);
			texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarcoreAssaultRifleEffect2").Value;
			for (int k = 0; k < 2; k++)
				Main.spriteBatch.Draw(texture, position, frame, new Color(Main.DiscoG, Main.DiscoB, Main.DiscoR, 0), 0f, origin, scale, SpriteEffects.None, 0f);
			texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarcoreAssaultRifleEffect3").Value;
			for (int k = 0; k < 2; k++)
				Main.spriteBatch.Draw(texture, position, frame, new Color(Main.DiscoB, Main.DiscoR, Main.DiscoG, 0), 0f, origin, scale, SpriteEffects.None, 0f);
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarcoreAssaultRifleEffect").Value;
			for (int k = 0; k < 2; k++)
				Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), rotation, new Vector2(texture.Width/2, texture.Height/2), scale, SpriteEffects.None, 0f);
			texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarcoreAssaultRifleEffect2").Value;
			for (int k = 0; k < 2; k++)
				Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, new Color(Main.DiscoG, Main.DiscoB, Main.DiscoR, 0), rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
			texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarcoreAssaultRifleEffect3").Value;
			for (int k = 0; k < 2; k++)
				Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, new Color(Main.DiscoB, Main.DiscoR, Main.DiscoG, 0), rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);

			texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarcoreAssaultRifleGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }
        public override int GetVoid(Player player)
		{
			return  6;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item11, position);
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians((velocity.X < 0 ? -1 : 1) * (-3f + 1.75f * projectileNum)));
			Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<StarcoreRifle>(), Item.useTime + (projectileNum >= highestProjectileNum - 1 && highestProjectileNum > 3 ? Item.reuseDelay - 1 : 0) + 1, 0, player.whoAmI, perturbedSpeed.ToRotation() - velocity.ToRotation());
			velocity = perturbedSpeed;
			position += velocity * 6;

			projectileNum++;
			if(highestProjectileNum < projectileNum)
				highestProjectileNum = projectileNum;
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<VibrantPistol>(1).AddIngredient<VibrancyModule>(1).AddIngredient<StarlightAlloy>(12).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}
