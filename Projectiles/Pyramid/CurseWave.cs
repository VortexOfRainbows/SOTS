using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Curse;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseWave : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse");
		}
        public override void SetDefaults()
        {
			projectile.height = 36;
			projectile.width = 36;
			projectile.friendly = false;
			projectile.timeLeft = 240;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.tileCollide = false;
			projectile.hide = true;
			//projectile.extraUpdates = 1;
		}
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindNPCs.Add(index);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(36f * projectile.scale);
			hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
		}
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public void catalogueParticles()
		{
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				CurseFoam particle = foamParticleList1[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList1.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						foamParticleList1.RemoveAt(i);
						i--;
					}
					else if (!particle.noMovement)
						particle.position += projectile.velocity * 0.925f;
				}
			}
		}
		List<Vector2> trailPos = new List<Vector2>();
		Vector2 originalCenter;
		Vector2 originalVelocity;
		bool runOnce = true;
		bool[] ignore;
		float Direction1 = 0;
		float Direction2 = 0;
		public void runPreAI(int updates = 1)
		{
			if(runOnce)
			{
				Direction1 = projectile.ai[1];
				Direction2 = Direction1;
				if (ignore == null)
				{
					ignore = new bool[Main.tileSolid.Length];
					for (int i = 0; i < ignore.Length; i++)
					{
						ignore[i] = true;
					}
					ignore[ModContent.TileType<TrueSandstoneTile>()] = false;
					ignore[ModContent.TileType<AncientGoldGateTile>()] = false;
				}
				originalCenter = projectile.Center;
				originalVelocity = projectile.velocity;
				runOnce = false;
            }
			float veloMult = 1.01f;
			float rotateMod = 0.9f;
			for(int i = 0; i < updates; i++)
			{
				if (trailPos.Count >= projectile.timeLeft)
					break;
				Vector2 temp = originalVelocity;
				originalVelocity = Collision.AdvancedTileCollision(ignore, originalCenter - new Vector2(4, 4), originalVelocity, 8, 8, true, true);
				originalCenter += originalVelocity;
				if (originalVelocity != temp)
				{
					originalVelocity = -temp;
					Direction1 = -Direction1;
				}
				originalVelocity *= veloMult;
				originalVelocity = originalVelocity.RotatedBy(MathHelper.ToRadians(rotateMod * Direction1));
				trailPos.Add(originalCenter);
			}
			if(updates == -1)
			{
				Vector2 temp = projectile.velocity;
				projectile.velocity = Collision.AdvancedTileCollision(ignore, projectile.Center - new Vector2(4, 4), projectile.velocity, 8, 8, true, true);
				projectile.Center += projectile.velocity;
				if (projectile.velocity != temp)
				{
					projectile.velocity = -temp;
					Direction2 = -Direction2;
				}
				projectile.velocity *= veloMult;
				projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(rotateMod * Direction2));
			}
			else if(trailPos.Count >= 1)
				trailPos.RemoveAt(0);
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public void DrawTelegraph(SpriteBatch spriteBatch, Color lightColor)
        {
			if(trailPos.Count > 1)
			{
				Vector2 from = trailPos[0];
				for (int i = 1; i < trailPos.Count; i++)
				{
					float alphaMult = 1.1f - (0.5f * counter / 240f) - ((float)(i - 1) / trailPos.Count);
					if (alphaMult > 1)
						alphaMult = 1;
					Vector2 to = trailPos[i];
					Vector2 toPos = from - to;
					float rotation = toPos.ToRotation();
					int length = (int)toPos.Length() + 1;
					Texture2D texture2 = ModContent.GetTexture("SOTS/Projectiles/Pyramid/CurseLineIndicator");
					Main.spriteBatch.Draw(texture2, from - Main.screenPosition, new Rectangle(0, 0, length, 2), Color.White * alphaMult, rotation, new Vector2(1, 1), 1f, SpriteEffects.None, 0f);
					from = to;
				}
			}
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override bool PreAI()
        {
			runPreAI(4);
			runPreAI(-1);
			return base.PreAI();
        }
		int counter = 0;
        public override void AI()
		{
			counter++;
			int parentID = (int)projectile.ai[0];
			if (parentID >= 0)
			{
				NPC npc = Main.npc[parentID];
				if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
				{
					if (Main.netMode != NetmodeID.Server)
					{
						if (projectile.timeLeft == 2)
						{
							PharaohsCurse curse = npc.modNPC as PharaohsCurse;
							for (int j = 0; j < 40; j++)
							{
								Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(1.05f, 3.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
								curse.foamParticleList1.Add(new CurseFoam(projectile.Center, rotational, 1.55f, true));
							}
						}
						PharaohsCurse.SpawnPassiveDust(Main.projectileTexture[projectile.type], projectile.Center, 1.1f * projectile.scale, foamParticleList1, 0.2f, 4, 40, projectile.velocity.ToRotation() + MathHelper.ToRadians(90));
					}
				}
				else
				{
					projectile.Kill();
				}
			}
			for(float i = 0; i < 1; i+= 0.25f)
			{
				foamParticleList1.Add(new CurseFoam(projectile.Center - projectile.velocity * i, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.3f, 0.3f)), 0.4f, true));
			}
			catalogueParticles();
			projectile.scale *= 0.998f; 
		}
	}
}
		