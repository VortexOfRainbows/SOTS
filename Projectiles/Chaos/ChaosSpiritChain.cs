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
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private float aiCounter2
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Chain");
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 1200;
			Projectile.netImportant = true;
			Projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCs.Add(index);
		}
        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if (center != Vector2.Zero)
			{
				Vector2 ToOwner = Projectile.Center - center;
				float dist = ToOwner.Length();
				int max = 12;
				for(int i = 0; i < max; i++)
				{
					float mult = (float)i / max;
					Vector2 drawPos = Vector2.Lerp(Projectile.Center, center, mult) - Main.screenPosition;
					float droop = (float)Math.Sin(MathHelper.ToRadians(210 * (float)i / max));
					drawPos.Y += droop * (64 - dist * 0.1f);
					for (int k = 0; k < 6; k++)
					{
						Color color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(k * 60)) * 0.5f * ((float)i / max);
						color.A = 0;
						Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
						Main.spriteBatch.Draw(texture, drawPos + circular, null, color, 0, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
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
					Projectile.Kill();
					return;
				}
			}
			else
            {
				Projectile.Kill();
				return;
			}
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item96, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.4f, -0.4f);
				runOnce = false;
			}
			aiCounter2++;
			if(aiCounter2 > 20)
            {
				Projectile.velocity *= 0.6f;
            }
			if(Projectile.velocity.Length() < 1f)
            {
				Projectile.velocity *= 0;
            }
			else if(aiCounter < 30)
			{
				for (float i = 0; i < 1; i += 0.5f)
				{
					Dust dust2 = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.CopyDust4>(), Main.rand.NextVector2Circular(2f, 2f));
					dust2.velocity += Projectile.velocity * 0.4f;
					dust2.noGravity = true;
					dust2.color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(Main.rand.NextFloat(360)), true);
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 2.5f;
				}
			}
		}
	}
}