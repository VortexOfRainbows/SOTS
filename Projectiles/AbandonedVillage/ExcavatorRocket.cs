using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.WorldgenHelpers;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.AbandonedVillage
{    
    public class ExcavatorRocket : ModProjectile 
    {
        public void ConfirmTargetSpot()
        {
            int failsAllowed = 80;
            Point t = target.ToTileCoordinates();
            while(SOTSWorldgenHelper.TrueTileSolid(t.X, t.Y))
            {
                Vector2 toProj = Projectile.Center - target;
                toProj = toProj.SNormalize() * 16 + Main.rand.NextVector2Circular(15, 15);
                Projectile.ai[0] += toProj.X;
                Projectile.ai[1] += toProj.Y;
                t = target.ToTileCoordinates();
                if(--failsAllowed < 0)
                {
                    return;
                }
            }
        }
		public Vector2 target => new Vector2(Projectile.ai[0], Projectile.ai[1]);
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 960;
        }
        public override void SetDefaults()
        {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
            Projectile.timeLeft = 540;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
		}
        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = height = 12;
            return true;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if(Projectile.timeLeft < 30)
			{
                int size = 40;
                hitbox = new Rectangle((int)(Projectile.Center.X - size), (int)(Projectile.Center.Y - size), size * 2, size * 2);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (Projectile.timeLeft >= 30)
				Projectile.timeLeft = 31;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft == 540)
                return false;
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/Projectiles/AbandonedVillage/ExcavatorRocketCross").Value;
            Vector2 origin = texture.Size() / 2;
            Color color = Color.White;
            float percent2 = MathF.Min(Projectile.timeLeft / 30f, 1);
            float percent = MathF.Min(1, Projectile.ai[2] / 36f);
            float percent3 = percent2 * percent;
            float sin = MathF.Sin(percent3 * MathF.PI) * 0.9f + MathF.Sqrt(percent3) * 1.2f;
            float r = MathF.PI * MathF.Sqrt(percent);
            float bonusScale = 1.0f - 0.075f * MathF.Sin(Projectile.ai[2] / 30f * MathF.PI) * percent;
            float spawnPercent = MathF.Sin(MathF.PI * percent);
            float scale = Projectile.scale * sin * bonusScale;
            for (int i = 0; i < 4; ++i)
            {
                Vector2 corners = new Vector2(16, 16).RotatedBy(i * MathHelper.PiOver2 + r) * scale;
                Vector2 toCorners = corners + target - Projectile.Center;
                Main.spriteBatch.Draw(SOTSUtils.WhitePixel, Projectile.Center - Main.screenPosition, null, new Color(251, 129, 13, 50) * spawnPercent * 0.2f, toCorners.ToRotation(), Vector2.UnitY, new Vector2(toCorners.Length() / 2f, 1f), SpriteEffects.None, 0f);
            }
            for (int i = 0; i < 4; ++i)
            {
                Vector2 circular = new Vector2(0, 2).RotatedBy(i * MathHelper.PiOver2);
                Main.spriteBatch.Draw(texture, circular + target - Main.screenPosition, null, new Color(251, 129, 13, 50) * percent3 * 0.9f, r, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, target - Main.screenPosition, null, new Color(191, 20, 61) * percent3, r, origin,scale, SpriteEffects.None, 0f);
            return Projectile.alpha != 255;
		}
		public override void PostDraw(Color lightColor)
        {
            if (Projectile.timeLeft <= 30 || Projectile.alpha == 255)
            {
                float percent = Projectile.timeLeft / 30f;
                float sin = MathF.Sqrt(MathF.Abs( MathF.Sin(percent * MathF.PI)));
                float scale = sin * 1.5f;

                Texture2D ProjTexture = TextureAssets.Projectile[ModContent.ProjectileType<FoundryFireBoom>()].Value;

                Vector2 drawOrigin = ProjTexture.Size() / 2;

                Vector2 vector = Projectile.Center - Main.screenPosition;

                Color color1 = new Color(251, 129, 13, 50) * scale * 0.15f;
                Color color2 = new Color(255, 255, 255, 0) * scale * 0.2f;

                for (int i = 0; i < 360; i += 90)
                {
                    Vector2 circular = new Vector2(Main.rand.NextFloat(1f, 2f), Main.rand.NextFloat(1f, 2f)).RotatedBy(MathHelper.ToRadians(i));

                    Main.EntitySpriteDraw(ProjTexture, vector + circular, null, color1, Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(ProjTexture, vector + circular, null, color2, Projectile.rotation, drawOrigin, scale * 0.7f, SpriteEffects.None, 0);
                }
            }
        }
        private bool RunOnce = true;
        public bool SpawnedInWall = false;
        public override void AI()
		{
            if(RunOnce)
            {
                ConfirmTargetSpot();
                RunOnce = false;
                Point p = Projectile.Center.ToTileCoordinates();
                SpawnedInWall = !SOTSWorldgenHelper.Empty(p.X - 1, p.Y - 1, 3, 3, 2, true);
            }
            else if(!SpawnedInWall)
            {
                Projectile.tileCollide = true;
            }
            else
            {
                Point p = Projectile.Center.ToTileCoordinates();
                SpawnedInWall = !SOTSWorldgenHelper.Empty(p.X - 1, p.Y - 1, 3, 3, 2, true);
            }
            //Main.NewText(target + ": " + Main.myPlayer + ": " + Projectile.owner);
            float approaching = ((540f - Projectile.timeLeft) / 540f);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			Lighting.AddLight(Projectile.Center, 0.5f, 0.65f, 0.75f);
			Vector2 norm = Projectile.velocity.SNormalize();
			Color c = new Color(251, 129, 13, 100);
            Dust d = Dust.NewDustDirect(Projectile.Center + new Vector2(-4, -4) - norm * 28, 0, 0, DustID.Smoke, 0, 0, Projectile.alpha, default, 1.4f);
			d.noGravity = true;
            d.velocity *= 0.2f;

            d = PixelDust.Spawn(Projectile.Center - norm * 28, 0, 0, -norm * Main.rand.NextFloat(4) + Main.rand.NextVector2Circular(0.45f, 0.45f), c, 4);
			Vector2 toTarget = target - Projectile.Center;
            float lengthSquared = toTarget.LengthSquared();
            if (lengthSquared < 256)
			{
				if(Projectile.timeLeft > 30)
				{
					Projectile.Center = target;
					Projectile.timeLeft = 30;
					Projectile.netUpdate = true;
                    Projectile.velocity *= 0f;
                }
			}
			else if(Projectile.timeLeft > 30 && lengthSquared > 0)
            {
                float distToTarget = MathF.Sqrt(lengthSquared);
                toTarget = toTarget / distToTarget; //may as well normalize manually since we have the value needed
                float bonusMultiplier = MathF.Max(0, 1 - distToTarget / 160f);
                float timeLeftPercent = MathF.Min(1, (Projectile.timeLeft - 300) / 240f);
				float iPercent = MathF.Min(1, 1 - timeLeftPercent + bonusMultiplier * 0.5f);
				float length = Projectile.velocity.Length();
                Projectile.velocity = Vector2.Lerp(Projectile.velocity * 0.98f, toTarget * 1.75f, iPercent).SNormalize() * length;
				if (length < 12)
				{
					Projectile.velocity = Projectile.velocity.SNormalize() * (length * 1.01f + 0.05f);
				}
				else
					Projectile.velocity = Projectile.velocity.SNormalize() * 12;
            }
			Projectile.ai[2]++;
			if(Projectile.timeLeft == 30)
			{
				SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.4f);
			}
			if (Projectile.timeLeft <= 30)
            {
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
				Projectile.alpha = 255;
				Vector2 spawnCenter = Projectile.Center;
				Vector2 spawnDimensions = new(12, 12);
				spawnCenter -= spawnDimensions / 2f;
				if(Projectile.timeLeft > 25)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        d = PixelDust.Spawn(spawnCenter, (int)spawnDimensions.X, (int)spawnDimensions.Y, Main.rand.NextVector2Circular(6f, 6f), c, 3);
                        d.scale = Main.rand.NextFloat(1.5f, 2.5f);

                        d = Dust.NewDustDirect(spawnCenter - new Vector2(4), (int)spawnDimensions.X, (int)spawnDimensions.Y, ModContent.DustType<CopyDust4>(), newColor: Color.Lerp(new Color(191, 20, 61, 0), c, Main.rand.NextFloat(1)));
                        d.velocity *= 2.5f; 
                        d.velocity += Projectile.oldVelocity * 0.5f;
                        d.noGravity = true;
                        d.fadeIn = 0.2f;
                        d.scale = d.scale * 1.1f + 0.9f;
                        d.color.A = 0;
                    }
                }
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.timeLeft = 31;
			Projectile.tileCollide = false;
			Projectile.velocity *= 0f;
            Projectile.alpha = 255;
			Projectile.netUpdate = true;
            return false;
        }
    }
}
		