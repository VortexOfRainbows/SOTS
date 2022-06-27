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
            NPC.value = 0;
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath6;
			NPC.dontTakeDamage = true;
			NPC.netAlways = true;
			NPC.ai[2] = 60;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, NPC.Center - screenPos, null, Color.White * ((255 - NPC.alpha) / 255f), NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		public override Color? GetAlpha(Color drawColor)
        {
			return Color.White;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.damage = (int)(NPC.damage * 0.75f);
		}
		bool runOnce = true;
		public override void AI()
		{	
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.9f / 255f, (255 - NPC.alpha) * 0.1f / 255f, (255 - NPC.alpha) * 0.3f / 255f);
			if (runOnce)
			{
				int myID = (int)NPC.ai[0];
				NPC.ai[3] = -15 * myID;
				runOnce = false;
			}
			NPC.ai[2]--;
			if (NPC.ai[2] < 0)
			{
				NPC.ai[2] = 180;
				if (Main.netMode != 1)
				{
					thrusterDistance = Main.rand.Next(0, 301);
					NPC.netUpdate = true;
				}
			}
			if (dist < thrusterDistance)
				dist++;
			if (dist > thrusterDistance)
				dist--;
			NPC polaris = Main.npc[(int)NPC.ai[1]];
			if(!polaris.active || polaris.type != ModContent.NPCType<Polaris>())
			{
				NPC.scale -= 0.008f;
				NPC.rotation += 0.3f;
				if (NPC.scale < 0)
					NPC.active = false;
			}
			else
			{
				NPC.ai[3]++;
				Vector2 rotateVelocity = new Vector2(128 + (float)dist, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[3]));
				NPC.rotation = rotateVelocity.ToRotation();
				if (rotateVelocity.X > 0)
				{
					NPC.rotation -= MathHelper.Pi;
					NPC.spriteDirection = 1;
				}
				else
				{
					NPC.spriteDirection = -1;
				}
				NPC.Center = polaris.Center + rotateVelocity;
			}
		}
	}
}





















