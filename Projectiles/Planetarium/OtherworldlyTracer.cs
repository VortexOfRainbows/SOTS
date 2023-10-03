using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Advisor;
using SOTS.NPCs.Constructs;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Planetarium
{
	public class OtherworldlyTracer : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 58;
			Projectile.height = 58;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.timeLeft = 12000;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 100;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.ignoreWater = true;
		}
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.alpha);
			writer.Write(Projectile.timeLeft);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.alpha = reader.ReadInt32();
			Projectile.timeLeft = reader.ReadInt32();
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			if (Projectile.ai[1] == -1 || hasPlayer)
				texture = Mod.Assets.Request<Texture2D>("Projectiles/Planetarium/OtherworldlyTracerAlt").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for(int i = 0; i < 1 + (int)(ai1)/30; i++)
			{
				for (int k = 0; k < 7 - (hasPlayer ? 2 : 0); k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), rotation[i], drawOrigin, sizes[i], SpriteEffects.None, 0f);
				}
			}
		}
		public override void OnKill(int timeLeft)
		{
			//Terraria.Audio.SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 14, 0.6f);
		}
		bool hasPlayer = false;
		float[] rotation = { 0, 0, 0 };
		float[] rotationSpeed = { 0, 0, 0 };
		float[] sizes = { 0.4f, 0.7f, 1.1f};
		bool doOnce = true;
		int ai1 = 0;
		public override void AI()
		{
			if(Projectile.ai[1] == -1)
			{
				hasPlayer = true;
				Lighting.AddLight(Projectile.Center, 0.45f * ((255 - Projectile.alpha) / 255), 0.4f * ((255 - Projectile.alpha) / 255), 0.95f * ((255 - Projectile.alpha) / 255));
				if (Projectile.alpha < 255)
					Projectile.alpha += 3;

				Projectile.timeLeft = 6;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.position = Main.MouseWorld - new Vector2(Projectile.width/2, Projectile.height/2);
				}
			}
			else if (Projectile.ai[1] >= 0)
			{
				Lighting.AddLight(Projectile.Center, 0.95f * ((255 - Projectile.alpha)/255), 0.45f * ((255 - Projectile.alpha) / 255), 0.95f * ((255 - Projectile.alpha)/255));
				NPC owner = Main.npc[(int)Projectile.ai[1]];
				if (!owner.active || !(owner.type == ModContent.NPCType<OtherworldlyConstructHead2>() || owner.type == ModContent.NPCType<OtherworldlyConstructHead>() || owner.type == ModContent.NPCType<TheAdvisorHead>()))
				{
					Projectile.Kill();
				}
				if (owner.type == ModContent.NPCType<TheAdvisorHead>())
					Projectile.extraUpdates = 1;
			}
			if(Projectile.ai[1] == -3)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
				}
				Projectile.timeLeft = 30;
				Projectile.ai[1]--;
			}
			ai1++;
			if (ai1 == 30)
			{
				SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.3f);
			}
			if (ai1 == 60)
			{
				SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.4f);
			}
			if (ai1 > 61)
			{
				ai1 = 61;
			}
			if(doOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.2f);
				if (Projectile.timeLeft > Projectile.ai[0])
				{
					Projectile.timeLeft = (int)Projectile.ai[0];
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
			if(Projectile.timeLeft < 50 && Projectile.ai[1] >= 0)
			{
				Projectile.alpha -= 2;
			}
		}
	}
}