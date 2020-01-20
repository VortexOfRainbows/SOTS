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
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Hook");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = -1; 
            npc.lifeMax = 350;   
            npc.damage = 40; 
            npc.defense = 10;  
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
				projectile.damage--;
				if(projectile.damage < 15)
				{
					projectile.damage = 15;
				}
			}
		}
		public override void AI()
		{	
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 1f / 155f, (255 - npc.alpha) * 1f / 155f, (255 - npc.alpha) * 1f / 155f);
			npc.rotation += 0.21f;
			int pIndex = -1;
			for(int i = 0; i < 200; i++)
			{
				NPC npc1 = Main.npc[i];
				if(npc1.type == mod.NPCType("PutridPinkyPhase2") && npc1.active)
				{
					pIndex = i;
					break;
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
		}
	}
}





















