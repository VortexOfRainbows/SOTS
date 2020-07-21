using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs
{
	public class SittingMushroom : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Sitting Mushroom");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 40;   
            npc.damage = 20; 
            npc.defense = 6;  
            npc.knockBackResist = 0.1f;
            npc.width = 30;
            npc.height = 30;
			Main.npcFrameCount[npc.type] = 8;  
            npc.value = 125;
            npc.npcSlots = 1f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath16;
            npc.netAlways = true;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.velocity.X *= 0.9f;
			if(Main.rand.Next(40) == 0)
			{
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 28, 10, 41, 0, -2f, 250, new Color(100, 100, 100, 250), 0.8f);
			}
			npc.ai[0]++;
			if(npc.ai[0] >= 180)
			{
				Vector2 distance = npc.Center - player.Center;
				if(distance.Length() <= 150 || npc.ai[1] >= 1)
				{
					npc.ai[1]++;
				}
			}
			if(npc.ai[1] == 1)
			{
				npc.velocity.Y -= 7f;
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.netUpdate = true;
				}
			}
			if((int)npc.ai[1] % 7 == 0 && npc.ai[1] >= 7)
			{
				Vector2 distance = npc.Center - player.Center;
				distance.Normalize();
				distance *= -8f;
				int damage2 = npc.damage / 2;
				if (Main.expertMode)
				{
					damage2 = (int)(damage2 / Main.expertDamage);
				}
				if(Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, distance.X, distance.Y, mod.ProjectileType("SporeCloud"), damage2, 2f, Main.myPlayer);
				Main.PlaySound(2, npc.Center, 34);
			}
			if (npc.ai[1] >= 42)
			{
				npc.ai[0] = 0;
				npc.ai[1] = 0;
			}
		}
		int frame = 0;
		public override void FindFrame(int frameHeight) 
		{
			frame = frameHeight;
			float frameSpeed = 8f;
			npc.frameCounter++;
			if (npc.frameCounter >= frameSpeed) 
			{
				npc.frameCounter -= frameSpeed;
				npc.frame.Y += frame;
				if(npc.frame.Y >= 8 * frame)
				{
					npc.frame.Y = 0;
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.player.ZoneGlowshroom || spawnInfo.spawnTileType == TileID.MushroomGrass)
			{
				if(!Main.hardMode)
					return 0.2f;
				return 0.1f;
			}
			if (spawnInfo.player.ZoneRockLayerHeight)
			{
				return 0.005f;
			}
			return 0;
		}
		public override void NPCLoot()
		{
			if(npc.HasBuff(BuffID.OnFire))
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CookedMushroom"), Main.rand.Next(2) + 3);
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GlowingMushroom, Main.rand.Next(6) + 7);
			}
			if(Main.rand.Next(2) == 0 || Main.expertMode)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfEarth"), Main.rand.Next(2) + 1);

			if(Main.rand.Next(100) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SporeSprayer"), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 20.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 41, (float)(2 * hitDirection), -2f, 250, new Color(100, 100, 100, 250), 0.8f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 10; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 41, (float)(2 * hitDirection), -2f, 250, new Color(100, 100, 100, 250), 0.8f);
				}
				for (int k = 0; k < 7; k++)
				{
					Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
					int damage2 = npc.damage / 2;
					if (Main.expertMode)
					{
						damage2 = (int)(damage2 / Main.expertDamage);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, circularLocation.X, circularLocation.Y, mod.ProjectileType("SporeCloud"), damage2, 2f, Main.myPlayer);
				}
				Main.PlaySound(2, npc.Center, 34);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SittingMushroomGore1"), 1f);
			}
		}
	}
}





















