using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class OtherworldlyTracer : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Otherworldly Tracer");
		}
		public override void SetDefaults()
		{
			projectile.width = 58;
			projectile.height = 58;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.timeLeft = 12000;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 100;
			projectile.ranged = true;
			projectile.ignoreWater = true;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.alpha);
			writer.Write(projectile.timeLeft);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.alpha = reader.ReadInt32();
			projectile.timeLeft = reader.ReadInt32();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			if (projectile.ai[1] == -1 || hasPlayer)
				texture = mod.GetTexture("Projectiles/Otherworld/OtherworldlyTracerAlt");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for(int i = 0; i < 1 + (int)(ai1)/30; i++)
			{
				for (int k = 0; k < 7 - (hasPlayer ? 2 : 0); k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (projectile.alpha / 255f)), rotation[i], drawOrigin, sizes[i], SpriteEffects.None, 0f);
				}
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void Kill(int timeLeft)
		{
			//Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 0.6f);
		}
		bool hasPlayer = false;
		float[] rotation = { 0, 0, 0 };
		float[] rotationSpeed = { 0, 0, 0 };
		float[] sizes = { 0.4f, 0.7f, 1.1f};
		bool doOnce = true;
		int ai1 = 0;
		public override void AI()
		{
			if(projectile.ai[1] == -1)
			{
				hasPlayer = true;
				Lighting.AddLight(projectile.Center, 0.45f * ((255 - projectile.alpha) / 255), 0.4f * ((255 - projectile.alpha) / 255), 0.95f * ((255 - projectile.alpha) / 255));
				if (projectile.alpha < 255)
					projectile.alpha += 3;

				projectile.timeLeft = 6;
				if (Main.myPlayer == projectile.owner)
				{
					projectile.netUpdate = true;
					projectile.position = Main.MouseWorld - new Vector2(projectile.width/2, projectile.height/2);
				}
			}
			else if (projectile.ai[1] >= 0)
			{
				Lighting.AddLight(projectile.Center, 0.95f * ((255 - projectile.alpha)/255), 0.45f * ((255 - projectile.alpha) / 255), 0.95f * ((255 - projectile.alpha)/255));
				NPC owner = Main.npc[(int)projectile.ai[1]];
				if (!owner.active || !(owner.type == mod.NPCType("OtherworldlyConstructHead2") || owner.type == mod.NPCType("OtherworldlyConstructHead") || owner.type == mod.NPCType("TheAdvisorHead")))
				{
					projectile.Kill();
				}
				if (owner.type == mod.NPCType("TheAdvisorHead"))
					projectile.extraUpdates = 1;
			}
			if(projectile.ai[1] == -3)
			{
				if (Main.myPlayer == projectile.owner)
				{
					projectile.netUpdate = true;
				}
				projectile.timeLeft = 30;
				projectile.ai[1]--;
			}
			ai1++;
			if (ai1 == 30)
			{
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 30, 0.3f);
			}
			if (ai1 == 60)
			{
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 30, 0.4f);
			}
			if (ai1 > 61)
			{
				ai1 = 61;
			}
			if(doOnce)
			{
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 30, 0.2f);
				if (projectile.timeLeft > projectile.ai[0])
				{
					projectile.timeLeft = (int)projectile.ai[0];
				}
				doOnce = false;
				for (int i = 0; i < 3; i++)
				{
					rotation[i] = Main.rand.NextFloat(360);
					rotationSpeed[i] = Main.rand.NextFloat(-10, 11);
				}
			}
			for (int i = 0; i < 3; i++)
			{
				rotation[i] += MathHelper.ToRadians(0.5f * rotationSpeed[i]);
			}
			if(projectile.timeLeft < 50 && projectile.ai[1] >= 0)
			{
				projectile.alpha -= 2;
			}
		}
	}
}