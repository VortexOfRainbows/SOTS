using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class Queen : ModNPC
	{	int start = 0;
		int faze2 = 0;
		int faze3 = 0;
		int despawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Queen");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
            npc.lifeMax = 27500;   //boss life
            npc.damage = 55;  //boss damage
            npc.defense = 4;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 36;
            npc.height = 76;
			 Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 100000;
            npc.npcSlots = 0f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = false;
			bossBag = mod.ItemType("QueenBag");
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.66f);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 1.25f);  //boss damage increase in expermode
        }
		public override void BossLoot(ref string name, ref int potionType)
		{ 
		potionType = ItemID.GreaterHealingPotion;
	
		if(Main.expertMode)
		
		{ 
		npc.DropBossBags();
		} 
		else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("QueenSkip"),Main.rand.Next(1, 4)); 
				}
				}
		public override void AI()
		{
			npc.timeLeft = 6;
		 Player player  = Main.player[npc.target];
		 start++;
		 
		 if(Main.expertMode)
		 npc.ai[2]++;
	 
		 npc.ai[2]++;
		 
			 SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			 
			
		 if(start >= 24920 || modPlayer.chessSkip == true)
		 {
			npc.dontTakeDamage = false;
			npc.aiStyle = 0;
		}
		else
		{
			npc.aiStyle = 0;
			npc.dontTakeDamage = true;
			
		}
		{ //dashy attack #1
			if(npc.ai[2] >= 240)
			{
				if(player.position.X - 80 > npc.position.X && npc.ai[2] == 120)
				{
					npc.velocity.X += 20;
				}
				if(player.position.X + 80 < npc.position.X && npc.ai[2] == 120)
				{
					npc.velocity.X -= 20;
				}
				if(player.position.Y -80 > npc.position.Y)
				{
					npc.velocity.Y += 11.5f;
				}
				if(player.position.Y +80 < npc.position.Y)
				{
					npc.velocity.Y -= 11.5f;
				}
					
					npc.ai[2] = 0;
				
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-2,3), -4, mod.ProjectileType("CrossSnow"), 42, 1, 0);
				
				
				
				
				
				
				}
				if(npc.ai[2] == 15)
				{
					npc.velocity.X = 0;
					npc.velocity.Y = 0;
				}
			}
			if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 720)
			{
			npc.active = false;
			}
		}
	}
}