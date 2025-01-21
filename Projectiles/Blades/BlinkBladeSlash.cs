using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Blades
{    
    public class BlinkBladeSlash : SOTSBlade
	{
		public override string Texture => "SOTS/Items/Conduit/BlinkBlade";
        public override Color color1 => new Color(177, 73, 190) * 1.5f;
		public override Color color2 => new Color(204, 129, 193) * 1.5f;
		public override void SafeSetDefaults()
		{
			Projectile.localNPCHitCooldown = 120;
			Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
			delayDeathTime = 1;
			Projectile.extraUpdates = 7;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override float HitboxWidth => 20;
		public override float AdditionalTipLength => 0;
		public override float HeldDistFromPlayer => 8;
		public override Vector2 drawOrigin => new Vector2(7, 35);
        public override void SwingSound(Player player)
		{
			SOTSUtils.PlaySound(SoundID.Item1, (int)player.Center.X, (int)player.Center.Y, 1.1f, -0.1f); //playsound function
		}
		public override float speedModifier => Projectile.ai[1];
		public override float GetBaseSpeed(float swordLength)
		{
			return 6f;
		}
		public override float MeleeSpeedMultiplier => 1.25f;
		public override float OverAllSpeedMultiplier => 6f;
		public override float MinSwipeDistance => thisSlashNumber == 2 ? 108 : 99;
		public override float MaxSwipeDistance => thisSlashNumber == 2 ? 108 : 99;
		public override float ArcStartDegrees => 200;
		public override float swipeDegreesTotal => 250f;
		public override float swingSizeMult => 1.0f;
		public override float ArcOffsetFromPlayer => thisSlashNumber == 2 ? 0.2f : 0.55f;
		public override float delayDeathSlowdownAmount => 0.7f;
		public override Color? DrawColor => null;
		private bool RunOnce = true;
		private float nextIntervalForProj = 175;
        public override void PostAI()
        {
			base.PostAI();
			Player p = Main.player[Projectile.owner];
			if(timeLeftCounter > nextIntervalForProj)
            {
                if (thisSlashNumber == 1)
				{
					SOTSUtils.PlaySound(SoundID.Item72, Projectile.Center, 0.7f, 0.6f);
					if (Main.myPlayer == Projectile.owner)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - dustAway.SNormalize() * 5f, dustAway.SNormalize() * (1 + 4 * p.GetTotalAttackSpeed<VoidMelee>()), ModContent.ProjectileType<BlinkLightning>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 0, 1);
					}
				}
				else
				{
                    delayDeathTime = 40;
                }
                nextIntervalForProj = 1000;
			}
			if(thisSlashNumber == 2 && timeLeftCounter < nextIntervalForProj && nextIntervalForProj != 1000)
			{
				Vector2 warpDirection = Projectile.velocity.SNormalize() * 20f / (Projectile.extraUpdates + 1) * p.GetTotalAttackSpeed<VoidMelee>();
                warpDirection = Collision.TileCollision(p.position, warpDirection, p.width, p.height, true, true, (int)p.gravDir);
				p.position += warpDirection;
				p.velocity.Y *= 0.95f;
				p.velocity += warpDirection * 0.001f;
            }
        }
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
			if(thisSlashNumber == 2)
                original.Y *= 0.8f * yDistanceCompression;
            else
				original.Y *= 0.7f * yDistanceCompression;
			return original;
		}
		public override void SlashPattern(Player player, int slashNumber)
		{
			//int damage = Projectile.damage;
			//if (slashNumber > 0)
			//{
			//	float knockBackMult = 1;
			//	if (slashNumber == 1)
            //    {
			//		damage = (int)(damage * 1.2f);
			//		knockBackMult = 2.4f;
			//	}
			//	Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack * knockBackMult, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1]);
			//	if (proj.ModProjectile is ColossusSlash v)
			//	{
			//		if (slashNumber == 1)
			//		{
			//			v.distance = 180;
			//			v.delayDeathTime = 12;
			//		}
			//		if (slashNumber == 1)
			//		{
			//			v.distance = 230;
			//			v.delayDeathTime = 20;
			//		}
			//	}
			//}
		}
		public override float ArmAngleOffset => 2;
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
		{
			if (!Main.rand.NextBool(4))
				return;
			Vector2 sn = bladeDirection.SafeNormalize(Vector2.Zero);
			Color c = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
			c.A = 0;
            PixelDust.Spawn(Projectile.Center, 0, 0, sn * Main.rand.NextFloat(2) + Main.rand.NextVector2Circular(0.2f, 0.2f), c, 8).scale = Main.rand.NextFloat(1.0f, 1.5f);
		}
        public override float TrailLengthMultiplier => 0.8f;
		public override float TrailOffsetFromTip => base.TrailOffsetFromTip;
    }
}
		
			