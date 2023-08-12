using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using System;

namespace SOTS.Projectiles.Otherworld
{
	public class HardlightColumn : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hardlight Column");
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.velocity.X);
			writer.Write(Projectile.velocity.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.velocity.X = reader.ReadSingle();
			Projectile.velocity.Y = reader.ReadSingle();
		}
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;
			Projectile.scale = 0.75f;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox = new Rectangle((int)Projectile.position.X - Projectile.width, (int)Projectile.position.Y - Projectile.height, Projectile.width * 3, Projectile.height * 3);
            base.ModifyDamageHitbox(ref hitbox);
        }
        Vector2[] trailPos = new Vector2[4];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/HardlightColumn").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
                {
					return false;
                }
				Color color = new Color(120, 140, 160, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (14 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 4; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
                        {
							x = 0;
							y = 0;
                        }
						if(trailPos[k] != Projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
		bool runOnce = true;
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
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if((endHow == 1 && endHow != 2 && Main.rand.NextBool(6)) || (Main.rand.NextBool(35) && Projectile.velocity.Length() > 0))
			{
				int dust1 = Dust.NewDust(Projectile.position - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[dust1];
				dust.scale *= 1f * (10f - iterator)/10f;
				dust.velocity += Projectile.velocity * 0.3f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(160, 200, 220, 70), new Color(120, 140, 180, 70), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = Projectile.alpha;
			}
			if (iterator >= trailPos.Length)
				Projectile.Kill();
		}
		int endHow = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			endHow = 1;
			Projectile.tileCollide = false;
			Projectile.velocity *= 0f;
            return false;
        }
		int counter = 0;
		int counter2 = 0;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
        public override void AI()
		{
			if(Projectile.timeLeft < 220)
			{
				endHow = 2;
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
			}
			if(runOnce)
			{
				if(Projectile.ai[1] == -1)
				{
					Projectile.extraUpdates = 3;
					Projectile.ai[1] = 0;
					SOTSUtils.PlaySound(SoundID.Item94, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f);
				}
				originalVelo = Projectile.velocity;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				originalPos = Projectile.Center;
			}
			originalPos += originalVelo * 1.4f;
			checkPos();
			Player player = Main.player[Projectile.owner];
			Vector2 toPlayer = player.Center - Projectile.Center;
			if(counter2 > 28 - Projectile.ai[0] * 4)
            {
				if(Projectile.ai[0] > 0 && endHow == 0)
                {
					for (int i = 0; i < 3; i += 2)
					{
						if (Projectile.owner == Main.myPlayer)
						{
							Vector2 perturbedSpeed = new Vector2(originalVelo.X, originalVelo.Y).RotatedBy(MathHelper.ToRadians((i - 1) * 12.5f));
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<HardlightColumn>(), Projectile.damage, 1f, Main.myPlayer, Projectile.ai[0] - 1);
						}
					}
				}
				Projectile.velocity *= 0f;
				originalVelo *= 0f;
				Projectile.ai[0] = 0f;
				Projectile.friendly = false;
			}
			counter++;
			counter2++;
			if(counter >= 0)
			{
				cataloguePos();
				counter = -10;
				if(Main.myPlayer == Projectile.owner)
				{
					if (Projectile.velocity.Length() != 0f)
					{
						Vector2 toPos = originalPos - Projectile.Center;
						Projectile.velocity = new Vector2(originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
						Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
					}
					Projectile.ai[1] = Main.rand.Next(-35, 36);
					if (Projectile.owner == Main.myPlayer)
						Projectile.netUpdate = true;
				}
			}
		}
	}
}