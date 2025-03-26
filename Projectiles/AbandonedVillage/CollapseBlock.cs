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

namespace SOTS.Projectiles.AbandonedVillage
{    
    public class CollapseBlock : ModProjectile 
    {
        public static void Spawn(IEntitySource spawn, int i, int j)
		{
			Vector2 pos = new Vector2(i * 16 + 8, j * 16 + 8);
			SOTSUtils.PlaySound(SoundID.Item62, pos, 0.8f, -0.5f);
            int origI = i;
			int origJ = j;
			for(int a = 0; a < 6; a++)
            {
				pos = new Vector2(i * 16 + 8, j * 16 + 8);
                Projectile.NewProjectile(spawn, pos, Main.rand.NextVector2Circular(4, 4), ModContent.ProjectileType<CollapseBlock>(), 10, 0, Main.myPlayer, 0, Main.rand.Next(2), 0);
				i = origI + Main.rand.Next(-a, a + 1);
				j = origJ + Main.rand.Next(-a, a + 1);
            }
        }
        public override bool PreDraw(ref Color lightColor)
		{
			int type = (int)Projectile.ai[0];
			Texture2D tileTexture = TextureAssets.Tile[type].Value;
			int frameX = 162 + (int)Projectile.ai[1] * 18; //18 * 9
			int frameY = 54; //18 * 3
			Rectangle frame = new Rectangle(frameX, frameY, 16, 16);
            Main.EntitySpriteDraw(tileTexture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, Vector2.One * 8, Projectile.scale * 1.25f, SpriteEffects.None, 0);
            return false;
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
		public override void AI()
		{
			if (Projectile.ai[0] == 0)
            {
				Projectile.ai[0] = Main.rand.NextFromList(ModContent.TileType<SootBlockTile>(), ModContent.TileType<SootSlabTile>(), WorldGen.crimson ? ModContent.TileType<CrimsonSootTile>() : ModContent.TileType<CorruptionSootTile>());
			}
            int i = (int)(Projectile.Center.X / 16);
			int j =	(int)(Projectile.Center.Y / 16);
			if(!Main.tile[i, j].HasTile && Projectile.ai[2] >= 0)
				++Projectile.ai[2];
			Projectile.tileCollide = Projectile.ai[2] >= 5;
            Projectile.frameCounter++;
			Projectile.velocity.Y += 0.1f;
			Projectile.velocity.X *= 0.985f;
			Projectile.rotation += Projectile.velocity.X * 0.01f;

			int dustType = Projectile.ai[0] == ModContent.TileType<CrimsonSootTile>() ? ModContent.DustType<CrimsonSootDust>() : Projectile.ai[0] == ModContent.TileType<CorruptionSootTile>() ? ModContent.DustType<CorruptionSootDust>() : ModContent.DustType<SootDust>();

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
                SOTSUtils.PlaySound(SoundID.Item62, Projectile.Center, 0.8f, 0.8f);
				for(int a = 0; a < 12; ++a)
				{
                    Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(1, 1) - Projectile.velocity, 16, 16, dustType);
                    d.noGravity = false;
                    d.velocity *= 1.25f;
					d.scale *= 1.25f;
                    d.velocity += Projectile.velocity * 0.55f;
                }
				Projectile.Kill();
            }
        }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			Projectile.tileCollide = false;
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X * 0.8f;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y * 0.25f;
			}
			Projectile.ai[2] = -1;
			Projectile.netUpdate = true;
            return false;
		}
	}
}
		