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
			// DisplayName.SetDefault("Curse");
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 140;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.alpha = 0;
			Projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCs.Add(index);
        }
        List<Vector2> posList = new List<Vector2>();
		public void Laser(float scale = 1f)
		{
			posList = new List<Vector2>();
			int parentID = (int)Projectile.ai[1];
			NPC npc = Main.npc[parentID];
			Vector2 current = Projectile.Center;
			Vector2 velo = Projectile.velocity.SafeNormalize(Vector2.Zero);
			if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
			{
				int maxLength = (int)(60 * scale);
				for (int i = 0; i <= maxLength; i++)
				{
					if (Main.rand.NextBool(6))
					{
						float scaleMult = 0.25f + 0.85f * ((maxLength - i) / (float)maxLength) * (0.7f + 0.3f * scale);
						PharaohsCurse curse = npc.ModNPC as PharaohsCurse;
						Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(0.75f + 1.75f * scaleMult)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						curse.foamParticleList1.Add(new CurseFoam(current, rotational, 0.4f + 0.6f * scaleMult, true));
					}
					posList.Add(current);
					current += velo * 4.5f;
				}
			}
		}
		bool runOnce = false;
		public override void AI()
		{
			Projectile.rotation += MathHelper.ToRadians(8);
			if (Projectile.ai[0] == 0)
			{
				bool capable = false;
				for(int i = 0; i < Main.maxPlayers; i++)
                {
					Player player = Main.player[i];
					if(Vector2.Distance(player.Center, Projectile.Center) <= 416)
                    {
						capable = true;
						break;
                    }
                }
				if(!capable)
				{
					Projectile.Kill();
					return;
                }
			}
			int parentID = (int)Projectile.ai[1];
			NPC npc = Main.npc[parentID];
			if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
			{
				Projectile.hostile = true;
				if (Projectile.ai[0] < 40)
				{
					float scale = Projectile.ai[0] * 0.0075f;
					Laser(scale);
				}
				else if (Projectile.ai[0] == 40)
				{
					if (Main.netMode != NetmodeID.Server)
					{
						Vector2 distanceToOwner = Projectile.Center - npc.Center;
						PharaohsCurse curse = npc.ModNPC as PharaohsCurse;
						for (int j = 0; j < 40; j++)
						{
							Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(2.75f, 3.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
							curse.foamParticleList1.Add(new CurseFoam(Projectile.Center, rotational, 1.55f, true));
						}
						runOnce = true;
					}
				}
				else if (Projectile.ai[0] > 40 && Projectile.ai[0] <= 70)
				{
					float current = Projectile.ai[0] - 40f;
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
				else if(Projectile.ai[0] > 70)
                {
					Projectile.Kill();
                }
				if (runOnce)
				{
					SOTSUtils.PlaySound(SoundID.Item73, (int)npc.Center.X, (int)npc.Center.Y, 1.3f, 0.3f);
					runOnce = false;
					//Projectile.friendly = true;
				}
			}
			else
            {
				Projectile.Kill();
			}
			Projectile.ai[0]++;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			int counter = posList.Count;
			if (Projectile.friendly || Projectile.hostile)
				for (int i = 0; i < posList.Count; i += 8)
				{
					counter--;
					float scale = 0.5f + 0.5f * ((float)counter / posList.Count);
					int width = (int)(32 * scale);
					Rectangle rect = new Rectangle((int)posList[i].X - width/2, (int)posList[i].Y - width/2, width, width);
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
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}