using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Lux;
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
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Chaos/ChaosSpiritChain");
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
					spriteBatch.Draw(texture, drawPos, null, new Color(100, 100, 100, 0), 0, drawOrigin, projectile.scale, SpriteEffects.None, 0f); 
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
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 96, 1.4f);
				runOnce = false;
			}
			aiCounter2++;
			if(aiCounter2 > 20)
            {
				projectile.velocity *= 0.6f;
            }
			if(projectile.velocity.Length() < 0.2f && projectile.velocity.Length() != 0)
            {
				projectile.velocity *= 0;
            }
		}
	}
}