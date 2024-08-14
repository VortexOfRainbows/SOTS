using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Blades
{    
    public class StarshardSlash : SOTSBlade
	{
        public override string Texture => "SOTS/Items/ChestItems/StarshardSaber";
        public override Color color1 => new Color(123, 214, 248);
		public override Color color2 => new Color(209, 132, 255);
		public override void SafeSetDefaults()
		{
			Projectile.localNPCHitCooldown = 30;
			Projectile.DamageType = DamageClass.Melee;
			delayDeathTime = 8;
			Projectile.extraUpdates = 3;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override float HitboxWidth => 40;
		public override float AdditionalTipLength => 0;
		//public override float handleOffset => 24;
		public override float HeldDistFromPlayer => 12;
		public override Vector2 drawOrigin => new Vector2(7, 43);
        public override void SwingSound(Player player)
		{
			if(thisSlashNumber == 4)
				SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, -0.2f);
            else
                SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, -0.2f + (4 - thisSlashNumber) * 0.1f);
        }
		public override float speedModifier => Projectile.ai[1];
		public override float GetBaseSpeed(float swordLength)
		{
			return 3f + (4 - thisSlashNumber) * 0.65f;
		}
		public override float MeleeSpeedMultiplier => 0.6f;
		public override float OverAllSpeedMultiplier => 6f;
		public override float MinSwipeDistance => 110;
		public override float MaxSwipeDistance => 110;
		public override float ArcStartDegrees => thisSlashNumber != 4 ? 174 + (12 * thisSlashNumber) : 190;
		public override float swipeDegreesTotal => thisSlashNumber != 4 ? 164 + (12 * thisSlashNumber) : 230;
		public override float swingSizeMult => thisSlashNumber != 4 ? 0.9f + (3 - thisSlashNumber) * 0.1f : 1.0f;
		public override float ArcOffsetFromPlayer => 0.35f;
		public override float delayDeathSlowdownAmount => 0.9f;
		public override Color? DrawColor => null;
		private bool RunOnce = true;
        public override void PostAI()
        {
			base.PostAI();
        }
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
			if(thisSlashNumber == 4)
				original.Y *= 1 / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
            else if(original.Y * FetchDirection > 0)
            {
                original.Y *= 0.65f / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
            }
            return original;
		}
		public override void SlashPattern(Player player, int slashNumber)
		{
			int damage = Projectile.damage;
			if (slashNumber > 0)
			{
				float knockBackMult = 1;
				if (slashNumber == 1)
                {
					damage = (int)(damage * 1.2f);
					knockBackMult = 2.4f;
				}
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack * knockBackMult, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1]);
				if (proj.ModProjectile is ColossusSlash v)
				{
					if (slashNumber == 1)
					{
						v.distance = 180;
						v.delayDeathTime = 12;
					}
					if (slashNumber == 1)
					{
						v.distance = 230;
						v.delayDeathTime = 20;
					}
				}
			}
		}
		public override float ArmAngleOffset => 0;
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
		{
			float amt = Main.rand.NextFloat(0.1f, 1.1f);
            Dust d = PixelDust.Spawn(Projectile.Center, 0, 0, bladeDirection.SafeNormalize(Vector2.Zero) + Main.rand.NextVector2Circular(1, 1), Color.Lerp(color1, color2, Main.rand.NextFloat() * Main.rand.NextFloat()) * 0.7f, Main.rand.Next(15, 21));
			d.scale = 1.5f;
			d.color.A = 0;
            Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
			for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
			{
				float rand = Main.rand.NextFloat(1.0f, 1.2f);
				int type = ModContent.DustType<Dusts.AlphaDrainDust>();
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - toProjectile * Main.rand.NextFloat(0.6f), 16, 16, type);
				dust.velocity *= 0.15f;
				dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.3f, 0.5f);
				dust.noGravity = true;
				dust.scale *= 0.1f;
				dust.scale += rand * 0.8f;
				dust.fadeIn = 0.2f;
				dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat() * Main.rand.NextFloat()) * 1.5f;
			}
		}
        public override float TrailLengthMultiplier => 0.5f;
		public override float TrailOffsetFromTip => 0.96f;
    }
}
		
			