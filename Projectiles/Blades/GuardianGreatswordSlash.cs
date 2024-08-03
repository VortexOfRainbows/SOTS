using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Items.Tools;
using System.Windows.Markup;
using SOTS.Void;

namespace SOTS.Projectiles.Blades
{    
    public class GuardianGreatswordSlash : SOTSBlade
	{
		public override Color color1 => new Color(179, 33, 68);
		public override Color color2 => new Color(243, 200, 186);
		public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 7200;
            Projectile.localNPCHitCooldown = 15;
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
            delayDeathTime = 12;
            Projectile.friendly = true;
		}
		public override float HitboxWidth => 24;
		public override float AdditionalTipLength => 0;
		public override float HeldDistFromPlayer => 11;
		public override Vector2 drawOrigin => new Vector2(6, 52);
        public override void SwingSound(Player player)
        {
            SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, -(thisSlashNumber == 1 ? 0.3f : 0.5f)); //playsound function 
        }
		public override float speedModifier => 1f;
		public override float GetBaseSpeed(float swordLength)
		{
			return 3f;
		}
		public override float OverAllSpeedMultiplier => (thisSlashNumber == 1 ? 3.5f : 4.75f);
        public override float ActiveSpeedMultiplier()
		{
            float timeLeft = timeLeftCounter;
            if (timeLeft < 0)
                timeLeft = 0;
			return (thisSlashNumber == 1 ? 0.4f : 0.3f) + (float)Math.Pow(1.6f * (timeLeft / swipeDegreesTotal), (thisSlashNumber == 1 ? 1.5f : 2)) * Projectile.ai[1];
        }
        public override float MinSwipeDistance => 150;
		public override float MaxSwipeDistance => 150;
		public override float ArcStartDegrees => 190;
		public override float swipeDegreesTotal => thisSlashNumber == 1 || thisSlashNumber == 2 ? 200f : 170f;
        public override float swingSizeMult => thisSlashNumber == 1 ? 1.2f : thisSlashNumber == 2 ? 0.8f : 1.0f;
		public override float ArcOffsetFromPlayer => 0.3f;
		public override float delayDeathSlowdownAmount => thisSlashNumber == 1 ? 0.75f : 0.7f;
		public override Color? DrawColor => null;
		private bool RunOnce = true;
        private int nextProj = 12;
        public override bool PreAI()
        {
            if(RunOnce)
            {
                delayDeathTime = thisSlashNumber == 2 ? 8 : thisSlashNumber == 1 ? 16 : delayDeathTime;
                RunOnce = false;
            }
            return base.PreAI();
        }
        public override void AI()
        {
            base.AI(); //This is needed in order to do the Blade Stuff
            if(thisSlashNumber == 1)
            {
                int slashCount = 20;
                int slashDelay = 9;
                if (timeLeftCounter > 90 && timeLeftCounter > slashDelay * nextProj && nextProj <= slashCount && timeLeftCounter < 270)
                {
                    if(nextProj % 2 == 0) 
                        SOTSUtils.PlaySound(SoundID.Item60, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, 0.25f);
                    nextProj++;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        for(int i = -1; i <= 1; i++)
                        {
                            Vector2 awayVector = dustAway.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(15f * i));
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - dustAway.SafeNormalize(Vector2.Zero) * 72 + awayVector * 32, awayVector * 12, ModContent.ProjectileType<GuardianGreatswordBeam>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0, -2);
                        }
                    }
                }
            }
        }
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
            if(swingNumber == 2 && original.Y * Projectile.ai[0] > 0)
                yDistanceCompression += 0.1f;
            original.Y *= (swingNumber == 1 ? 1f : swingNumber == 2 ? 1.2f : 0.75f) / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
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
			if (slashNumber > 0)
			{
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack * 0.4f, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1]);
			}
		}
		public override float ArmAngleOffset => 5; 
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
		{
            for(int i = 0; i < 2; i++)
            {
                float rand = Main.rand.NextFloat(0.6f, 0.7f);
                int type = ModContent.DustType<Dusts.PixelDust>();
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5) + bladeDirection.SafeNormalize(Vector2.Zero) * 8, 2, 2, type);
                dust.velocity *= 0.25f;
                dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
                dust.noGravity = true;
                dust.scale *= 0.5f * rand;
                dust.scale += 1.2f * rand;
                dust.fadeIn = 6f;
                dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
                dust.color.A = 0;
            }
		}
        public override float TrailLengthMultiplier => base.TrailLengthMultiplier;
		public override float TrailOffsetFromTip => 0.81f;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
    }
}
		
			