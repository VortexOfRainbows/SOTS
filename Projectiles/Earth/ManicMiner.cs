using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth 
{    
    public class ManicMiner : ModProjectile
	{
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Earth/ManicMinerTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 lastPosition = projectile.Center;
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				float scale = 1f - 0.5f * (k / (float)projectile.oldPos.Length);
				Vector2 drawPos = projectile.oldPos[k] + new Vector2(projectile.width / 2, projectile.height / 2);
				Color color = new Color(100, 100, 100, 0);
				float lengthTowards = Vector2.Distance(lastPosition, drawPos) / texture.Height / scale;
				if(projectile.oldPos[k] != projectile.position)
				for (int j = 0; j < 4; j++)
				{
					Vector2 offset = new Vector2(2, 0).RotatedBy(j * MathHelper.PiOver2);
					spriteBatch.Draw(texture, drawPos - Main.screenPosition + offset, null, color * scale * 0.8f, projectile.rotation + MathHelper.PiOver2, drawOrigin, new Vector2(0.5f, lengthTowards) * scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
				lastPosition = drawPos;
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (projectile.timeLeft < 40)
            {
				return;
            }
				Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = new Vector2(2, 0).RotatedBy(k * MathHelper.PiOver2);
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + offset, null, color * (1f - (projectile.alpha / 255f)), projectile.rotation + MathHelper.PiOver4, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Mining Laser");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.width = 20;
			projectile.height = 20;
			projectile.timeLeft = 80;
			projectile.extraUpdates = 2;
		}
		bool runOnce = true;
		public override void AI()
		{
			if(projectile.timeLeft < 40)
            {
				if(projectile.velocity.Length() > 0)
				{
					for (int i = 0; i < 10; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
						dust.velocity *= 0.8f;
						dust.noGravity = true;
						dust.color = VoidPlayer.EarthColor;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.2f;
						dust.alpha = projectile.alpha;
					}
				}			
				projectile.velocity *= 0f;
            }
			else
			{
				if (runOnce)
				{
					SoundEngine.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/StarLaser"), 0.6f, -0.1f + Main.rand.NextFloat(-0.1f, 0.1f));
					for (int i = 0; i < 13; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
						dust.velocity *= 0.5f;
						dust.velocity += projectile.velocity * 1.2f;
						dust.noGravity = true;
						dust.color = VoidPlayer.EarthColor;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.2f;
						dust.alpha = projectile.alpha;
					}
					runOnce = false;
				}
				else if (Main.rand.NextBool(4))
				{
					Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
					dust.velocity *= 0.8f;
					dust.noGravity = true;
					dust.color = VoidPlayer.EarthColor;
					dust.fadeIn = 0.1f;
					dust.scale = 1.0f;
					dust.alpha = projectile.alpha;
				}
				projectile.rotation = projectile.velocity.ToRotation();
				if (Main.myPlayer == projectile.owner)
					HitTiles();
			}
		}
		public void HitTiles()
		{
			Player player = Main.player[projectile.owner];
			int i = (int)projectile.Center.X / 16;
			int j = (int)projectile.Center.Y / 16;
			player.PickTile(i, j, 30);
		}
    }
}
		
			