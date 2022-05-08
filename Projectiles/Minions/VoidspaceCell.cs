using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Projectiles.Celestial;
using System.Collections.Generic;
using System;

namespace SOTS.Projectiles.Minions
{
    public class VoidspaceCell : ModProjectile
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidspace Cell");
		}
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 40; 
            projectile.hostile = false; 
            projectile.friendly = false; 
            projectile.ignoreWater = true;  
            Main.projFrames[projectile.type] = 1;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.penetrate = -1;
            projectile.tileCollide = false;
			projectile.netImportant = true;
			projectile.sentry = true;
		}
		List<FireParticle> particleList = new List<FireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
				}
			}
		}
		float sphereRadius = 215f;
		float counter = 0;
		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			float mult = counter / 45f;
			sphereRadius = 105f + (300f * (player.minionDamage + (player.allDamage - 1f))) * mult;
			if(counter < 45)
				counter++;
			cataloguePos();
			Vector2 rotational = new Vector2(0, -1.8f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
			rotational.X *= 0.25f;
			rotational.Y *= 0.75f;
			rotational = rotational.SafeNormalize(Vector2.Zero) * 3f;
			particleList.Add(new FireParticle(projectile.Center - rotational * 2, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.9f, 1.1f)));
			for(int i = 0; i < 360; i++)
			{
				Vector2 circular = new Vector2(sphereRadius, 0).RotatedBy(MathHelper.ToRadians(i));
				rotational = new Vector2(0, -1.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
				if (Main.rand.NextBool(40))
				{
					int i2 = (int)(circular.X + projectile.Center.X) / 16;
					int j2 = (int)(circular.Y + projectile.Center.Y) / 16;
					bool disable = false;
					if (!WorldGen.InWorld(i2, j2, 20) || Main.tile[i2, j2].active() && Main.tileSolidTop[Main.tile[i2, j2].type] == false && Main.tileSolid[Main.tile[i2, j2].type] == true)
						disable = true;
					if(!disable)
						for(int j = 0; j < Main.maxProjectiles; j++)
						{
							Projectile other = Main.projectile[j];
							if(other.active && other.type == projectile.type && other.owner == projectile.owner && j != projectile.whoAmI)
							{
								Vector2 distanceBetween = projectile.Center + circular - other.Center;
								if (distanceBetween.Length() < sphereRadius)
								{
									disable = true;
									break;
								}
							}
						}
					if(!disable)
						particleList.Add(new FireParticle(projectile.Center + circular - rotational * 2, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.7f, 0.9f)));
				}
			}
			return base.PreAI();
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if(chains)
				DrawChain((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, spriteBatch);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = new Color(75, 255, 30, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.15f, SpriteEffects.None, 0f);
				}
			}
		}
		public void DrawChain(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Minions/VoidspaceAuraChain").Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Color color = new Color(255, 255, 255) * 0.5f;
			float height = 16;
			float timer = Main.GlobalTime * 40;
			int maxLength = 20;
			for (int j2 = 1; j2 < maxLength; j2++)
			{
				Tile tile2 = Framing.GetTileSafely(i, j - j2);
				if ((tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type]) || !WorldGen.InWorld(i, j - j2, 27))
				{
					maxLength = j2;
					break;
				}
			}
			maxLength++;
			Vector2 previous = projectile.Center;
			for (int z = 0; z < maxLength; z++)
			{
				float dynamicMult = 0.52f + 0.48f * (float)Math.Cos(MathHelper.ToRadians(180f * z / maxLength));
				Vector2 dynamicAddition = new Vector2(20f / maxLength * z * 0.4f + 0.5f, 0).RotatedBy(MathHelper.ToRadians(z * 24 + timer)) * dynamicMult;
				Vector2 pos = projectile.Center;
				pos.Y -= z * 16;
				pos += dynamicAddition;
				Vector2 rotateTo = pos - previous;
				float lengthTo = rotateTo.Length();
				float stretch = (lengthTo / height) * 1.00275f;
				if (z == 0)
					stretch = 1f;
				Vector2 scaleVector2 = new Vector2(0.8f, stretch);
				if (z != 0)
				{
					float alphaScale = (32f - z * 1.575f) / 20f;
					Main.spriteBatch.Draw(texture, previous - Main.screenPosition, null, color * alphaScale, rotateTo.ToRotation() + MathHelper.ToRadians(90), origin, scaleVector2, SpriteEffects.None, 0f);
				}
				previous = pos;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) 
		{
			//SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			//spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Gores/CircleAura").Value, projectile.Center - Main.screenPosition, null, new Color(0, 20, 0, 0), 0f, new Vector2(300f, 300f), sphereRadius / 300f, SpriteEffects.None, 0f);
			//spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Gores/CircleBorder").Value, projectile.Center - Main.screenPosition, null, new Color(5, 30, 5, 0), 0f, new Vector2(300f, 300f), sphereRadius / 300f, SpriteEffects.None, 0f);
			return true;
		}
		bool runOnce = true;
		bool chains = true;
		Vector2 ogPosition;
        public override void AI()
		{
			if (runOnce)
            {
				ogPosition = projectile.Center;
				runOnce = false;
            }
			if(!projectile.Center.Equals(ogPosition))
            {
				chains = false;

			}
			Main.player[projectile.owner].UpdateMaxTurrets();
			if(projectile.owner == Main.myPlayer)
				projectile.netUpdate = true;
			Lighting.AddLight(projectile.Center, sphereRadius / 510f, sphereRadius / 510f, sphereRadius / 510f);
			projectile.ai[0]--;
			if (projectile.ai[0] <= 0)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC target = Main.npc[i];
					if ((projectile.Center - target.Center).Length() <= sphereRadius + 4f && !target.friendly && target.active)
					{
						if (Main.myPlayer == projectile.owner)
						{
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<VoidspaceExplosion>(), projectile.damage, projectile.knockBack, projectile.owner);
						}
					}
				}
				projectile.ai[0] = 45;
			}
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
				if((projectile.Center - target.Center).Length() <= sphereRadius + 4f && target.active)
				{
					target.AddBuff(mod.BuffType("AuraBoost"), 330, false);
				}
            }
        }
    }
}