using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs
{
	public class LostSoul : ModNPC
	{	float ai1 = 0;
		float ai2 = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Lost Soul");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 44;
            npc.lifeMax = 55;
            npc.damage = 40; 
            npc.defense = 3; 
            npc.knockBackResist = 0.8f;
            npc.width = 28;
            npc.height = 40;
			Main.npcFrameCount[npc.type] = 6;  
            npc.value = 550;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.netAlways = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.DeathSound = null;
		}
		public override void AI()
		{
				int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 28, 24, mod.DustType("LostSoulDust"));
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity.X = npc.velocity.X;
				Main.dust[num1].velocity.Y = -3;
				Main.dust[num1].alpha = npc.alpha;
				npc.dontTakeDamage = false;
				
				if(ai2 == 0)
				{
					npc.alpha++;
					if(npc.alpha >= 235)
					{
						ai2 = -1;
					}
				}
				if(ai2 == -1)
				{
					npc.alpha--;
					if(npc.alpha <= 20)
					{
						ai2 = 0;
					}
				}
			ai1++;
			npc.rotation = 0;
		}
		int frame = 0;
		public override void FindFrame(int frameHeight) 
		{
			frame = frameHeight;
			
				if (ai1 >= 5f) 
				{
					ai1 -= 5f;
					npc.frame.Y += frame;
					if(npc.frame.Y >= 6 * frame)
					{
						npc.frame.Y = 0;
					}
				}
				
			
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.player.GetModPlayer<SOTSPlayer>().PyramidBiome && spawnInfo.spawnTileType == (ushort)mod.TileType("PyramidSlabTile"))
			{
				return 0.6f;
			}
			return 0;
		}
		public override void NPCLoot()
		{
			if(Main.rand.Next(2) == 0 || Main.expertMode)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SoulResidue"), Main.rand.Next(2) + 1);	
		}	
	}
}