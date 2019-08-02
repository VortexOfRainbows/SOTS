using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class MightPawn : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Might Pawn");
		}
		public override void SetDefaults()
		{
			
			npc.CloneDefaults(73);
            aiType = 73; //18 is the demon scythe style
            npc.lifeMax = 125;   //boss life
            npc.damage = 17;  //boss damage
            npc.defense = 6;    //boss defense
            npc.knockBackResist = 0.2f;
            npc.width = 40;
            npc.height = 40;
			animationType = 3;
			 Main.npcFrameCount[npc.type] = 3;    //boss frame/animation
            npc.value = 750;
            npc.npcSlots = 0f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = false;
		}
		public override void NPCLoot()
		{
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.SoulofMight), 1);
		
			if((Main.rand.Next(19) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SoulSingularity")), 1);
				
			if((Main.rand.Next(199) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("MightyAquarius")), 1);	
				
			
		}
		public override void AI()
		{
			
			if( Main.rand.Next(180) == 0)
			{
		 Player player  = Main.player[npc.target];
			
			float Speed = 7f;  //projectile speed
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                int damage = 34;  //projectile damage
                int type = mod.ProjectileType("MightArrowHostile");  //put your projectile
                Main.PlaySound(23, (int)npc.position.X, (int)npc.position.Y, 17);
                float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.35f)), vector8.X - (player.position.X + (player.width * 0.5f)));
                int num54 = Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, 0);
			}
		}
		
			
			
			
}
	}
