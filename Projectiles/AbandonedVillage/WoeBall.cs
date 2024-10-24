using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.BiomeChest;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.AbandonedVillage
{
	public class WoeBall : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public sealed override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
			Projectile.hide = true;
			Projectile.localNPCHitCooldown = 24;
			Projectile.usesLocalNPCImmunity = true;
            Projectile.alpha = 70;
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindProjectiles.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (pastParent != null && Projectile.timeLeft >= 4)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/AbandonedVillage/WoeTail");
                Vector2 drawOrigin = new Vector2(texture.Width / 2, 0);
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
                    Color c = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));
                    float sinusoid = MathF.Sin(MathHelper.ToRadians(i * 12 + Projectile.ai[1] * -2f));
                    float rot = toPrev.ToRotation();
                    Vector2 stretch = new Vector2(perc, dist / texture.Height * 1.05f);
                    for (int j = -1; j <= 1; j += 2)
                    {
                        Vector2 helix = new Vector2(0, 12 * sinusoid * MathF.Sin(perc * MathF.PI) * j * perc).RotatedBy(rot);
                        Main.EntitySpriteDraw(texture, center + helix - Main.screenPosition, null, Projectile.GetAlpha(c) * perc, rot - MathHelper.PiOver2, drawOrigin, new Vector2(stretch.X * 0.5f, stretch.Y), SpriteEffects.FlipVertically, 0f);
                    }
                    Main.EntitySpriteDraw(texture, center - Main.screenPosition, null, Projectile.GetAlpha(c) * perc, rot - MathHelper.PiOver2, drawOrigin, stretch, SpriteEffects.FlipVertically, 0f);
                    previous = center;
                }
                Vector2 drawPos = Projectile.Center - Main.screenPosition;
                texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                lightColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16));
                drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                for (int j = 0; j < 4; j++)
                {
                    Vector2 circular = new Vector2(4 * MathF.Sin(Projectile.ai[1] / 60f * MathF.PI), 0).RotatedBy(MathHelper.ToRadians(j * 90 + Projectile.ai[1] * 1.5f));
                    Main.EntitySpriteDraw(texture, drawPos + circular, null, Projectile.GetAlpha(lightColor) * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, 0, 0f);
                }
                Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, 0, 0f);
            }
            return false;
        }
        private Projectile pastParent = null;
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
        private int counter = 0;
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
                if (owner == null || !owner.active || player.dead || !player.active || Projectile.damage != modPlayer.LittleWoeDamage)
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
            Projectile.ai[1] += owner.direction != 0 ? owner.direction : SOTSUtils.SignNoZero(owner.velocity.X);
            Vector2 targetPosition = owner.Center + new Vector2(20 + Projectile.Size.Length(), 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * 3.3f + owner.whoAmI * 45));
            Vector2 toTarget = targetPosition - Projectile.Center;
            float dist = toTarget.Length();
            float speed = dist * 0.01f + 1.0f;
            if (speed > dist)
                speed = dist;
            Projectile.velocity *= 0.825f;
            Projectile.velocity += toTarget.SNormalize() * speed;
            Projectile.rotation += 0.04f * Projectile.velocity.Length() * SOTSUtils.SignNoZero(owner.direction);
            if(Main.rand.NextBool())
            {
                PixelDust.Spawn(Projectile.Center, 0, 0, Main.rand.NextVector2Circular(2f, 2f), Helpers.ColorHelper.AVDustColor * 0.85f, -7);
            }
        }
    }
}