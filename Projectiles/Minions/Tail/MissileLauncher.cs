using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions.Tail
{
	public class MissileLauncher : ModProjectile
	{
		public Vector2 Target
		{
			get
			{
				return new Vector2(AI2, AI3);
			}
			set
			{
				AI2 = value.X;
				AI3 = value.Y;
			}
		}
		public float AI0 = 0f;
        private ref float AI1 => ref Projectile.ai[0];
        private ref float AI2 => ref Projectile.ai[1];
        private ref float AI3 => ref Projectile.ai[2];
        public override void SendExtraAI(BinaryWriter writer)
        {

		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {

		}
        public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
            Main.projPet[Type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.DamageType = ModContent.GetInstance<VoidSummon>();
			Projectile.minionSlots = 0f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
		}
		public override void PostDraw(Color lightColor)
		{

		}
		public float DirectionFollower = 0;
		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D cannon = Terraria.GameContent.TextureAssets.Projectile[Type].Value ;
            Texture2D cannon2 = ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/Tail/MissileLauncherNoMissile").Value;
            Texture2D tail1 = ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/Tail/TailSegment").Value;
            Texture2D tail2 = ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/Tail/TailSegment2").Value;
            Texture2D tail3 = ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/Tail/TailSegment3").Value;
            Texture2D connector = ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/Tail/MissileLauncherStem").Value;
			

            float animPercent = AI0 / 100f;
            float cos = MathF.Cos(animPercent * MathF.PI);
            float sin = 0.5f + 0.5f * MathF.Sin(animPercent * MathF.PI);
			Vector2 defaultPos = player.Center + new Vector2(-38 * DirectionFollower, -24) + new Vector2(12 * sin * DirectionFollower, -4 * sin);
            Vector2 toTarget = (Target - defaultPos).SNormalize();
            float r = MathHelper.WrapAngle(toTarget.ToRotation());

            Vector2 gfx = new Vector2(0, player.gfxOffY) - Main.screenPosition;
            Vector2 prev = player.Center.ToVector2Int() + new Vector2(0, 9);

			Vector2 end = Projectile.Center;

			Vector2 position = prev;
            float spriteLen = 6;
			Vector2 toPos = prev - position;
            Vector2 b1 = prev;
            Vector2 b2 = prev + new Vector2(-34 * DirectionFollower, 12);
            Vector2 b3 = end + new Vector2(toTarget.X * -20, 12 + toTarget.Y * 4f);
            Vector2 b4 = end;
			float prevR = 0f;
			float order = 36;
            for (int i = 1; i < order; i++)
            {
				float percent = i / order;
				var draw = tail1;
                if (i == 22)
                    draw = tail3;
                if (i >= 24)
                    draw = tail3;
				float sinPer = MathF.Sin(percent * MathF.PI);
				sin = MathF.Sin(percent * MathF.PI * 3f + animPercent);
                Vector2 circularDrawOffset = new Vector2(0, sin * 2.5f * sinPer * cos).RotatedBy(prevR);
                position = SOTS.CalculateBezierPoint(percent, b1, b2, b3, b4) + circularDrawOffset;
				toPos = prev - position;
				float len = toPos.Length();
				prevR = toPos.ToRotation();
                Main.EntitySpriteDraw(draw, position + gfx, null, Lighting.GetColor((position / 16f).ToPoint()), prevR, new Vector2(0, 3), new Vector2((len + 1) / spriteLen, 1), SpriteEffects.None, 0f);
				prev = position;
            }
            //Main.spriteBatch.Draw(SOTSUtils.WhitePixel, b1 + gfx, null, Color.Red, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 0f);
            //Main.spriteBatch.Draw(SOTSUtils.WhitePixel, b2 + gfx, null, Color.Red, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 0f);
            //Main.spriteBatch.Draw(SOTSUtils.WhitePixel, b3 + gfx, null, Color.Red, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 0f);
            //Main.spriteBatch.Draw(SOTSUtils.WhitePixel, b4 + gfx, null, Color.Red, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 0f);

			SpriteEffects s = SpriteEffects.FlipVertically;
			SpriteEffects s2 = SpriteEffects.FlipVertically;
			Vector2 conOrig = new Vector2(connector.Width - 4,  4);
            float a = MathHelper.PiOver4;
			if (prevR < -MathHelper.PiOver2 || prevR > MathHelper.PiOver2)
			{
				a *= -1;
                s = 0;
				conOrig = new Vector2(connector.Width - 4, connector.Height - 4);
            }
            if (r < -MathHelper.PiOver2 || r > MathHelper.PiOver2)
                s2 = 0;

            Vector2 stemPoint = new Vector2(connector.Width - 7, connector.Height - 7).RotatedBy(prevR + MathHelper.Pi * 3 / 4f);

            float percent2 = MathHelper.Clamp( AI1 / player.SOTSPlayer().MissileTailAttackRate, 0, 1);
            int j = (int)(8 * (1 - percent2) + 0.5f);
            Main.EntitySpriteDraw(cannon, Projectile.Center + stemPoint + gfx, new Rectangle(0, 0, 8, cannon.Height), Lighting.GetColor((position / 16f).ToPoint()), r + MathF.PI, new Vector2(cannon.Width / 2 + 5 - j, cannon.Height / 2), 1, s2, 0f);
			Main.EntitySpriteDraw(cannon2, Projectile.Center + stemPoint + gfx, null, Lighting.GetColor((position / 16f).ToPoint()), r + MathF.PI, new Vector2(cannon.Width / 2 + 5, cannon.Height / 2), 1, s2, 0f);

            Main.EntitySpriteDraw(connector, Projectile.Center + gfx, null, Lighting.GetColor((position / 16f).ToPoint()), prevR + a, conOrig, 1, s, 0f);
            //Main.spriteBatch.Draw(SOTSUtils.WhitePixel, Projectile.Center + stemPoint + gfx, null, Color.Red, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 0f);
            return false;
        }
		public Vector2 FindTarget()
		{
			int i = SOTSNPCs.FindTarget_WithLos(Projectile.Center, out float dist, 4, 1200, this, -1);
			if(i >= 0)
			{
				return Main.npc[i].Center;
			}
			return Vector2.Zero;
		}
		public override void AI()
		{
			AI0++;
			float animPercent = AI0 / 100f;
			float sin = 0.5f + 0.5f * MathF.Sin(animPercent * MathF.PI);
			Player player = Main.player[Projectile.owner];

			Vector2 defaultPos = player.Center + new Vector2(-38 * DirectionFollower, -24) + new Vector2(12 * sin * DirectionFollower, -4 * sin);
			Vector2 toTarget = Target - defaultPos;
			Vector2 norm = toTarget.SNormalize();
            toTarget = norm * 12;
			toTarget.Y *= 0.5f;

			bool canAttack = !player.SOTSPlayer().MissileTailIsVanity && (player.VoidPlayer().voidMeter >= 1 || !player.VoidPlayer().safetySwitch);
            Vector2 newTarget = !canAttack ? Vector2.Zero : FindTarget();
			bool hasTarget = true;
			if(newTarget == Vector2.Zero)
			{
				hasTarget = false;
                newTarget = player.Center + new Vector2(64 * player.direction, -48f);
            }
			float speed = player.SOTSPlayer().MissileTailAttackRate;
			if(AI1 < speed)
				AI1++;

            if (Target == Vector2.Zero)
				Target = newTarget;
			else
				Target = Vector2.Lerp(Target, newTarget, 0.07f);

			DirectionFollower = float.Lerp(DirectionFollower, player.direction, 0.07f);

            float percent = AI1 / speed;
			sin = MathF.Cos(MathF.Sqrt(percent) * MathF.PI * 2f);
            Projectile.Center = defaultPos + toTarget * (0.6f + 0.4f * sin);

            if (percent >= 1 && hasTarget)
            {
                Projectile.netUpdate = true;
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, -7.5f), norm * 2, ModContent.ProjectileType<Missile>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
					VoidItem.DrainMana(player, 1);
                }
                AI1 = 0;
            }
        }
    }
    public class Missile : ModProjectile
    {
        public override bool PreDraw(ref Color lightColor)
        {
            float scaler = 1f;
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 drawOrigin = new(0, 1);
            Vector2 previous = Projectile.Center;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Vector2 toPrev = previous - center;
                float dist = toPrev.Length();
                if (dist > 1600)
                    break;
                float rot = toPrev.ToRotation();
                Vector2 stretch = new(dist / texture.Width, perc * 2f * scaler);
                Color color = ColorHelper.InfernoColorGradient(perc) * 0.5f;
                color.A = 0;
                Main.EntitySpriteDraw(texture, center - Main.screenPosition, null, Color.Lerp(Color.Red, color, 0.8f) * perc, rot, drawOrigin, stretch, SpriteEffects.FlipVertically, 0f);
                previous = center;
            }
            float r = Projectile.rotation + MathF.PI;
            var text = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(text, Projectile.Center - Main.screenPosition, null, lightColor, r,
               text.Size() / 2f, 1, Projectile.velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 33;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<VoidSummon>();
            Projectile.width = Projectile.height = 8;
            Projectile.timeLeft = 360;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[0] <= 0)
            {
                SOTSUtils.PlaySound(SoundID.Item61, Projectile.Center, 0.4f, -0.3f);
                for (int i = 0; i < 16; i++)
                {
                    float p = i / 16f;
                    Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.TwoPi * p);
                    circular.X *= 0.5f;
                    circular = circular.RotatedBy(Projectile.rotation);
                    Dust dust = PixelDust.Spawn(Projectile.Center + Projectile.velocity * 12, 0, 0, circular + Main.rand.NextVector2Circular(0.1f, 0.1f), Color.Lerp(Color.Red, ColorHelper.InfernoColorGradient(MathF.Sin(MathF.PI * p) * 0.5f + 0.5f), 0.8f));
                    dust.scale = Main.rand.NextFloat(0.8f, 1.4f);
                    dust.velocity += Projectile.velocity * Main.rand.NextFloat(1f, 1.5f);
                    dust.color.A = 0;
                }
                Projectile.ai[0]++;
            }

            int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 480, Projectile);
            if (target != -1)
            {
                ++Projectile.ai[1];
                NPC npc = Main.npc[target];
                Vector2 toNPC = npc.Center - Projectile.Center;
                toNPC = toNPC.SNormalize() * (0.1f + 0.02f * Projectile.ai[1]);
                float prevSpeed = Projectile.velocity.Length();
                Projectile.velocity += toNPC;
                Projectile.velocity = Projectile.velocity.SNormalize() * prevSpeed;
                Projectile.velocity *= 1.005f;
            }
            Projectile.velocity *= 1.0024f;
            if (Main.rand.NextBool(6))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4) - Projectile.velocity * 0.5f, 0, 0, ModContent.DustType<AlphaDrainDust>(), newColor: ColorHelper.InfernoColorGradient(Main.rand.NextFloat(0.5f, 1.0f)));
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale = 1.0f;
                dust.alpha = Projectile.alpha;
                dust.velocity = dust.velocity * 0.1f + Projectile.velocity * 0.5f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SOTSUtils.PlaySound(SoundID.Item62, Projectile.Center, 0.25f, 0.5f);
            int count = Math.Min(360 - timeLeft, 16);
            for (int i = 0; i < count; i++)
            {
                Vector2 circular = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.TwoPi * i / count);
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<AlphaDrainDust>(), newColor: ColorHelper.InfernoColorGradient(Main.rand.NextFloat(0.5f, 1.0f)));
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale = 1.6f;
                dust.alpha = Projectile.alpha;
                dust.velocity *= 0.3f;
                dust.velocity += circular * Main.rand.NextFloat(0.5f, 1.1f) + Projectile.oldVelocity * 0.3f;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Color c = ColorHelper.InfernoColorGradient(perc) * perc;
                c.A = 0;
                PixelDust.Spawn(center, 0, 0, Main.rand.NextVector2Circular(0.3f, 0.3f) * perc + Projectile.oldVelocity * 0.4f, c, 7).scale = 0.5f + perc;
            }
        }
    }
}