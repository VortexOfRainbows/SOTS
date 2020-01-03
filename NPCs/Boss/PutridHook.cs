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
            npc.lifeMax = 250;   
            npc.damage = 40; 
            npc.defense = 2;  
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
			Vector2 drawPos = npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY);
			
			float shootToX = aimToX - npc.Center.X;
			float shootToY = aimToY - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 1f / distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			
			drawPos.X += shootToX;
			drawPos.Y += shootToY;

			spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) 
		{
			if(projectile.active)
			{
				projectile.Kill();
			}
		}
		public override void AI()
		{	
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
			npc.rotation += 0.3f;
			int pIndex = -1;
			for(int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if(npc.type == mod.NPCType("PutridPinkyPhase2") && npc.active)
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





















