using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Void;
using SOTS.Prim.Trails;
using Mono.CompilerServices.SymbolWriter;

namespace SOTS.Projectiles.Blades
{    
    public abstract class SOTSBlade : ModProjectile
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(counter);
            writer.Write(spinSpeed);
            writer.Write(toCursor.X);
            writer.Write(toCursor.Y);
            writer.Write(cursorArea.X);
            writer.Write(cursorArea.Y);
            writer.Write(distance);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            counter = reader.ReadSingle();
            spinSpeed = reader.ReadSingle();
            toCursor.X = reader.ReadSingle();
            toCursor.Y = reader.ReadSingle();
            cursorArea.X = reader.ReadSingle();
            cursorArea.Y = reader.ReadSingle();
            distance = reader.ReadSingle();
        }
        public sealed override bool PreDraw(ref Color lightColor)
        {
            Draw(Main.spriteBatch, ref lightColor);
            return false;
        }
        private float prevGfxOffY = 0;
        public Vector2 PlayerCenter()
        {
            Player player = Main.player[Projectile.owner];
            int totalUpdates = Projectile.extraUpdates + 1;
            int currentUpdate = Projectile.numUpdates + 1;
            float mult = 1 - (float)currentUpdate / totalUpdates;
            //1f + Math.Abs(velocity.X) / 3f; //Rate of change for player gfxOffY
            //It might make it smoother to take this into account
            return player.RotatedRelativePoint(Vector2.Lerp(player.oldPosition, player.position, mult) + player.Size / 2, false, true);
        }
        public void Draw(SpriteBatch spriteBatch, ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (DrawColor != null)
            {
                lightColor = (Color)DrawColor;
            }
            Vector2 playerToProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 rotateToPosition = playerToProjectile.SNormalize() * HeldDistFromPlayer;
            Vector2 playerArmPos = player.Center + rotateToPosition;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            float length = playerToProjectile.Length() - HeldDistFromPlayer;
            Vector2 drawPos = playerArmPos - Main.screenPosition;
            Vector2 bottomLeft = new Vector2(0, texture.Height);
            Vector2 origin = drawOrigin;

            int direction = 1;
            if (toCursor.X < 0)
            {
                direction = -1;
                direction *= -(int)FetchDirection;
            }
            else
            {
                direction *= (int)FetchDirection;
            }
            if (direction == -1)
            {
                origin = new Vector2(texture.Width - drawOrigin.X, drawOrigin.Y);
            }

            float standardSwordLength = texture.Size().Length() - (bottomLeft - drawOrigin).Length();
            if (!isDiagonalSprite && Projectile.type != ModContent.ProjectileType<BetrayersSlash>())
            {
                standardSwordLength = texture.Height - HeldDistFromPlayer;
            }
            float scaleMultiplier = length / standardSwordLength;
            float rotation = playerToProjectile.ToRotation();
            rotation += isDiagonalSprite ? MathHelper.ToRadians(direction == -1 ? -225 : 45) : MathHelper.ToRadians(90 + direction * OffsetAngleIfNotDiagonal);
            if (Projectile.type == ModContent.ProjectileType<EarthGrinderSlash>() && thisSlashNumber == 1)
            {
                Texture2D bonusTexture = ModContent.Request<Texture2D>("SOTS/Projectiles/Blades/EarthGrinderSlashAlternate").Value;
                spriteBatch.Draw(bonusTexture, drawPos + Main.rand.NextVector2CircularEdge(1, 1) * (SOTSWorld.GlobalCounter / 4 % 2), new Rectangle(0, bonusTexture.Height / 2 * (SOTSWorld.GlobalCounter / 4 % 2), bonusTexture.Width, bonusTexture.Height / 2), lightColor, rotation, origin, 0.1f + 1f * scaleMultiplier, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(texture, drawPos, null, lightColor, rotation, origin, scaleMultiplier, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }

            //Draw a dot on the tipe of the blade
            //Vector2 tip = Projectile.Center;
            //Texture2D ball = ModContent.Request<Texture2D>("SOTS/Projectiles/Blades/BloodSplatter").Value;
            //origin = ball.Size() / 2;
            //spriteBatch.Draw(ball, tip - Main.screenPosition, null, lightColor * 0.5f, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
        public virtual bool createDustWhileSlowingDown => true;
		public int GravDirection => (int)Main.player[Projectile.owner].gravDir;
		public int thisSlashNumber => Math.Abs((int)Projectile.ai[0]);
		public virtual float delayDeathSlowdownAmount => 0.5f;
		public virtual Color color1 => new Color(255, 185, 81);
		public virtual Color color2 => new Color(209, 117, 61);
        public virtual float HitboxWidth => 30;
        public virtual float AdditionalTipLength => 30;
        public virtual float HeldDistFromPlayer => 24; 
        public virtual Vector2 drawOrigin => new Vector2(10, 52);
        public virtual bool isDiagonalSprite => true;
        public virtual float OffsetAngleIfNotDiagonal => 0;
        public virtual Color? DrawColor => Color.White;
        public virtual float ArmAngleOffset => 18;
        public virtual float speedModifier => Projectile.ai[1];
        public virtual float GetBaseSpeed(float swordLength)
        {
            return (2.5f + (1.0f / (float)Math.Pow(swordLength / MaxSwipeDistance, 2f)));
        }
        public virtual float MeleeSpeedMultiplier => 0.1f;
        public virtual float OverAllSpeedMultiplier => 5f;
        public virtual float MinSwipeDistance => 80;
        public virtual float MaxSwipeDistance => 92;
        public virtual float ArcStartDegrees => 270 - 60f / speedModifier;
        public virtual float TrailOffsetFromTip => 0f;
        public virtual float TrailLengthMultiplier => 1f;
        public virtual float swipeDegreesTotal => 262.5f + (1800f / distance / speedModifier);
        public virtual float swingSizeMult => 0.7f + 0.3f * speedModifier;
        public virtual float ArcOffsetFromPlayer => 0.25f;
        public int FetchDirection => Math.Sign(Projectile.ai[0]);
        protected float counter = 225;
        protected float spinSpeed = 0;
        public int delayDeathTime = 0;
        private float delayDeathSlowdown = 1f;
        public Vector2 dustAway = Vector2.Zero;
        public Vector2 cursorArea = Vector2.Zero;
        public Vector2 toCursor = Vector2.Zero;
        public bool runOnce = true;
        public float distance = 0;
        public float counterOffset;
        public float timeLeftCounter = 0;
        protected BladeTrail myTrail;
        public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;    
		}        
		public sealed override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32; 
            Projectile.timeLeft = 12000;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>(); 
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.ownerHitCheck = true;
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{
			Projectile.localNPCHitCooldown = 15;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 center = PlayerCenter();
			float point = 0f;
			Vector2 previousPosition = Projectile.Center;
			float scale = Projectile.scale * 1f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition + dustAway.SafeNormalize(Vector2.Zero) * AdditionalTipLength, center, HitboxWidth * scale, ref point))
			{
				return true;
			}
			return false;
		}
		public override bool ShouldUpdatePosition()
        {
            return false;
        }
		public Vector2 relativePoint(Vector2 toArea, float length = 24)
        {
			return toArea;
		}
		public int GetArcLength()
		{
			Player player = Main.player[Projectile.owner];
			Vector2 toProjectile = Projectile.Center - PlayerCenter();
			int length = (int)(toProjectile.Length() - HeldDistFromPlayer);
			return length;
		}
		public virtual void SwingSound(Player player)
		{
			SOTSUtils.PlaySound(SoundID.Item71, (int)PlayerCenter().X, (int)PlayerCenter().Y, 0.75f, 0.6f * speedModifier); //playsound function
		}
		public virtual Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
			original.Y *= (0.75f + swingNumber * 0.005f) / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
			return original;
		}
		public virtual float ActiveSpeedMultiplier()
		{
			return 1f;
		}
		public virtual float TimeLeftIterator(float incrementAmount)
		{
			return (float)Math.Abs(incrementAmount);
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                int trailType = 1;
                int trailLength = 20;
                if (Type == ModContent.ProjectileType<TesseractSlash>())
                {
                    trailType = 3;
                    trailLength = 120;
                }
                myTrail = new BladeTrail(Projectile, FetchDirection, color1.ToVector4(), color2.ToVector4(), trailLength, trailType);
                SOTS.primitives.CreateTrail(myTrail);
                SwingSound(player);
                if (Main.myPlayer == Projectile.owner)
                {
                    cursorArea = Main.MouseWorld;
                    Projectile.netUpdate = true;
                    if (distance == 0)
                    {
                        distance = Vector2.Distance(PlayerCenter(), cursorArea) * speedModifier;
                        if (distance < MinSwipeDistance)
                            distance = MinSwipeDistance;
                        if (distance > MaxSwipeDistance)
                            distance = MaxSwipeDistance;
                    }
                    toCursor = cursorArea - PlayerCenter();
                    spinSpeed = GetBaseSpeed(distance) * speedModifier * OverAllSpeedMultiplier * ((1 - MeleeSpeedMultiplier) + MeleeSpeedMultiplier * (SOTSPlayer.ModPlayer(player).attackSpeedMod * player.GetAttackSpeed(DamageClass.Melee))); //add virtual/abstract variables for this
                }
                counterOffset = ArcStartDegrees; //add virtual/abstract variables for this
                float slashOffset = counterOffset * FetchDirection;
                counter = slashOffset;
                runOnce = false;
            }
            return base.PreAI();
        }
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			int AbsAI0 = (int)Math.Abs(Projectile.ai[0]);
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.2f / 255f, (255 - Projectile.alpha) * 0.7f / 255f, (255 - Projectile.alpha) * 1.0f / 255f);
			if(toCursor != Vector2.Zero)
			{
				float yDistMult = (float)Math.Pow(distance / MaxSwipeDistance, 0.5);
				double deg = counter; 
				double rad = deg * (Math.PI / 180);
				Vector2 ovalArea = new Vector2(distance * ArcOffsetFromPlayer * swingSizeMult, 0).RotatedBy(toCursor.ToRotation()); //center a point somewhat distant from the player
				Vector2 ovalArea2 = new Vector2(distance * (1 - ArcOffsetFromPlayer) * swingSizeMult, 0).RotatedBy((float)rad); //create a circle

				ovalArea2 = ModifySwingVector2(ovalArea2, yDistMult, AbsAI0);

				ovalArea2 = ovalArea2.RotatedBy(toCursor.ToRotation());
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;
				Projectile.Center = PlayerCenter() + ovalArea; 
				dustAway = ovalArea;
				Projectile.rotation = dustAway.ToRotation();
			}
			float incrementAmount = spinSpeed * FetchDirection * ActiveSpeedMultiplier();
			if (timeLeftCounter > swipeDegreesTotal)
			{
				if (delayDeathTime > 0)
				{
					delayDeathTime--;
					delayDeathSlowdown *= delayDeathSlowdownAmount;
					incrementAmount *= delayDeathSlowdown;
				}
			}
			timeLeftCounter += TimeLeftIterator(incrementAmount);
			counter += incrementAmount;
			if (timeLeftCounter > swipeDegreesTotal)
            {
				if(delayDeathTime <= 0)
				{
					Projectile.hide = true;
					Projectile.Kill();
				}
				else if (createDustWhileSlowingDown && delayDeathSlowdown > 0.15f)
				{
					SpawnDustDuringSwing(player, distance, dustAway);
				}
			}
			else
            {
				if (dustAway != Vector2.Zero)
				{
					SpawnDustDuringSwing(player, distance, dustAway);
				}
			}
        }
        public override void PostAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.hide == false && toCursor != Vector2.Zero)
            {
                Vector2 toProjectile = Projectile.Center - PlayerCenter();
                int direction = 1;
                if (toCursor.X < 0)
                    direction = -1;
                Projectile.alpha = 0;
                player.ChangeDir(direction);
                player.heldProj = Projectile.whoAmI;
                player.itemTime = 4;
                player.itemAnimation = 4;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * toProjectile.ToRotation() + MathHelper.ToRadians(-90 + (GravDirection * FetchDirection == -1 ? -ArmAngleOffset : ArmAngleOffset))));
            }
            Projectile.hide = false;
            int currentUpdate = Projectile.numUpdates + 1;
            if (currentUpdate > 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    if (myTrail.Entity is Projectile proj && proj.whoAmI == Projectile.whoAmI && proj.type == Projectile.type)
                    {
                        myTrail.Update();
                    }
                }
            }
        }
        public virtual void SlashPattern(Player player, int slashNumber)
        {
			float speedBonus = 0.2f;
			int damage = Projectile.damage;
			Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), PlayerCenter(), Projectile.velocity, Type, damage, Projectile.knockBack, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1] + speedBonus);
			if (proj.ModProjectile is VertebraekerSlash a)
			{
				a.distance = distance * 0.9f + 8;
			}
		}
		public virtual void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
		{
			float amt = Main.rand.NextFloat(1.0f, 1.4f) * bladeLength / 180f;
			for (int i = 0; i < amt * 0.6f; i++) //generates dust at the end of the blade
			{
				float dustScale = 1f;
				float rand = Main.rand.NextFloat(0.9f, 1.1f);
				int type = ModContent.DustType<Dusts.CopyDust4>();
				if (Main.rand.NextBool(5))
					type = DustID.RedTorch;
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + bladeDirection.SafeNormalize(Vector2.Zero) * 24, 16, 16, type);
				dust.velocity *= 0.8f / rand;
				dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
				dust.noGravity = true;
				dust.scale *= 0.2f / rand;
				dust.scale += 1.3f / rand * dustScale;
				dust.fadeIn = 0.1f;
				if (type == ModContent.DustType<Dusts.CopyDust4>())
					dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
			}
			Vector2 toProjectile = Projectile.Center - PlayerCenter();
			for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
			{
				float rand = Main.rand.NextFloat(0.9f, 1.1f);
				int type = ModContent.DustType<Dusts.CopyDust4>();
				if (Main.rand.NextBool(3))
					type = DustID.RedTorch;
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + (toProjectile.SafeNormalize(Vector2.Zero)) * 24 - toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
				dust.velocity *= 0.1f / rand;
				dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.4f, 0.9f) * rand;
				dust.noGravity = true;
				dust.scale *= 0.2f / rand;
				dust.scale += 1.1f * rand;
				dust.fadeIn = 0.1f;
				if (type == ModContent.DustType<Dusts.CopyDust4>())
					dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
			}
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.myPlayer && !player.dead)
            {
                int AbsAI0 = (int)Math.Abs(Projectile.ai[0]);
                AbsAI0--;
                SlashPattern(player, AbsAI0);
            }
        }
    }
}
		
			