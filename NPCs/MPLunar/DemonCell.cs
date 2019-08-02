using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.MPLunar
{
	public class DemonCell : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Devil Cell");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14;  //5 is the flying AI
            npc.lifeMax = 500;   //boss life
            npc.damage = 200;  //boss damage
            npc.defense = 0;    //boss defense
            npc.knockBackResist = 0.25f;
            npc.width = 38;
            npc.height = 42;
            animationType = NPCID.Probe;   //this boss will behavior like the DemonEye
			 Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 200;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.buffImmune[24] = true;
            npc.netAlways = false;
		}public override void AI()
		{	
		 Player player  = Main.player[npc.target];
			if( Main.rand.Next(500) == 0)
			{
			float Speed = 4f;  //projectile speed
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                int damage = 50;  //projectile damage
                int type = 573;  //put your projectile
                Main.PlaySound(23, (int)npc.position.X, (int)npc.position.Y, 17);
                float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
                int num54 = Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, 0);
		   if (Main.player[npc.target].dead)
		   {
			   npc.timeLeft = 0;
			   npc.position.Y += 100000;
		   }
		   else
		   {
			   npc.timeLeft = 10000;
		   }
			}
			
		}
	
	}
}