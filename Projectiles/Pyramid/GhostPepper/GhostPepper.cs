using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SOTS.Projectiles.Pyramid.GhostPepper
{
	public class GhostPepper : ModProjectile
	{
		public static bool IsAlternate => SOTSWorld.GoldenAppleSolved;
		private string AltText => IsAlternate? "Alt" : "";
		public override void SetStaticDefaults()
		{
			Main.projPet[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 7;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 36;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
		}
		private Vector2[] trailPos = new Vector2[13];
        private bool runOnce = true;
        public override bool PreAI()
		{
			if (Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 20;
			if (runOnce)
			{
				Projectile.ai[1] = 80f;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			return base.PreAI();
		}
		public void cataloguePos()
		{
			float curve = 80 * MathF.Sin(MathHelper.ToRadians(Projectile.ai[0]));
			Vector2 toRotate = new Vector2(0, 2.5f).RotatedBy(MathHelper.ToRadians(curve));
			Vector2 current = Projectile.Center + new Vector2(0, 8f).RotatedBy(Projectile.rotation) + Projectile.velocity;
			Vector2 velo = Projectile.velocity * 0.2f;
			velo.Y += 0.65f;
			velo += toRotate;
			velo = velo.RotatedBy(Projectile.rotation);
			velo.Y += 0.8f;
			for (int i = 0; i < trailPos.Length; i++)
			{
				if(trailPos[i] != Vector2.Zero)
					trailPos[i] += velo * (trailPos.Length - i)/(float)trailPos.Length;
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return true;
			Color color = IsAlternate ? new Color(60, 60, 200, 0) : new Color(75, 65, 75, 0);
			Texture2D texture3 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepper" + AltText).Value;
			Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			int frames = IsAlternate ? 8 : 7;
			Vector2 drawOrigin1 = new Vector2(texture3.Width * 0.5f, texture3.Height * 0.5f / frames);
			Rectangle frame = new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height);
            float dist = IsAlternate ? 2f : 3.0f;
			for (int j = 0; j < 6; j++)
			{
				Vector2 circular = new Vector2(dist, 0).RotatedBy(j * MathHelper.Pi / 3f + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
				Main.EntitySpriteDraw(texture3, drawPos2 + circular, frame, color, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
            Main.EntitySpriteDraw(texture3, drawPos2, frame, IsAlternate ? Color.White : lightColor, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
        }
        public override void PostDraw(Color lightColor)
		{
			if (runOnce)
				return;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepperTail" + AltText).Value;
			Color color = IsAlternate ? new Color(35, 35, 40, 0) : new Color(45, 40, 60, 0);
            Vector2 previous = Projectile.Center + new Vector2(0, 8f).RotatedBy(Projectile.rotation);
            for (int i = 0; i < trailPos.Length; i++)
            {
                if (trailPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)trailPos.Length;
                Vector2 center = trailPos[i];
                Vector2 toPrev = previous - center;
                Main.EntitySpriteDraw(texture2, center - Main.screenPosition, null, color * perc * 4f, toPrev.ToRotation() - MathHelper.PiOver2, new Vector2(texture2.Width / 2, 0), new Vector2(1f * perc, toPrev.Length() / 10f), Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                previous = center;
            }
            Texture2D eyeGlow = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepperGlow" + AltText).Value;
			Texture2D tFront = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepperFront" + AltText).Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepperGlowmask").Value;
			Vector2 drawOrigin1 = new Vector2(eyeGlow.Width * 0.5f, eyeGlow.Height * 0.5f);
			Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			Main.EntitySpriteDraw(tFront, drawPos2, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), lightColor, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			color = IsAlternate ? new Color(35, 35, 40, 0) * 0.5f : new Color(40, 25, 35, 0);
            if (IsAlternate)
				Main.EntitySpriteDraw(eyeGlow, drawPos2, null, Color.White, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			for (int j = 0; j < 8; j++)
            {
				Vector2 circular = new Vector2(0.5f, 0).RotatedBy(j * MathHelper.PiOver4);
				Main.EntitySpriteDraw(eyeGlow, drawPos2 + circular, null, color, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			if(!IsAlternate)
				Main.EntitySpriteDraw(texture4, drawPos2, null, Color.White, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return false;
		}
		private int mode = 0; //0 = follow, 1 = find souls, 2 = targetting
		public float cooldown = 30;
		public int npcTargetId = -1;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (mode == 0)
			{
				bool hasHoloEye = modPlayer.HoloEye;
				Projectile.velocity *= 0.625f;
				Vector2 toLocation = player.Center - new Vector2(0, 64 + (hasHoloEye ? 48 : 0) - player.gfxOffY);
				Vector2 goTo = toLocation - Projectile.Center;
				Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
				float dist = 1.5f + goTo.Length() * 0.01f;
				if (dist > goTo.Length())
					dist = goTo.Length();
				Projectile.velocity += newGoTo * dist;
			}
			cooldown++;
			int projId = -1;
			float lastLength = 2000f;
			Projectile.velocity *= 0.94f;
			if(mode != 2)
			{
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.type == ModContent.ProjectileType<SoulofLooting>() && proj.active && (int)proj.ai[0] == Projectile.owner)
					{
						Vector2 toNPC = proj.Center - Projectile.Center;
						if (toNPC.Length() < lastLength)
						{
							lastLength = toNPC.Length();
							projId = proj.whoAmI;
						}
					}
				}
				if (projId != -1)
				{
					mode = 1;
					Projectile proj = Main.projectile[projId];
					if (proj.active)
					{
						cooldown = 0;
						Vector2 toLocation = proj.Center;
						Vector2 goTo = toLocation - Projectile.Center;
						Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
						float dist = 0.85f;
						if (dist > goTo.Length())
							dist = goTo.Length();
						Projectile.velocity += newGoTo * dist;
					}
				}
				else if(cooldown > 20)
				{
					mode = 0;
				}
			}

			if(npcTargetId != -1)
            {
				NPC npc = Main.npc[npcTargetId];
				if (npc.active)
				{
					mode = 2;
					Projectile.Center = npc.Center + new Vector2(0, -npc.height/2 - 48);
					if(cooldown >= 60)
                    {
						mode = 0;
						npcTargetId = -1;
                    }
				}
				else
                {
					npcTargetId = -1;
					mode = 0;
				}
            }
			float inc = 2.75f;
			Projectile.ai[0] += inc;
			float curve = 8 * MathF.Sin(MathHelper.ToRadians(Projectile.ai[0]));
			Vector2 toRotate = new Vector2(0, 5).RotatedBy(MathHelper.ToRadians(curve));
			Projectile.rotation = toRotate.ToRotation() - MathHelper.ToRadians(90);
			Projectile.rotation += Projectile.velocity.X * 0.02f;
			#region Animation and visuals
			if(mode != 2 || Projectile.ai[0] % (inc * 3) == 0)
				cataloguePos();
			Projectile.frameCounter++;
			if(Projectile.frameCounter > 5)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if(Projectile.frame >= (IsAlternate ? 8 : 7))
				{
					Projectile.frame = 0;
                }
            }
			Lighting.AddLight(Projectile.Center, new Color(75, 30, 75).ToVector3());
			#endregion
		}
	}
}