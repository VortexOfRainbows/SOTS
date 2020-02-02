using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	public class PutridHook : ModNPC
	{	
		private float aimToX {
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}

		private float aimToY {
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}
		
		private float hookID {
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		
		private float rotationAmt {
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		
		private int storeDamage = -1;
		private int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Hook");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = -1; 
            npc.lifeMax = 225;   
            npc.damage = 32; 
            npc.defense = 8;  
            npc.knockBackResist = 0f;
            npc.width = 68;
            npc.height = 68;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.netAlways = true;
            npc.buffImmune[20] = true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[npc.target];
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/PutridHookEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			
			float shootToX = aimToX - npc.Center.X;
			float shootToY = aimToY - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 2f/ distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			
			drawColor = npc.GetAlpha(drawColor);
			drawPos.X += shootToX;
			drawPos.Y += -1 + shootToY + (texture.Height * 0.5f);
			if(npc.scale == 1)
			spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) 
		{
			if(damage > 60 && !crit)
			{
				damage = 60; //actually 50 because of defense
			}
			if(projectile.active)
			{
				projectile.velocity.X *= -0.9f;
				projectile.velocity.Y *= -0.9f;
				if(projectile.damage > 10)
				{
					projectile.damage--;
				}
				projectile.netUpdate = true;
			}
			if(npc.defense == 9999)
			{
				damage = 0;
			}
		}
		public override void NPCLoot()
		{
			if(Main.netMode != 1)
			{
				int num1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HookTurret"));	
				NPC newNpc = Main.npc[num1];
				newNpc.ai[0] = npc.ai[0];
				newNpc.ai[1] = npc.ai[1];
				newNpc.ai[2] = npc.ai[2];
				newNpc.ai[3] = npc.ai[3];
				newNpc.netUpdate = true;
			}
		}
		public override void AI()
		{	
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 1f / 155f, (255 - npc.alpha) * 1f / 155f, (255 - npc.alpha) * 1f / 155f);
			npc.rotation += 0.21f;
			int pIndex = -1;
			int totalHook = 0;
			if(storeDamage == -1)
			{
				storeDamage = npc.damage;
			}
			for(int i = 0; i < 200; i++)
			{
				NPC npc1 = Main.npc[i];
				if(npc1.type == mod.NPCType("PutridPinkyPhase2") && npc1.active && pIndex == -1)
				{
					pIndex = i;
				}
				if(npc1.type == npc.type && npc1.active)
				{
					totalHook++;
				}
			}
			if(pIndex == -1)
			{
				npc.life--;
				npc.scale *= 0.98f;
				if(npc.life < 50 || npc.scale < 0.4f)
				{
					npc.active = false;
				}
				return;
			}
			NPC putridPinky = Main.npc[pIndex];
			float rotationDistance = putridPinky.ai[3];
			float rotationSpeed = putridPinky.ai[2];
			rotationAmt += rotationSpeed;
			Vector2 rotationArea = new Vector2(rotationDistance, 0).RotatedBy(MathHelper.ToRadians(rotationAmt + (hookID * 30)));
			rotationArea += putridPinky.Center;
			npc.position = rotationArea - new Vector2(npc.width/2, npc.height/2);
				
			counter++;
			if(Main.netMode != 1 && counter % 15 == 0)
			{
				npc.netUpdate = true;
			}
				
			npc.alpha = putridPinky.alpha;
			npc.dontTakeDamage = false;
			if(totalHook <= 4)
			{
				npc.defense = 9999;
				if(npc.life < npc.lifeMax && counter % 15 == 0)
				{
					npc.life++;
				}
			}
			if(npc.alpha > 0)
			{
				npc.dontTakeDamage = true;
				npc.damage = 0;
			}
			else
			{
				npc.damage = storeDamage;
			}
		}
	}
}





















