using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Earth
{
	public abstract class VibrantAmmo : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Earth/VibrantArrowTrail");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 lastPosition = projectile.Center;
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				float scale = 1f - 0.5f * (k / (float)projectile.oldPos.Length);
				Vector2 drawPos = projectile.oldPos[k] + new Vector2(projectile.width / 2, projectile.height / 2);
				Color color = new Color(100, 100, 100, 0) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				float lengthTowards = Vector2.Distance(lastPosition, drawPos) / texture.Height / scale;
				spriteBatch.Draw(texture, drawPos - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), null, color * scale, projectile.rotation, drawOrigin, new Vector2(trailScale(), lengthTowards) * scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				lastPosition = drawPos;
			}
			return true;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.4f) * (1 - projectile.alpha / 255f);
        }
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(off);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			off = reader.ReadBoolean();
		}
		public virtual float trailScale() => 1;
		public bool off = false;
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.type == ModContent.ProjectileType<VibrantArrow>())
			{
				if (Main.myPlayer == projectile.owner)
				{
					for (int i = -1; i <= 1; i++)
					{
						Vector2 circular = new Vector2(0, 7).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(2f, 10f) * i)) * (1 + Main.rand.NextFloat(-0.1f, 0.1f) * i);
						Projectile.NewProjectile(target.Center + new Vector2(0, target.height / 2 + 8), circular, ModContent.ProjectileType<VibrantShard>(), (int)(projectile.damage * 1.2f), projectile.knockBack, projectile.owner);
					}
				}
			}
			projectile.friendly = false;
			off = true;
			projectile.netUpdate = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.type == ModContent.ProjectileType<VibrantArrow>())
			{
				if (Main.myPlayer == projectile.owner)
				{
					for (int i = -1; i <= 1; i++)
					{
						Vector2 circular = new Vector2(0, 7).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(2f, 10f) * i)) * (1 + Main.rand.NextFloat(-0.1f, 0.1f) * i);
						Projectile.NewProjectile(projectile.Center + new Vector2(0, 12), circular, ModContent.ProjectileType<VibrantShard>(), (int)(projectile.damage * 1.2f), projectile.knockBack, projectile.owner);
					}
				}
			}
			projectile.friendly = false;
			off = true;
			projectile.netUpdate = true;
			return false;
		}
		bool runOnce = true;
		public sealed override bool PreAI()
		{
			if (off)
			{
				if (projectile.timeLeft > 14)
					projectile.timeLeft = 14;
				if(runOnce && projectile.alpha < 100)
				{
					for(int i=  0; i < (projectile.width + projectile.height) / 6; i++)
                    {
						Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 12, projectile.Center.Y - 12), 16, 16, ModContent.DustType<CopyDust4>());
						Color color2 = VoidPlayer.VibrantColorAttempt(Main.rand.NextFloat(360));
						dust.color = color2;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.2f;
						dust.alpha = projectile.alpha;
						dust.velocity *= 0.9f;
						dust.velocity += projectile.velocity * 0.5f;
					}
					if (projectile.type != ModContent.ProjectileType<VibrantShard>())
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 27, 0.7f, -0.2f);
					runOnce = false;
				}
				projectile.friendly = false;
				projectile.alpha = 255;
				projectile.tileCollide = false;
				projectile.velocity *= 0f;
				return false;
			}
			else if(Main.rand.NextBool(7) && projectile.friendly && projectile.alpha < 100)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Color color2 = VoidPlayer.VibrantColorAttempt(Main.rand.NextFloat(360));
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 1.1f;
				dust.alpha = projectile.alpha;
				dust.velocity *= 0.3f;
				dust.velocity -= projectile.velocity * 0.1f;
			}
			return true;
		}
	}

	public class VibrantArrow : VibrantAmmo
	{
        public override void SetDefaults()
		{
			projectile.CloneDefaults(1);
			aiType = 1;
			projectile.width = 26;
			projectile.height = 56;
			projectile.hide = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.light = 0;
			projectile.extraUpdates = 1;
		}
        public override float trailScale()
        {
			return 1.2f;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)projectile.Center.X - 12, (int)projectile.Center.Y - 12, 24, 24);
			base.ModifyDamageHitbox(ref hitbox);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = true;
			width = 12;
			height = 12;
			return true;
		}
		public override void AI()
		{
			projectile.spriteDirection = projectile.direction;
			projectile.hide = false;
			projectile.scale = 0.9f;
			projectile.velocity.Y += 0.03f;
		}
	}
	public class VibrantBullet : VibrantAmmo
	{
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.width = 14;
			projectile.height = 36;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.light = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)projectile.Center.X - 8, (int)projectile.Center.Y - 8, 16, 16);
			base.ModifyDamageHitbox(ref hitbox);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}
		float aiCounter = 400;
		public override void AI()
		{
			projectile.scale = 0.9f;
			projectile.spriteDirection = projectile.direction;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (projectile.alpha > 0)
				projectile.alpha -= 20;
			else
				projectile.alpha = 0;
			aiCounter -= 8 + projectile.velocity.Length();
			if(aiCounter <= 0 && !off)
            {
				off = true;
				if(Main.myPlayer == projectile.owner)
                {
					for(int i = -1; i <= 1; i++)
                    {
						Vector2 circular = projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(3f, 13f) * i)) * (1 + Main.rand.NextFloat(-0.3f, -0.1f) * Math.Abs(i));
						Projectile.NewProjectile(projectile.Center, circular, ModContent.ProjectileType<VibrantShard>(), (int)(projectile.damage * (1 - (float)Math.Abs(i) * 0.75f)), projectile.knockBack, projectile.owner);
                    }
                }
            }
		}
	}
	public class VibrantShard : VibrantAmmo
	{
		public override float trailScale()
		{
			return 0.8f;
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.width = 10;
			projectile.height = 20;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.light = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)projectile.Center.X - 12, (int)projectile.Center.Y - 12, 24, 24);
			base.ModifyDamageHitbox(ref hitbox);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 4;
			height = 4;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}
		public override void AI()
		{
			projectile.scale = 1.1f;
			projectile.spriteDirection = projectile.direction;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (projectile.alpha > 0)
				projectile.alpha -= 50;
			else
				projectile.alpha = 0;
		}
	}
}
		
			