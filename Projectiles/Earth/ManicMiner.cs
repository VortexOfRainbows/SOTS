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
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Earth/ManicMinerTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 lastPosition = Projectile.Center;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float scale = 1f - 0.5f * (k / (float)Projectile.oldPos.Length);
				Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width / 2, Projectile.height / 2);
				Color color = new Color(100, 100, 100, 0);
				float lengthTowards = Vector2.Distance(lastPosition, drawPos) / texture.Height / scale;
				if(Projectile.oldPos[k] != Projectile.position)
				for (int j = 0; j < 4; j++)
				{
					Vector2 offset = new Vector2(2, 0).RotatedBy(j * MathHelper.PiOver2);
					spriteBatch.Draw(texture, drawPos - Main.screenPosition + offset, null, color * scale * 0.8f, Projectile.rotation + MathHelper.PiOver2, drawOrigin, new Vector2(0.5f, lengthTowards) * scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
				lastPosition = drawPos;
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (Projectile.timeLeft < 40)
            {
				return;
            }
				Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = new Vector2(2, 0).RotatedBy(k * MathHelper.PiOver2);
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + offset, null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation + MathHelper.PiOver4, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Mining Laser");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.timeLeft = 80;
			Projectile.extraUpdates = 2;
		}
		bool runOnce = true;
		public override void AI()
		{
			if(Projectile.timeLeft < 40)
            {
				if(Projectile.velocity.Length() > 0)
				{
					for (int i = 0; i < 10; i++)
					{
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
						dust.velocity *= 0.8f;
						dust.noGravity = true;
						dust.color = VoidPlayer.EarthColor;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.2f;
						dust.alpha = Projectile.alpha;
					}
				}			
				Projectile.velocity *= 0f;
            }
			else
			{
				if (runOnce)
				{
					SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/StarLaser"), 0.6f, -0.1f + Main.rand.NextFloat(-0.1f, 0.1f));
					for (int i = 0; i < 13; i++)
					{
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
						dust.velocity *= 0.5f;
						dust.velocity += Projectile.velocity * 1.2f;
						dust.noGravity = true;
						dust.color = VoidPlayer.EarthColor;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.2f;
						dust.alpha = Projectile.alpha;
					}
					runOnce = false;
				}
				else if (Main.rand.NextBool(4))
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
					dust.velocity *= 0.8f;
					dust.noGravity = true;
					dust.color = VoidPlayer.EarthColor;
					dust.fadeIn = 0.1f;
					dust.scale = 1.0f;
					dust.alpha = Projectile.alpha;
				}
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (Main.myPlayer == Projectile.owner)
					HitTiles();
			}
		}
		public void HitTiles()
		{
			Player player = Main.player[Projectile.owner];
			int i = (int)Projectile.Center.X / 16;
			int j = (int)Projectile.Center.Y / 16;
			player.PickTile(i, j, 30);
		}
    }
}
		
			