using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Helpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles
{
	public class WishingStarProj : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 0;
			Projectile.timeLeft = 1200;
			Projectile.tileCollide = false;
			Projectile.penetrate = 3;
            Projectile.extraUpdates = 1;
            Projectile.localNPCHitCooldown = 20;
            Projectile.usesLocalNPCImmunity = true;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = Math.Max(Projectile.damage / 2, 1);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.X -= 16;
            hitbox.Y -= 16;
            hitbox.Width += 32;
            hitbox.Height += 32;
        }
        public Color BandColor(int i)
        {
            Color c = new Color(185, 39, 23);
            if (i == 0)
                c = new Color(122, 243, 305);
            if (i == 1)
                c = new Color(305, 170, 0);
            if (i == 2)
                c = new Color(167, 150, 300);
            if (i == 3)
                c = new Color(110, 304, 130);
            return c;
        }
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D pixel = ModContent.Request<Texture2D>("SOTS/Items/Secrets/WhitePixel").Value;
            Color color;
            for (int j = 0; j < BandLists.Length; j++)
            {
                color = BandColor(j);
                Vector2 previous = Projectile.Center + new Vector2(0, -12).RotatedBy(MathHelper.ToRadians(j * 72) + Projectile.rotation);
				float count = BandLists[j].Count;
                for (int i = 0; i < count; i++)
                {
                    Vector2 bandPosition = BandLists[j][i];
					Vector2 toPrev = previous - bandPosition;
					float percent = 1 - i / count;
                    Color c2 = color * percent;
                    Main.spriteBatch.Draw(pixel, bandPosition - Main.screenPosition, null, c2, toPrev.ToRotation(), new Vector2(0, 1), new Vector2(toPrev.Length() / 2f, 2), SpriteEffects.None, 0f);
					previous = bandPosition;
                }
            }
            return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.ai[2] == -1)
                texture = ModContent.Request<Texture2D>("SOTS/Projectiles/ShatteredDreamsProj").Value;
            Texture2D pixel = ModContent.Request<Texture2D>("SOTS/Items/Secrets/WhitePixel").Value;
            Color color = Color.White;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, 14);
            Vector2 previous = Projectile.Center;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Color c = ColorHelper.VibrantColorGradient(i * 2, false);
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Vector2 toPrev = previous - center;
                Main.spriteBatch.Draw(pixel, center - Main.screenPosition, null, c * perc, toPrev.ToRotation(), new Vector2(0, 1), new Vector2(toPrev.Length() / 2f, 6f * perc), SpriteEffects.None, 0f);
                previous = center;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(100, 100, 100, 0), Projectile.rotation * 0.5f, drawOrigin, 2f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(100, 100, 100, 0), -Projectile.rotation * 0.75f, drawOrigin, 1.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color * (1f - (Projectile.alpha / 305f)), Projectile.rotation, drawOrigin, 1.2f, SpriteEffects.None, 0f);
        }
		private List<Vector2>[] BandLists;
		private bool RunOnce = true;
		private void SimulateBands(List<Vector2> BandList, int degrees)
        {
            Vector2 awayFromProjectile = new Vector2(0, -11).RotatedBy(MathHelper.ToRadians(degrees) + Projectile.rotation);
            Vector2 pointPos = Projectile.Center + awayFromProjectile;
            int cap = SOTS.Config.lowFidelityMode ? 10 : 30;
            while (BandList.Count > cap)
            {
                BandList.RemoveAt(BandList.Count - 1);
            }
            BandList.Insert(0, pointPos + awayFromProjectile * 0.2f);
            Vector2 prev = pointPos;
			for(int j = 0; j <= 1; j++)
			{
                for (int i = 0; i < BandList.Count; i++)
                {
                    float tensionFactor = (float)i / BandLists[j].Count;
					if(j == 1)
                    {
                        BandList[i] += getBandVelocity(degrees / 72, i);
                    }
					else
					{
                        BandList[i] = Vector2.Lerp(BandList[i], prev, 0.30f + 0.5f * tensionFactor);
                    }
                    prev = BandList[i];
                }
            }
		}
        private Vector2 getBandVelocity(int j, int i)
        {
            Vector2 toProj = Projectile.Center - BandLists[j][i];
            float maxLength = 160f;
            int degrees = j * 72;
            float tensionFactor = (float)i / BandLists[j].Count;
            Vector2 awayFromProjectile = new Vector2(0, -11).RotatedBy(MathHelper.ToRadians(degrees) + Projectile.rotation);
            Vector2 velo = Vector2.Zero;
            velo += awayFromProjectile.SNormalize() * (1) * (5 + Projectile.velocity.Length() * 0.5f) * 0.2f;
            velo -= toProj * tensionFactor / maxLength;
            velo += Projectile.velocity * (1 - tensionFactor);
            return velo;
        }
		public int RotateDir => Projectile.whoAmI % 2 * 2 - 1;
		public float RotationSpeed => RotateDir * (Projectile.velocity.Length() * 0.3f + 2);
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.4f, 1f, 0.7f);
			if (RunOnce)
			{
				BandLists = new List<Vector2>[] { new List<Vector2>(), new List<Vector2>(), new List<Vector2>(), new List<Vector2>(), new List<Vector2>() };
                RunOnce = false;
				SOTSUtils.PlaySound(SoundID.Item60, Projectile.Center, 1.2f, 0.2f, 0.1f);
			}
			else
            {
                for(int i = 0; i < BandLists.Length; i++)
                {
					SimulateBands(BandLists[i], i * 72);
                }
                GenerateParticleOnBand(Main.rand.Next(5), Main.rand.Next(BandLists[0].Count), Main.rand.NextFloat(1, 1.5f));
            }
            if(Projectile.ai[0] != -1)
            {
                Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                Vector2 toDest = destination - Projectile.Center;
                if (toDest.Length() < 180)
                {
                    Projectile.ai[0] = -1;
                    if (Projectile.owner == Main.myPlayer)
                        Projectile.netUpdate = true;
                }
                else
                    Projectile.velocity += toDest * 0.0019f;
                Projectile.velocity *= 0.955f;
            }
            else
            {
                int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 480, this, false);
                if(target != -1)
                {
                    NPC npc = Main.npc[target];
                    Vector2 toNPC = npc.Center - Projectile.Center;
                    Projectile.velocity += toNPC * 0.004f;
                    Projectile.velocity *= 0.97f;
                }
            }
            Projectile.rotation += MathHelper.ToRadians(RotationSpeed);
		}
        public void GenerateParticleOnBand(int j, int i, float size)
        {
            Vector2 position = BandLists[j][i];
            Color c = BandColor(j);
            float percent = 1 - i / 30f;
            c.A = 0;
            c *= percent;
            Dust d = PixelDust.Spawn(position, 0, 0,Main.rand.NextVector2CircularEdge(1, 1), c, 6);
            d.scale = size * (0.5f + 0.5f * percent);
            if(size >= 2)
            {
                d.velocity *= 0.4f;
                d.velocity += Projectile.oldVelocity * 0.1f * percent + getBandVelocity(j, i) * 0.4f;
            }
            else
            {
                d.velocity += Projectile.velocity * 0.1f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            SOTSUtils.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.Center, 2f, -0.7f, 0.1f);
            for (int j = 0; j < BandLists.Length; j++)
            {
                for (int i = 0; i < BandLists[j].Count; i++)
                {
                    GenerateParticleOnBand(j, i, 2);
                }
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), newColor: ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180), true));
                d.velocity *= 1.5f;
                d.velocity += Projectile.oldVelocity * 0.5f;
                d.noGravity = true;
                d.fadeIn = 0.2f;
                d.scale = d.scale * 0.5f + 1.0f;
                d.color.A = 0;
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                d = Dust.NewDustDirect(center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), newColor: ColorHelper.VibrantColorGradient(i * 2, false) * perc);
                d.velocity *= 0.75f * perc;
                d.velocity += Projectile.oldVelocity * 0.4f;
                d.noGravity = true;
                d.fadeIn = 0.2f;
                d.scale = d.scale * 0.75f + 1.75f * perc;
                d.color.A = 0;
            }
        }
    }
}