using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using SOTS.Void;
using SOTS.Dusts;
using SOTS.Helpers;

namespace SOTS.Projectiles.Earth
{
	public abstract class VibrantAmmo : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/VibrantArrowTrail");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 lastPosition = Projectile.Center;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float scale = 1f - 0.5f * (k / (float)Projectile.oldPos.Length);
				Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width / 2, Projectile.height / 2);
				Color color = new Color(100, 100, 100, 0) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				float lengthTowards = Vector2.Distance(lastPosition, drawPos) / texture.Height / scale;
				Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color * scale, Projectile.rotation, drawOrigin, new Vector2(trailScale(), lengthTowards) * scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				lastPosition = drawPos;
			}
			return true;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.4f) * (1 - Projectile.alpha / 255f);
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.type == ModContent.ProjectileType<VibrantArrow>())
			{
				if (Main.myPlayer == Projectile.owner)
				{
					for (int i = -1; i <= 1; i++)
					{
						Vector2 circular = new Vector2(0, 7).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(2f, 10f) * i)) * (1 + Main.rand.NextFloat(-0.1f, 0.1f) * i);
						Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center + new Vector2(0, target.height / 2 + 8), circular, ModContent.ProjectileType<VibrantShard>(), (int)(Projectile.damage * 1.2f), Projectile.knockBack, Projectile.owner);
					}
				}
			}
			Projectile.friendly = false;
			off = true;
			Projectile.netUpdate = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.type == ModContent.ProjectileType<VibrantArrow>())
			{
				if (Main.myPlayer == Projectile.owner)
				{
					for (int i = -1; i <= 1; i++)
					{
						Vector2 circular = new Vector2(0, 7).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(2f, 10f) * i)) * (1 + Main.rand.NextFloat(-0.1f, 0.1f) * i);
						Projectile.NewProjectile(Projectile.GetSource_FromThis("SOTS:VibrantArrowTileCollide"), Projectile.Center + new Vector2(0, 12), circular, ModContent.ProjectileType<VibrantShard>(), (int)(Projectile.damage * 1.2f), Projectile.knockBack, Projectile.owner);
					}
				}
			}
			Projectile.friendly = false;
			off = true;
			Projectile.netUpdate = true;
			return false;
		}
		bool runOnce = true;
		public sealed override bool PreAI()
		{
			if (off)
			{
				if (Projectile.timeLeft > 14)
					Projectile.timeLeft = 14;
				if(runOnce && Projectile.alpha < 100)
				{
					for(int i=  0; i < (Projectile.width + Projectile.height) / 6; i++)
                    {
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, ModContent.DustType<CopyDust4>());
						Color color2 = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(360));
						dust.color = color2;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.2f;
						dust.alpha = Projectile.alpha;
						dust.velocity *= 0.9f;
						dust.velocity += Projectile.velocity * 0.5f;
					}
					if (Projectile.type != ModContent.ProjectileType<VibrantShard>())
						SOTSUtils.PlaySound(SoundID.Item27, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f, -0.2f);
					runOnce = false;
				}
				Projectile.friendly = false;
				Projectile.alpha = 255;
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
				return false;
			}
			else if(Main.rand.NextBool(7) && Projectile.friendly && Projectile.alpha < 100)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Color color2 = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(360));
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 1.1f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.3f;
				dust.velocity -= Projectile.velocity * 0.1f;
			}
			return true;
		}
	}

	public class VibrantArrow : VibrantAmmo
	{
        public override void SetDefaults()
		{
			Projectile.CloneDefaults(1);
			AIType = 1;
			Projectile.width = 26;
			Projectile.height = 56;
			Projectile.hide = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.light = 0;
			Projectile.extraUpdates = 1;
		}
        public override float trailScale()
        {
			return 1.2f;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)Projectile.Center.X - 12, (int)Projectile.Center.Y - 12, 24, 24);
			base.ModifyDamageHitbox(ref hitbox);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true;
			width = 12;
			height = 12;
			return true;
		}
		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.hide = false;
			Projectile.scale = 0.9f;
			Projectile.velocity.Y += 0.03f;
		}
	}
	public class VibrantBullet : VibrantAmmo
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.width = 14;
			Projectile.height = 36;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.light = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)Projectile.Center.X - 8, (int)Projectile.Center.Y - 8, 16, 16);
			base.ModifyDamageHitbox(ref hitbox);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return true;
		}
		float aiCounter = 400;
		public override void AI()
		{
			Projectile.scale = 0.9f;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (Projectile.alpha > 0)
				Projectile.alpha -= 20;
			else
				Projectile.alpha = 0;
			aiCounter -= 8 + Projectile.velocity.Length();
			if(aiCounter <= 0 && !off)
            {
				off = true;
				if(Main.myPlayer == Projectile.owner)
                {
					for(int i = -1; i <= 1; i++)
                    {
						Vector2 circular = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(3f, 13f) * i)) * (1 + Main.rand.NextFloat(-0.3f, -0.1f) * Math.Abs(i));
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<VibrantShard>(), (int)(Projectile.damage * (1 - (float)Math.Abs(i) * 0.75f)), Projectile.knockBack, Projectile.owner);
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
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.width = 10;
			Projectile.height = 20;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.light = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)Projectile.Center.X - 12, (int)Projectile.Center.Y - 12, 24, 24);
			base.ModifyDamageHitbox(ref hitbox);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 4;
			height = 4;
			return true;
		}
		public override void AI()
		{
			Projectile.scale = 1.1f;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (Projectile.alpha > 0)
				Projectile.alpha -= 50;
			else
				Projectile.alpha = 0;
		}
	}
}
		
			