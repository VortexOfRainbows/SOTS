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
			// DisplayName.SetDefault("Curse");
		}
        public override void SetDefaults()
        {
			Projectile.height = 36;
			Projectile.width = 36;
			Projectile.friendly = false;
			Projectile.timeLeft = 240;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			//Projectile.extraUpdates = 1;
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCs.Add(index);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(36f * Projectile.scale);
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
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
						particle.position += Projectile.velocity * 0.925f;
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
				Direction1 = Projectile.ai[1];
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
				originalCenter = Projectile.Center;
				originalVelocity = Projectile.velocity;
				runOnce = false;
            }
			float veloMult = 1.01f;
			float rotateMod = 0.9f;
			for(int i = 0; i < updates; i++)
			{
				if (trailPos.Count >= Projectile.timeLeft)
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
				Vector2 temp = Projectile.velocity;
				Projectile.velocity = Collision.AdvancedTileCollision(ignore, Projectile.Center - new Vector2(4, 4), Projectile.velocity, 8, 8, true, true);
				Projectile.Center += Projectile.velocity;
				if (Projectile.velocity != temp)
				{
					Projectile.velocity = -temp;
					Direction2 = -Direction2;
				}
				Projectile.velocity *= veloMult;
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotateMod * Direction2));
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
					Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Pyramid/CurseLineIndicator");
					Main.spriteBatch.Draw(texture2, from - Main.screenPosition, new Rectangle(0, 0, length, 2), Color.White * alphaMult, rotation, new Vector2(1, 1), 1f, SpriteEffects.None, 0f);
					from = to;
				}
			}
        }
        public override bool PreDraw(ref Color lightColor)
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
			int parentID = (int)Projectile.ai[0];
			if (parentID >= 0)
			{
				NPC npc = Main.npc[parentID];
				if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
				{
					if (Main.netMode != NetmodeID.Server)
					{
						if (Projectile.timeLeft == 2)
						{
							PharaohsCurse curse = npc.ModNPC as PharaohsCurse;
							for (int j = 0; j < 30; j++)
							{
								Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(1.05f, 3.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
								curse.foamParticleList1.Add(new CurseFoam(Projectile.Center, rotational, 1.55f, true));
							}
						}
						PharaohsCurse.SpawnPassiveDust(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center, 1.1f * Projectile.scale, foamParticleList1, 0.2f, 4, 50, Projectile.velocity.ToRotation() + MathHelper.ToRadians(90));
					}
				}
				else
				{
					Projectile.Kill();
				}
            }
            if (Main.netMode != NetmodeID.Server)
            {
                for (float i = 0; i < 1; i += 0.34f)
                {
                    foamParticleList1.Add(new CurseFoam(Projectile.Center - Projectile.velocity * i, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.3f, 0.3f)), 0.4f, true));
                }
            }
			catalogueParticles();
			Projectile.scale *= 0.998f; 
		}
	}
}
		