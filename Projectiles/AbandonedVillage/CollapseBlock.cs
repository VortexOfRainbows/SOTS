using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;
using SOTS.FakePlayer;
using SOTS.Items.AbandonedVillage;
using static SOTS.Items.AbandonedVillage.CrimsonSoot;
using static SOTS.Items.AbandonedVillage.CorruptionSoot;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.WorldgenHelpers;
using Terraria.ID;
using Terraria.Graphics.CameraModifiers;

namespace SOTS.Projectiles.AbandonedVillage
{    
    public class CollapseBlock : ModProjectile 
    {
        public static void Spawn(IEntitySource spawn, int i, int j, int damage, Vector2 veloOffset = default)
		{
            if (veloOffset == default)
                veloOffset = Vector2.Zero;
            Vector2 pos = new Vector2(i * 16 + 8, j * 16 + 8);
            SOTSUtils.PlaySound(SoundID.Item62, pos, 0.6f, -0.5f);
            if(SOTS.Config.screenShake)
            {
                PunchCameraModifier modifier = new PunchCameraModifier(pos, Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), 16f, 6f, 20, 1000f);
                Main.instance.CameraModifiers.Add(modifier);
            }
			if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                int origI = i;
                int origJ = j;
                for (int a = 0; a < 5; a++)
                {
                    pos = new Vector2(i * 16 + 8, j * 16 + 8);
                    Projectile.NewProjectile(spawn, pos, Main.rand.NextVector2Circular(1, 1) + veloOffset, ModContent.ProjectileType<CollapseBlock>(), damage, 0, Main.myPlayer, 0, Main.rand.Next(2), pos.Y + 80);
                    i = origI + Main.rand.Next(-1, 2);
                    j = origJ + Main.rand.Next(-1, 2);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
		{
			int type = (int)Projectile.ai[0];
			Texture2D tileTexture = TextureAssets.Tile[type].Value;
			int frameX = 162 + (int)Projectile.ai[1] * 18; //18 * 9
			int frameY = 54; //18 * 3
			Rectangle frame = new Rectangle(frameX, frameY, 16, 16);
            DrawTrail();
            float percent = counter / 24f;
            if (percent > 1)
                percent = 1;
            Color c = Color.Lerp(lightColor, Color.White, 0.2f);
            if(percent < 1)
            {
                float sin = MathF.Sin(percent * MathF.PI);
                Texture2D glow = ModContent.Request<Texture2D>("SOTS/Assets/Glow").Value;
                Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, ExcavatorOrb.Color * sin * 0.7f, Projectile.rotation, glow.Size() / 2, Projectile.scale * 0.85f, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(tileTexture, Projectile.Center - Main.screenPosition, frame, c, Projectile.rotation, Vector2.One * 8, Projectile.scale * 1.25f, SpriteEffects.None, 0);
            return false;
        }
        public void DrawTrail()
        {
            float scaler = 1f;
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 drawOrigin = new Vector2(0, 1);
            Vector2 previous = Projectile.Center;
            Color c = Color.Lerp(new Color(44, 38, 33), ExcavatorOrb.Color, 0.3f);
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
                Vector2 stretch = new Vector2(dist / texture.Width, perc * 6f * scaler);
                Main.EntitySpriteDraw(texture, center - Main.screenPosition, null, c * perc * 0.7f, rot, drawOrigin, stretch, SpriteEffects.FlipVertically, 0f);
                previous = center;
            }
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.height = 16;
			Projectile.width = 16;
			Projectile.friendly = false;
			Projectile.timeLeft = 600;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
		}
        private int counter = 0;
        private int dustType = 0;
		public override void AI()
		{
			if (Projectile.ai[0] == 0)
            {
				Projectile.ai[0] = Main.rand.NextFromList(ModContent.TileType<SootBlockTile>(), ModContent.TileType<SootSlabTile>(), WorldGen.crimson ? ModContent.TileType<CrimsonSootTile>() : ModContent.TileType<CorruptionSootTile>());
			    dustType = Projectile.ai[0] == ModContent.TileType<CrimsonSootTile>() ? ModContent.DustType<CrimsonSootDust>() : Projectile.ai[0] == ModContent.TileType<CorruptionSootTile>() ? ModContent.DustType<CorruptionSootDust>() : ModContent.DustType<SootDust>();
                for (int a = 0; a < 15; ++a)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(1, 1) - Projectile.velocity, 16, 16, dustType);
                    d.noGravity = false;
                    d.velocity *= 1.6f;
                    d.scale *= 1.6f;
                    d.velocity -= Projectile.velocity * 0.25f;
                }
            }
            int i = (int)(Projectile.Center.X / 16);
			int j =	(int)(Projectile.Center.Y / 16);
			if(!Main.tile[i, j].HasTile && counter >= 0 && Projectile.Center.Y > Projectile.ai[2])
				++counter;
			Projectile.tileCollide = counter >= 6;
            Projectile.frameCounter++;
			Projectile.velocity.Y += 0.1f;
			Projectile.velocity.X *= 0.985f;
			Projectile.rotation += Projectile.velocity.X * 0.01f;


            if (Projectile.tileCollide)
            {
				if(Main.rand.NextBool(12))
                {
                    Color c = SOTSTile.EarthenPlatingLight.ToColor() * 3;
                    c.A = 0;
                    PixelDust.Spawn(Projectile.Center - Projectile.velocity.SNormalize() * 10, 0, 0, Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(.24f, .24f), c, 5).scale = 1.0f;
                }
				else if(Main.rand.NextBool(3))
                {
                    Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(1, 1) - Projectile.velocity, 16, 16, dustType);
                    d.scale *= 0.2f;
                    d.scale += 1f;
                    d.noGravity = false;
                    d.velocity *= 0.1f;
                    d.velocity += Projectile.velocity * 0.2f;
                }
            }
			else if(Projectile.ai[2] < 0)
			{
				Projectile.Kill();
            }
        }
        public override void OnKill(int timeLeft)
        {
            SOTSUtils.PlaySound(SoundID.Dig, Projectile.Center, 0.7f, -0.5f, 0.2f);
            for (int a = 0; a < 12; ++a)
            {
                Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(1, 1) - Projectile.velocity, 16, 16, dustType);
                d.noGravity = false;
                d.velocity *= 1.25f;
                d.scale *= 1.25f;
                d.velocity -= Projectile.velocity * 0.25f;
            }
            Color c = Color.Lerp(new Color(44, 38, 33), ExcavatorOrb.Color, 0.3f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Dust d = Dust.NewDustDirect(center - new Vector2(4, 4), 0, 0, SOTSUtils.TypeHelper.CopyDust4Type, newColor: c * perc * 0.5f);
                d.velocity = d.velocity * perc * 0.5f + Projectile.oldVelocity * 0.4f;
                d.fadeIn = 0.2f;
                d.noGravity = true;
                d.scale = 0.5f + 1.5f * perc;
            }
        }
	}
}
		