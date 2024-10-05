using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.NPCs;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class SupernovaScatter : ModProjectile
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(hasHit);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			hasHit = reader.ReadBoolean();
        }
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supernova Scatter");
		} 
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.extraUpdates = 4;
			Projectile.timeLeft = 900;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
		}
        Vector2[] trailPos = new Vector2[60];
		public void cataloguePos()
		{
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerUpdate();
			return false;
		}
		public void TrailPreDraw()
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * 0.75f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = ColorHelper.Pastel(MathHelper.ToRadians(Projectile.ai[1] + k * 2), true);
				color.A = 0;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (texture.Width * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					if (trailPos[k] != Projectile.Center)
						Main.spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
				}
				previousPosition = currentPos;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			TrailPreDraw();
			return false;
		}
        private bool hasHit = false;
        private bool runOnce = true;
		public override void AI()
		{
			cataloguePos();
			if (runOnce)
			{
				runOnce = false;
				//Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 60, 0.8f, -0.1f);
			}
			if(Main.rand.NextBool(40) || (hasHit && Main.rand.NextBool(8)))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				dust.color = ColorHelper.Pastel(MathHelper.ToRadians(Projectile.ai[1]), true);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 1.3f;
				dust.velocity *= 0.2f;
				dust.alpha = 125;
			}
			if (!Projectile.velocity.Equals(new Vector2(0, 0)))
				Projectile.rotation = Projectile.velocity.ToRotation();
			if (hasHit)
			{
				if (Projectile.timeLeft > 60)
					Projectile.timeLeft = 60;
			}
			else
			{
				float sin = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 1.1f)) * 0.25f;
				Projectile.Center += new Vector2(0, sin).RotatedBy(Projectile.velocity.ToRotation());
				Projectile.ai[1]++;
				int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 640);
				if (target >= 0)
                {
					NPC npc = Main.npc[target];
					if (npc.CanBeChasedBy())
					{
						Vector2 toNPC = npc.Center - Projectile.Center;
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * 5f, 0.01f);
					}
					else
						triggerUpdate();
				}
			}
			if (Projectile.timeLeft < 60 && !hasHit) 
				triggerUpdate();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
			triggerUpdate();
		}
		public void triggerUpdate()
		{
			hasHit = true;
			Projectile.velocity *= 0;
			Projectile.friendly = false;
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				//Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<VibrantRing>(), Projectile.damage, Projectile.knockBack * 0.1f, Main.myPlayer);
			}
		}
	}
}