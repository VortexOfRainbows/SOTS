using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Dusts;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Projectiles.Earth;

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
        private float nextIntervalForRocks = 170;
        public override void PostAI()
        {
            base.PostAI();
            if (timeLeftCounter > nextIntervalForRocks) // && thisSlashNumber == 1)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 velo = dustAway.SNormalize();
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), PlayerCenter() + velo * 44, velo * 7 + new Vector2(0, -1), ModContent.ProjectileType<StarShard>(), (int)(Projectile.damage * 0.1f + 1), Projectile.knockBack, Main.myPlayer, 0, 0, Main.rand.Next(3));
                }
                nextIntervalForRocks = 10000;
            }
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
    public class StarShard : ModProjectile
    {
        public static Color color1 => new Color(123, 214, 248);
        public static Color color2 => new Color(209, 132, 255);
        public static int TrailCount => 20;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = TrailCount;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<Void.VoidMelee>();
            Projectile.friendly = true;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 120;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.alpha = 255;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        private bool runOnce = true;
        public override void AI()
        {
            if(runOnce)
            {
                SOTSUtils.PlaySound(SoundID.Item39, Projectile.Center);
                runOnce = false;
            }
            Vector2 trueVelocity = Projectile.velocity * (1 + Projectile.ai[1] * Projectile.ai[1] / 2400f);
            Projectile.velocity.Y += 0.1f;
            if (Projectile.ai[1] > 6)
            {
                int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 360, Projectile, true);
                if (target != -1)
                {
                    NPC npc = Main.npc[target];
                    Vector2 toNPC = npc.Center - Projectile.Center;
                    toNPC = toNPC.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length() * 1.25f;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC, 0.03f);
                }
            }
            if (Projectile.ai[0] == -1)
            {
                Projectile.friendly = false;
                Projectile.velocity *= 0;
                Projectile.tileCollide = false;
                if (Projectile.timeLeft > TrailCount - 1)
                {
                    for (int i = 0; i < 360; i += 45)
                    {
                        Vector2 circularLocation = new Vector2(-5, 0).RotatedBy(MathHelper.ToRadians(i));
                        circularLocation.Y *= 0.4f;
                        circularLocation = circularLocation.RotatedBy(Projectile.rotation);
                        Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 5, Projectile.Center.Y + circularLocation.Y - 5), 0, 0, ModContent.DustType<PixelDust>());
                        dust.noGravity = true;
                        dust.velocity *= 0.1f;
                        dust.velocity += circularLocation * 0.1f + trueVelocity * 0.4f * Main.rand.NextFloat();
                        dust.scale = 1.0f;
                        dust.fadeIn = 6f;
                        dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(1));
                        dust.color.A = 0;
                        dust.alpha = 50;
                    }
                    SOTSUtils.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.Center, 1.0f, -0.2f);
                    Projectile.timeLeft = TrailCount - 1;
                }
            }
            else if(Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5) - trueVelocity * 1, 0, 0, ModContent.DustType<PixelDust>());
                dust.noGravity = true;
                dust.scale = 1f;
                dust.fadeIn = 7f;
                dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(1));
                dust.velocity *= 0.36f;
                dust.velocity += trueVelocity * 0.25f;
                dust.color.A = 0;
                dust.alpha = 50;
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            if (Projectile.timeLeft <= TrailCount + 1)
                Projectile.ai[0] = -1;
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 0)
                Projectile.position += trueVelocity;
            Projectile.alpha -= 25;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] == -1 ? false : base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = -1;
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = -1;
            Projectile.netUpdate = true;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 6);
            Rectangle frame = new Rectangle(0, (int)Projectile.ai[2] * texture.Height / 3, texture.Width, texture.Height / 3);
            int starting = 0;
            bool drawMain = false;
            if (Projectile.timeLeft <= TrailCount)
            {
                starting = TrailCount - Projectile.timeLeft;
            }
            else
            {
                drawMain = true;
            }
            for (int k = starting; k < Projectile.oldPos.Length; k++)
            {
                float scale = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Color toChange = Color.Lerp(color1, color2, scale);
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size / 2;
                Color color = Projectile.GetAlpha(toChange) * scale;
                Main.spriteBatch.Draw(texture, drawPos, frame, color * 0.25f, Projectile.oldRot[k] + MathHelper.PiOver2, drawOrigin, Projectile.scale * scale * 1f, SpriteEffects.None, 0f);
            }
            if (drawMain)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation + MathHelper.PiOver2, drawOrigin, Projectile.scale * 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}

