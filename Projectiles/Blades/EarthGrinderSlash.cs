using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Utilities;
using SOTS.Void;
using SOTS.Prim.Trails;
using SOTS.Projectiles.Earth;
using SOTS.Common.GlobalNPCs;
using SOTS.Items.Tools;

namespace SOTS.Projectiles.Blades
{    
    public class EarthGrinderSlash : SOTSBlade
	{
		public override Color color1 => new Color(250, 242, 76);
		public override Color color2 => new Color(184, 103, 16);
		public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 7200;
            Projectile.localNPCHitCooldown = 20;
			Projectile.DamageType = DamageClass.Melee;
			delayDeathTime = 16;
			Projectile.friendly = false;
		}
		public override float HitboxWidth => 32;
		public override float AdditionalTipLength => 36;
		public override float handleOffset => 8;
		public override float handleSize => 44;
		public override Vector2 drawOrigin => new Vector2(5, 64);
        public override void SwingSound(Player player)
		{
			SOTSUtils.PlaySound(SoundID.Item1, (int)player.Center.X, (int)player.Center.Y, 1.1f, thisSlashNumber == 1 ? -0.2f : 0.3f); 
		}
		public override float speedModifier => 1f;
		public override float GetBaseSpeed(float swordLength)
		{
			return 3f;
		}
		public override float OverAllSpeedMultiplier => 5 * (thisSlashNumber == 2 ? 1 : 0.65f);
        public override float ActiveSpeedMultiplier()
		{
            float timeLeft = timeLeftCounter;
            if (timeLeft < 0)
                timeLeft = 0;
			return 0.2f + (float)Math.Pow(1.8f * (timeLeft / swipeDegreesTotal), thisSlashNumber == 2 ? 2 : 1) * Projectile.ai[1];
        }
        public override float MinSwipeDistance => 80;
		public override float MaxSwipeDistance => 80;
		public override float ArcStartDegrees => thisSlashNumber == 1 ? 215 : 180;
		public override float swipeDegreesTotal => thisSlashNumber == 1 ? 215f : 185f;
		public override float swingSizeMult => 1.0f;
		public override float ArcOffsetFromPlayer => 0.3f;
		public override float delayDeathSlowdownAmount => 0.6f;
		public override Color? DrawColor => null;
		private bool BonusSound = false;
        public override void PostAI()
        {
            base.PostAI();
            //Main.NewText(timeLeftCounter);
            if (Main.player[Projectile.owner].HeldItem.type == ModContent.ItemType<EarthGrinder>())
            {
                Main.player[Projectile.owner].HeldItem.noUseGraphic = true;
            }
            if (thisSlashNumber == 3)
                delayDeathTime = 0;
            if (!BonusSound && ActiveSpeedMultiplier() > 1)
            {
                Projectile.friendly = true;
                BonusSound = true;
				if(thisSlashNumber == 2)
                {
                    SOTSUtils.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Projectile.Center, 1f, -0.3f);
                }
                else if (thisSlashNumber == 1)
                {
                    SOTSUtils.PlaySound(SoundID.Item23, Projectile.Center, 1f, -0.3f);
                    Projectile.localNPCHitCooldown = 3;
                }
            }
            if (thisSlashNumber == 1)
            {
                if (Projectile.ai[1] <= -0.5f)
                {
                    SOTSUtils.PlaySound(SoundID.Item23, Projectile.Center, 1f, 0.3f);
					for(int i = 0; i < 10; i++)
                    {
                        float rand = Main.rand.NextFloat(0.7f, 0.9f);
                        int type = ModContent.DustType<Dusts.CopyDust4>();
                        Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + dustAway.SafeNormalize(Vector2.Zero) * 16 + dustAway.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * 32 * FetchDirection, 16, 16, type);
                        dust.velocity *= 1.4f;
                        dust.noGravity = true;
                        dust.scale *= 0.5f * rand;
                        dust.scale += 0.5f * rand;
                        dust.fadeIn = 0.1f;
                        dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
                    }
                }
                if (Projectile.ai[1] < 1)
                {
                    if (Projectile.ai[1] > 0)
                        Projectile.ai[1] *= 1.1f;
                    Projectile.ai[1] += 0.1f;
                }
                Projectile.ai[1] = MathHelper.Clamp(Projectile.ai[1], -0.5f, 1.0f);
            }
        }
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
			if (original.Y * Projectile.ai[0] > 0)
				yDistanceCompression += 0.2f;
			original.Y *= (swingNumber == 1 ? 0.65f : 0.75f) / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
			return original;
		}
		public override float TimeLeftIterator(float incrementAmount)
        {
            float num = (float)Math.Abs(incrementAmount) * Math.Sign(Projectile.ai[1]);
            if (timeLeftCounter < 0 && num < 0)
                num = 0;
            return num;
        }
        public override void SlashPattern(Player player, int slashNumber)
		{
			int damage = Projectile.damage;
			if (slashNumber > 0 && slashNumber != 2)
			{
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack * 0.4f, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1]);
			}
		}
		public override float ArmAngleOffset => 18; //hold it with a backwards grip because thats funny
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
		{
			float rand = Main.rand.NextFloat(0.6f, 0.7f);
			int type = ModContent.DustType<Dusts.CopyDust4>();
			Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 16, Projectile.Center.Y - 16) + bladeDirection.SafeNormalize(Vector2.Zero) * 16, 24, 24, type);
			dust.velocity *= 0.45f;
			dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
			dust.noGravity = true;
			dust.scale *= 0.4f * rand;
			dust.scale += 1.3f * rand;
			dust.fadeIn = 0.1f;
			dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f)) * 0.5f;
		}
        public override float TrailDistanceFromHandle => 52f;
		public override float AddedTrailLength => 0f;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(thisSlashNumber == 1)
            {
                Projectile.netUpdate = true;
                Projectile.ai[1] = -0.5f;
                Projectile.localNPCHitCooldown++;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (thisSlashNumber == 2)
                modifiers.SetCrit();
        }
    }
}
		
			