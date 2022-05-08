using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Lux;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Projectiles.Chaos
{
	public class ChaosSpiritChain : ModProjectile
	{
		private float aiCounter
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}
		private float aiCounter2
		{
			get => projectile.ai[1];
			set => projectile.ai[1] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Chain");
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 38;
			projectile.height = 38;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 1200;
			projectile.netImportant = true;
			projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindNPCs.Add(index);
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if (center != Vector2.Zero)
			{
				Vector2 ToOwner = projectile.Center - center;
				float dist = ToOwner.Length();
				int max = 12;
				for(int i = 0; i < max; i++)
				{
					float mult = (float)i / max;
					Vector2 drawPos = Vector2.Lerp(projectile.Center, center, mult) - Main.screenPosition;
					float droop = (float)Math.Sin(MathHelper.ToRadians(210 * (float)i / max));
					drawPos.Y += droop * (64 - dist * 0.1f);
					for (int k = 0; k < 6; k++)
					{
						Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60)) * 0.5f * ((float)i / max);
						color.A = 0;
						Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
						Main.spriteBatch.Draw(texture, drawPos + circular, null, color, 0, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		bool runOnce = true;
		Vector2 center = Vector2.Zero;
		public override void AI()
		{
			if(aiCounter >= 0)
			{
				NPC owner = Main.npc[(int)aiCounter];
				if (owner.active && owner.type == ModContent.NPCType<Lux>())
				{
					center = owner.Center;
				}
				else
				{
					projectile.Kill();
					return;
				}
			}
			else
            {
				projectile.Kill();
				return;
			}
			if (runOnce)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 96, 1.4f, -0.4f);
				runOnce = false;
			}
			aiCounter2++;
			if(aiCounter2 > 20)
            {
				projectile.velocity *= 0.6f;
            }
			if(projectile.velocity.Length() < 1f)
            {
				projectile.velocity *= 0;
            }
			else if(aiCounter < 30)
			{
				for (float i = 0; i < 1; i += 0.5f)
				{
					Dust dust2 = Dust.NewDustPerfect(projectile.Center, ModContent.DustType<Dusts.CopyDust4>(), Main.rand.NextVector2Circular(2f, 2f));
					dust2.velocity += projectile.velocity * 0.4f;
					dust2.noGravity = true;
					dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(Main.rand.NextFloat(360)), true);
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 2.5f;
				}
			}
		}
	}
}