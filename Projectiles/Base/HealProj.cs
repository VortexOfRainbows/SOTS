using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Base
{    
    public class HealProj : ModProjectile 
    {	
		private float amount {
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		private float type {
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		private float extraNum1 {
			get => Projectile.knockBack;
			set => Projectile.knockBack = value;
		}
		
		private int healType {
			get => Projectile.damage;
			set => Projectile.damage = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heal Proj");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
        public override void SetDefaults()
        {
			Projectile.height = 8;
			Projectile.width = 8;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 720;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if ((int) type == 7)
			{
				Projectile.alpha = 0;
				Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Base/CeremonialEffect").Value;
				Vector2 drawOrigin = new Vector2(2, 2);
				for (int k = 0; k < 11; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					for (int j = 0; j < Projectile.oldPos.Length; j++)
					{
						Color color = new Color(100, 100, 100, 0);
						Vector2 drawPos = Projectile.oldPos[j] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
						color *= ((float)(Projectile.oldPos.Length - j) / (float)Projectile.oldPos.Length);
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		bool runOnce = true;
		int counter = 0;
		private void genDust()
		{
			if((int)type == 0) //platinum staff
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, 16);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
			if((int)type == 1 || (int)type == -1) //crimson heal
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, 60);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].scale = 1.5f;
			}
			if((int)type == 2) //corruption void heal
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, 62);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].scale = 1.5f;
			}
			if((int)type == 3) //corruption mana heal
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, 15);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].scale = 1.5f;
			}
			if((int)type == 4) //ice
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, 67);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				if(Projectile.timeLeft % 12 == 0)
				{
					additionalEffects();
				}
			}
			if((int)type == 5) //Hungry Hunter / default
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, 37);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
			if((int)type == 6) //Clover Charm
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, 2);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
			if ((int)type == 8) //Macaroni Heal
			{
				counter++;
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(255, 240, 50, 100), new Color(235, 240, 50, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 0.7f;
			}
			if ((int)type == 9) //Nature Heal
			{
				Color color1 = new Color(177, 238, 181, 100);
				Color color2 = new Color(64, 178, 77, 100);
				counter++;
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
				dust.color = Color.Lerp(color1, color2, new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 0.7f;
			}
		}
		private float getSpeed()
		{
			if((int)type == 4)
			{
				Projectile.extraUpdates = 2;
				return 11f;
			}
			if((int)type == 5) 
			{
				return 16.25f;
			}
			if((int)type == 6) //Clover Charm
			{
				return 11f;
			}
			if ((int)type == 7) //Ceremonial Dagger
			{
				Projectile.extraUpdates = 3;
				return 4.6f;
			}
			if ((int)type == 8 || (int) type == 9)  //Macaroni Heal or Nature Heal
			{
				Projectile.extraUpdates = 5;
				return 7.0f;
			}
			return 14.5f;
		}
		private float getMinDist()
		{
			if ((int)type == 7 || (int)type == 8 || (int)type == 9) //Ceremonial Dagger, Macaroni Heal or Nature Heal
			{
				return 9f;
			}
			return 20f;
		}
		private void additionalEffects()
		{
			if((int)type == 4)
			{
				for(int i = 0; i < 360; i += 30)
				{
					Vector2 circularLocation = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 67);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = -Projectile.velocity;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			
		}
		public override void AI()
		{
			if(runOnce)
            {
				if((int)type == -1)
				{
					SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, 0.1f);
					for (int i = 0; i < 40; i++)
					{
						Vector2 circularLocation = new Vector2(Main.rand.NextFloat(6f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
						int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, 60);
						Main.dust[dust].velocity = circularLocation;
						Main.dust[dust].scale *= 1.5f;
						Main.dust[dust].noGravity = true;
					}
				}
				runOnce = false;
            }
			Player player = Main.player[Projectile.owner];
			if(Projectile.timeLeft < 720)
			{
				genDust();
				Projectile.velocity = new Vector2(-getSpeed(), 0).RotatedBy(Math.Atan2(Projectile.Center.Y - player.Center.Y, Projectile.Center.X - player.Center.X));
				
				Vector2 toPlayer = player.Center - Projectile.Center;
				float distance = toPlayer.Length();
				if(distance < getMinDist() && amount > 0)
				{
					if(healType == 0)
					{
						player.statLife += (int)amount;
						if(player.whoAmI == Main.myPlayer)
							player.HealEffect((int)amount);
					}
					if(healType == 1 || (int)type == 8)
					{
						if ((int)type == 8)
                        {
							amount *= Main.rand.NextFloat(2f, 5f);
                        }
						player.statMana += (int)amount;
						if(player.whoAmI == Main.myPlayer)
							player.ManaEffect((int)amount);
					}
					if(healType == 2)
					{
						VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
						voidPlayer.voidMeter += amount;
						VoidPlayer.VoidEffect(player, (int)(amount + 0.95f));
					}
					Projectile.Kill();
				}
			}
		}
	}
}
		