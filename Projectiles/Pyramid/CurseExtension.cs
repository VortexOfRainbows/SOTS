using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;
using SOTS.Projectiles.Celestial;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{
	public class CurseExtension : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.timeLeft = 180;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 0;
			projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindNPCs.Add(index);
        }
        List<Vector2> posList = new List<Vector2>();
		public void Laser(float scale = 1f)
		{
			posList = new List<Vector2>();
			int parentID = (int)projectile.ai[1];
			NPC npc = Main.npc[parentID];
			Vector2 current = projectile.Center;
			Vector2 velo = projectile.velocity.SafeNormalize(Vector2.Zero);
			if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
			{
				int maxLength = (int)(90 * scale);
				for (int i = 0; i <= maxLength; i++)
				{
					if (Main.rand.Next(5) <= 1)
					{
						float scaleMult = 0.15f + 0.85f * ((maxLength - i) / (float)maxLength) * (0.7f + 0.3f * scale);
						PharaohsCurse curse = npc.modNPC as PharaohsCurse;
						Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(1.25f + 2.25f * scaleMult)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						curse.foamParticleList1.Add(new CurseFoam(current, rotational, 0.5f + 0.5f * scaleMult, true));
					}
					posList.Add(current);
					current += velo * 3f;
				}
			}
		}
		bool runOnce = false;
		public override void AI()
		{
			projectile.rotation += MathHelper.ToRadians(8);
			if (projectile.ai[0] == 0)
			{
				//Main.PlaySound(SoundID.Item, (int)(projectile.Center.X), (int)(projectile.Center.Y), 15, 1f, 0.25f);
			}
			int parentID = (int)projectile.ai[1];
			NPC npc = Main.npc[parentID];
			if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
			{
				projectile.hostile = true;
				if (projectile.ai[0] < 40)
				{
					float scale = projectile.ai[0] * 0.008f;
					Laser(scale);
				}
				else if (projectile.ai[0] == 40)
				{
					Vector2 distanceToOwner = projectile.Center - npc.Center;
					PharaohsCurse curse = npc.modNPC as PharaohsCurse;
					for (int j = 0; j < 50; j++)
					{
						Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(2.75f, 3.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						curse.foamParticleList1.Add(new CurseFoam(projectile.Center, rotational, 1.55f, true));
					}
					runOnce = true;
				}
				else if (projectile.ai[0] > 40 && projectile.ai[0] <= 70)
				{
					float current = projectile.ai[0] - 40f;
					if (current <= 10)
						current *= 9f;
					else
					{
						current -= 10;
						current *= 4.5f;
						current += 90f;
					}
					float scaleMod = (float)Math.Sin(MathHelper.ToRadians(current));
					float scale = 0.3f + 0.7f * scaleMod;
					Laser(scale);
				}
				else if(projectile.ai[0] > 70)
                {
					projectile.Kill();
                }
				if (runOnce)
				{
					Main.PlaySound(SoundID.NPCHit, (int)projectile.Center.X, (int)projectile.Center.Y, 54, 1.0f, -0.33f);
					runOnce = false;
					//projectile.friendly = true;
				}
			}
			else
            {
				projectile.Kill();
			}
			projectile.ai[0]++;
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.8f / 255f, (255 - projectile.alpha) * 0.8f / 255f, (255 - projectile.alpha) * 0.8f / 255f);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.friendly || projectile.hostile)
				for (int i = 0; i < posList.Count; i += 8)
				{
					Rectangle rect = new Rectangle((int)posList[i].X - 8, (int)posList[i].Y - 8, 16, 16);
					if (targetHitbox.Intersects(rect))
					{
						return true;
					}
				}
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
	}
}