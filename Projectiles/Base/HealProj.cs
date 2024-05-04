using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Base
{
	public static class HealProjID
    {
        public const int CrimsonHealAlt = -1;
        public const int PlatinumStaff = 0;
		public const int CrimsonHeal = 1;
		public const int CorruptionVoidHeal = 2;
		public const int CorruptionManaHeal = 3;
		public const int Ice = 4;
		public const int HungryHunter = 5;
		public const int CloverCharm = 6;
        public const int CeremonialDagger = 7;
        public const int MacaroniHeal = 8;
        public const int NatureSpiritHeal = 9;
    }
    public class HealProj : ModProjectile 
    {	
		private float amount {
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private int type {
			get => (int)Projectile.ai[1];
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
			// DisplayName.SetDefault("Heal Proj");
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
			if(type == HealProjID.PlatinumStaff) //platinum staff
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, DustID.Cloud);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.alpha = 100;
            }
			if(type == HealProjID.CrimsonHeal || type == HealProjID.CrimsonHealAlt) //crimson heal
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, DustID.RedTorch);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
                dust.scale = 1.5f;
                dust.alpha = 100;
            }
			if(type == HealProjID.CorruptionVoidHeal) //corruption void heal
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, DustID.PurpleTorch);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
                dust.scale = 1.5f;
                dust.alpha = 100;
            }
			if(type == HealProjID.CorruptionManaHeal) //corruption mana heal
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, DustID.MagicMirror);
				dust.noGravity = true;
                dust.velocity *= 0.1f;
				dust.scale = 1.5f;
                dust.alpha = 200;
            }
			if(type == HealProjID.Ice) //ice
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, DustID.IceRod);
				dust.noGravity = true;
                dust.velocity *= 0.1f;
				if(Projectile.timeLeft % 12 == 0)
				{
					additionalEffects();
                }
                dust.alpha = 100;
            }
			if(type == HealProjID.HungryHunter) //Hungry Hunter / default
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, DustID.Obsidian);
				dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.alpha = 100;
            }
			if(type == HealProjID.CloverCharm) //Clover Charm
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, DustID.Grass);
				dust.noGravity = true;
                dust.velocity *= 0.05f;
				dust.color = new Color(100, 120, 110, 0);
                dust.scale = dust.scale * 0.5f + 0.6f;
				dust.alpha = 100;
            }
			if (type == HealProjID.MacaroniHeal) //Macaroni Heal
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(255, 240, 50, 100), new Color(235, 240, 50, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 0.7f;
			}
			if (type == HealProjID.NatureSpiritHeal) //Nature Heal
			{
				Color color1 = new Color(177, 238, 181, 100);
				Color color2 = new Color(64, 178, 77, 100);
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
			if(type == HealProjID.Ice)
			{
				Projectile.extraUpdates = 4;
				return 5f;
			}
			if(type == HealProjID.HungryHunter)
            {
                Projectile.extraUpdates = 5;
                return 3.25f;
			}
			if(type == HealProjID.CloverCharm) //Clover Charm
            {
                Projectile.extraUpdates = 2;
                return 4f;
			}
			if (type == HealProjID.CeremonialDagger) //Ceremonial Dagger
			{
				Projectile.extraUpdates = 3;
				return 4.6f;
			}
			if (type == HealProjID.MacaroniHeal || type == HealProjID.NatureSpiritHeal)  //Macaroni Heal or Nature Heal
			{
				Projectile.extraUpdates = 5;
				return 7.0f;
			}
			Projectile.extraUpdates = 2;
			return 5.5f;
		}
		private float getMinDist()
		{
			if (type == HealProjID.CeremonialDagger || type == HealProjID.MacaroniHeal || type == HealProjID.NatureSpiritHeal) //Ceremonial Dagger, Macaroni Heal or Nature Heal
			{
				return 9f;
			}
			return 20f;
		}
		private void additionalEffects()
		{
			if(type == HealProjID.Ice)
			{
				for(int i = 0; i < 360; i += 30)
				{
					Vector2 circularLocation = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(i));
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.IceRod);
					dust.noGravity = true;
                    dust.velocity = -Projectile.velocity;
				}
			}
		}
		public override void OnKill(int timeLeft)
		{
			
		}
		public override void AI()
		{
			if(runOnce)
            {
				if(type == HealProjID.CrimsonHealAlt)
				{
					SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, 0.1f);
					for (int i = 0; i < 40; i++)
					{
						Vector2 circularLocation = new Vector2(Main.rand.NextFloat(6f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
                        Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, DustID.RedTorch);
						dust.velocity = circularLocation;
						dust.scale *= 1.5f;
                        dust.noGravity = true;
					}
				}
				runOnce = false;
            }
			Player player = Main.player[Projectile.owner];
			if(Projectile.timeLeft < 720)
            {
                counter++;
                genDust();
                Vector2 toPlayer = player.Center - Projectile.Center;
				float bonusSpeed = 1 + counter / 360f;
				float finalSpeed = getSpeed() * bonusSpeed;
                Projectile.velocity = finalSpeed * toPlayer.SafeNormalize(Vector2.Zero);
				float distance = toPlayer.Length();
				if(distance < getMinDist() && amount > 0)
				{
					if(healType == 0)
					{
						player.statLife += (int)amount;
						if(player.whoAmI == Main.myPlayer)
							player.HealEffect((int)amount);
					}
					if(healType == 1 || type == HealProjID.MacaroniHeal)
					{
						if (type == HealProjID.MacaroniHeal)
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
		