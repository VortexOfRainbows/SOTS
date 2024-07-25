using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Projectiles.BiomeChest;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class BundleSnakeMinion : ModProjectile
	{
		private float aiCounter2
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 3;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
			Projectile.hide = true;
			Projectile.idStaticNPCHitCooldown = 30;
			Projectile.usesIDStaticNPCImmunity = true;
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindProjectiles.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
			Draw(Main.spriteBatch, Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16));
			return false;
		}
		public void Draw(SpriteBatch spriteBatch, Color drawColor)
        {
			if (pastParent != null && Projectile.timeLeft >= 4)
            {
                bool flip = false;
                if (Math.Abs(MathHelper.WrapAngle(Projectile.rotation)) <= MathHelper.ToRadians(90))
                {
                    flip = true;
                }
                float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;

                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/BundleSnakeMinionBody");
                Vector2 drawOrigin = new Vector2(0, texture.Height / 2);
                Vector2 myCenter = Projectile.Center - new Vector2(12, 2 * (flip ? -1 : 1)).RotatedBy(Projectile.rotation);
                Vector2 p0 = pastParent.Center;
                Vector2 p1 = pastParent.Center;
                Vector2 p2 = myCenter - new Vector2(20, 2 * (flip ? -1 : 1)).RotatedBy(Projectile.rotation);
                Vector2 p3 = myCenter;
                int segments = 24;
                for (int i = 0; i < segments; i++)
                {
                    float t = i / (float)segments;
                    Vector2 drawPos2 = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
                    t = (i + 1) / (float)segments;
                    Vector2 drawPosNext = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
                    Vector2 toNext = (drawPosNext - drawPos2);
                    float rotation = toNext.ToRotation();
                    float distance = toNext.Length();
                    drawColor = Lighting.GetColor((int)drawPos2.X / 16, (int)(drawPos2.Y / 16));
                    spriteBatch.Draw(texture, drawPos2 - Main.screenPosition, null, Projectile.GetAlpha(drawColor), rotation, drawOrigin, Projectile.scale * new Vector2((distance + 4) / (float)texture.Width, 1), SpriteEffects.None, 0f);
                }
                Vector2 drawPos = Projectile.Center - Main.screenPosition;
                texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                drawOrigin = new Vector2(texture.Width / 2, texture.Height / 6);
                spriteBatch.Draw(texture, drawPos, new Rectangle(0, texture.Height / 3 * Projectile.frame, texture.Width, texture.Height / 3), Projectile.GetAlpha(drawColor), Projectile.rotation - bonusDir, drawOrigin, Projectile.scale, flip ? SpriteEffects.FlipHorizontally : 0, 0f);
            }
        }
        Projectile pastParent = null;
		public Projectile getParent()
		{
			Projectile parent = pastParent;
			if (parent != null && parent.active && parent.owner == Projectile.owner && (parent.minion || parent.type == ModContent.ProjectileType<CrystalSerpentHead>()) && parent.identity == (int)(Projectile.ai[0] + 0.5f)) //this is to prevent it from iterating the loop over and over
			{
				return parent;
			}
			else
            {
                parent = null;
            }
			for (short i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.owner == Projectile.owner && (proj.minion || proj.type == ModContent.ProjectileType<CrystalSerpentHead>()) && proj.identity == (int)(Projectile.ai[0] + 0.5f)) //use identity since it aids with server syncing (.whoAmI is client dependent)
				{
					parent = proj;
					break;
				}
			}
			pastParent = parent;
            return parent;
		}
		public Vector2 FindTarget(Vector2 Center)
		{
			Player player = Main.player[Projectile.owner];
			float distanceFromTarget = 220f;
			Vector2 targetCenter = Vector2.Zero;
			bool foundTarget = false;
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Center);
				bool lineOfSight = Collision.CanHitLine(Center - new Vector2(8, 8), 16, 16, npc.position, npc.width, npc.height);
				if (between < distanceFromTarget && lineOfSight)
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
			if (!foundTarget)
			{
				int i = SOTSNPCs.FindTarget_Basic(Center, distanceFromTarget, Projectile, true);
				if(i != -1)
                {
                    NPC npc = Main.npc[i];
					targetCenter = npc.Center;
                }
            }
            return targetCenter;
        }
        Vector2 toTarget;
        Vector2 lastOwnerPosition;
        int counter = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Projectile owner = getParent();
			counter++;
            if (Projectile.owner == Main.myPlayer)
            {
                if (counter % 2 == 0)
                    Projectile.netUpdate = true;
                if (owner == null || !owner.active || player.dead || !player.active || Projectile.damage != modPlayer.BundleSnakeDamage)
                {
                    Projectile.Kill();
                    return;
                }
            }
            else
            {
                Projectile.timeLeft = 100;
            }
			if (owner == null)
				return;
            Vector2 target = FindTarget(owner.Center);
            if (target == Vector2.Zero)
            {
                Projectile.velocity.Y += MathF.Sin(MathHelper.ToRadians(counter * 3)) * 0.2f;
                Vector2 combinedVelo = player.velocity;
                if(combinedVelo.Length() > 1)
                    toTarget = combinedVelo.SafeNormalize(Vector2.Zero);
                Projectile.velocity *= 0.95f;
            }
			else
                toTarget = target - Projectile.Center;

            if(toTarget == Vector2.Zero)
            {
                toTarget = Vector2.UnitX;
            }
            toTarget = toTarget.SafeNormalize(Vector2.Zero);

            if (aiCounter2 >= 0)
			{
				Vector2 toOwner = owner.Center - Projectile.Center - new Vector2(0, 24 + owner.height / 2);
				float speed = toOwner.Length();
				if (speed > 16)
					speed = 16;
				if(toOwner.Length() > 2000)
				{
					Projectile.Center = owner.Center;
				}
				else
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, toOwner.SafeNormalize(Vector2.Zero) * speed, 0.05f);
                }
                Projectile.velocity *= 0.95f;
            }
			else
            {
                Projectile.velocity *= 0.985f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget.SafeNormalize(Vector2.Zero) * 9, 0.02f);
            }
            if(lastOwnerPosition == Vector2.Zero)
            {
                lastOwnerPosition = owner.position;
            }
            Projectile.Center += (owner.position - lastOwnerPosition);
            lastOwnerPosition = owner.position;
            if (Projectile.velocity.Length() > 16)
			{
				Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 16f;
            }
            aiCounter2++;
            if (aiCounter2 >= 0)
            {
				Projectile.friendly = false;
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 4)
                {
                    Projectile.frame --;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame < 0)
                {
                    Projectile.frameCounter = Projectile.frame = 0;
                }
                Projectile.rotation = toTarget.ToRotation();
            }
            else
            {
                Projectile.friendly = true;
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 3)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 2)
                {
                    Projectile.frame = 2;
                    Projectile.frameCounter = 0;

                }
            }
            if (target != Vector2.Zero)
            {
                if (aiCounter2 >= 30)
                {
                    Projectile.velocity += toTarget * 12f;
                    aiCounter2 = -15;
                    //SOTSUtils.PlaySound(SoundID.Item96, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.2f, -0.1f);
                }
				else
                {
                    Projectile.velocity += toTarget * 0.5f;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Venom, 90);
        }
    }
}