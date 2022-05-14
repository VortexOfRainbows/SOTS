using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss.Polaris
{
	public class PolarisSpike : ModNPC
	{
		int thrusterDistance = 0;
		double dist = 128;
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(thrusterDistance);
			writer.Write(dist);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			thrusterDistance = reader.ReadInt32();
			dist = reader.ReadDouble();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polar Spike");
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =0;
            NPC.lifeMax = 1000;   
            NPC.damage = 60; 
            NPC.defense = 0;  
            NPC.knockBackResist = 0f;
            NPC.width = 54;
            NPC.height = 54;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
			npc.dontTakeDamage = true;
			npc.netAlways = true;
			npc.ai[2] = 60;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.White * ((255 - npc.alpha) / 255f), npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		public override Color? GetAlpha(Color drawColor)
        {
			return Color.White;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = 1000;
			NPC.damage = (int)(npc.damage * 0.75f);
		}
		bool runOnce = true;
		public override void AI()
		{	
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
			if (runOnce)
			{
				int myID = (int)npc.ai[0];
				npc.ai[3] = -15 * myID;
				runOnce = false;
			}
			npc.ai[2]--;
			if (npc.ai[2] < 0)
			{
				npc.ai[2] = 180;
				if (Main.netMode != 1)
				{
					thrusterDistance = Main.rand.Next(0, 301);
					npc.netUpdate = true;
				}
			}
			if (dist < thrusterDistance)
				dist++;
			if (dist > thrusterDistance)
				dist--;
			NPC polaris = Main.npc[(int)npc.ai[1]];
			if(!polaris.active || polaris.type != ModContent.NPCType<Polaris>())
			{
				npc.scale -= 0.008f;
				npc.rotation += 0.3f;
				if (npc.scale < 0)
					npc.active = false;
			}
			else
			{
				npc.ai[3]++;
				Vector2 rotateVelocity = new Vector2(128 + (float)dist, 0).RotatedBy(MathHelper.ToRadians(npc.ai[3]));
				npc.rotation = rotateVelocity.ToRotation();
				if (rotateVelocity.X > 0)
				{
					npc.rotation -= MathHelper.Pi;
					npc.spriteDirection = 1;
				}
				else
				{
					npc.spriteDirection = -1;
				}
				npc.Center = polaris.Center + rotateVelocity;
			}
		}
	}
}





















