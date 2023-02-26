using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
namespace SOTS.NPCs.Constructs
{
	public class Collector : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(runOnce);
			writer.Write(toPos.X);
			writer.Write(toPos.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			runOnce = reader.ReadBoolean();
			toPos.X = reader.ReadSingle();
			toPos.Y = reader.ReadSingle();
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =0; 
			NPC.lifeMax = 420;  
			NPC.damage = 0; 
			NPC.defense = 0;  
			NPC.knockBackResist = 0.1f;
			NPC.width = 102;
			NPC.height = 58;
			Main.npcFrameCount[NPC.type] = 1;  
			NPC.value = 7075;
			NPC.npcSlots = 3f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.alpha = 255;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.dontTakeDamage = true;
		}
		float spiritScale = 0f;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/CollectorDrill").Value;
			Texture2D texture5 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/CollectorSpirit").Value;
			Vector2 drawOrigin = new Vector2(texture4.Width * 0.5f, texture4.Height * 0.5f);
			Vector2 drawOrigin2 = new Vector2(texture5.Width * 0.5f, texture5.Height * 0.5f);

			for (int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				float overrideRotation = MathHelper.ToRadians(55 * -direction);
				Vector2 fromBody = NPC.Center + new Vector2(direction * 24, 10 + NPC.ai[1] * 0.35f - NPC.ai[2] * 0.35f).RotatedBy(NPC.rotation);
				Vector2 drawPos = fromBody - screenPos + new Vector2(0f, NPC.gfxOffY);
				spriteBatch.Draw(texture4, drawPos, null, drawColor * (1f - (NPC.alpha / 255f)), NPC.rotation + overrideRotation, drawOrigin, (NPC.ai[1] - NPC.ai[2]) * 0.009f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.2f * spiritScale;
				float y = Main.rand.Next(-10, 11) * 0.2f * spiritScale;
				Vector2 drawPos = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY + 12 * (1 - spiritScale) + 2);
				spriteBatch.Draw(texture5, new Vector2(drawPos.X + x, drawPos.Y + y), null, color * (1f - (NPC.alpha / 255f)), NPC.rotation, drawOrigin2, spiritScale, SpriteEffects.None, 0f);
			}
			return true;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/CollectorBooster").Value;
			Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Texture2D texture3 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/CollectorBoosterFill").Value;
			Color color = new Color(100, 100, 100, 0);
			for (int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				Vector2 rotationOrigin = new Vector2(-2.75f * -direction, 6f) - NPC.velocity * 10f;
				float overrideRotation = rotationOrigin.ToRotation() - MathHelper.ToRadians(90);
				Vector2 fromBody = NPC.Center + new Vector2(direction * (NPC.width/2 - 4), -6).RotatedBy(NPC.rotation);
				Vector2 drawPos = fromBody - screenPos + new Vector2(0f, NPC.gfxOffY);
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.25f;
					float y = Main.rand.Next(-10, 11) * 0.25f;
					spriteBatch.Draw(texture3, new Vector2( drawPos.X + x, drawPos.Y + y ), null, color * (1f - (NPC.alpha / 255f)), NPC.rotation + overrideRotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
				}
				spriteBatch.Draw(texture2, drawPos, null, drawColor * (1f - (NPC.alpha / 255f)), NPC.rotation + overrideRotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.AvaritianDust>(), 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
			}
		}
		public override void PostAI()
		{
			NPC.rotation = NPC.velocity.X * 0.3f;
			for (int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				Vector2 rotationOrigin = new Vector2(-2.75f * -direction, 6f) - NPC.velocity * 10f;
				float overrideRotation = rotationOrigin.ToRotation();
				Vector2 dustVelo = new Vector2(7.2f, 0).RotatedBy(overrideRotation);
				Vector2 fromBody = NPC.Center + new Vector2(direction * (NPC.width / 2 - 4), -6).RotatedBy(NPC.rotation);
				int index = Dust.NewDust(fromBody + dustVelo * NPC.scale + new Vector2(-4, -4), 0, 0, ModContent.DustType<Dusts.CopyDust2>(), 0, 0, 0, Color.White);
				Dust dust = Main.dust[index];
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.velocity = dustVelo;
				dust.scale += 0.3f;
				dust.scale *= NPC.scale;
				dust.alpha = NPC.alpha;
			}
			base.PostAI();
		}
        public override bool CheckActive()
        {
			return false;
        }
        bool runOnce = true;
		bool runAway = true;
		Vector2 toPos = Vector2.Zero;
        public override void AI()
		{
			NPC.TargetClosest(false);
			NPC.spriteDirection = 1;
		}
		int extraCounter = 0;
        public override bool PreAI()
		{
			NPC.TargetClosest(false);
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.15f / 155f, (255 - NPC.alpha) * 0.25f / 155f, (255 - NPC.alpha) * 0.65f / 155f);
			if(runOnce)
            {
				runOnce = false;
				toPos = NPC.Center;
				NPC.position += new Vector2(0, -600);
				NPC.netUpdate = true;
			}
			if(NPC.ai[0] < 180)
			{
				NPC.alpha = 0;
				NPC.ai[0]++;
				Vector2 betweenPos = toPos + NPC.Center + new Vector2(0, -600);
				betweenPos /= 2f;
				Vector2 circularLocation = new Vector2(0, -300).RotatedBy(MathHelper.ToRadians(-NPC.ai[0]));
				circularLocation.X /= 2f;
				NPC.position = circularLocation + betweenPos - new Vector2(NPC.width / 2, NPC.height / 2);
				NPC.velocity = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(-NPC.ai[0]));
			}
			else
            {
				NPC.velocity *= 0f;
				if(NPC.ai[1] < 100)
				{
					NPC.ai[1] += 1f;
				}
				else if(extraCounter < 30)
                {
					extraCounter++;
                }
				else if(NPC.ai[3] < 100)
                {
					NPC.ai[3] += 0.5f;
					if(NPC.ai[3] % 5 == 0 && spiritScale < 0.8f)
					{
						SOTSUtils.PlaySound(SoundID.Item13, (int)NPC.Center.X, (int)NPC.Center.Y, 1.2f);
					}
					if (spiritScale < 0.9f)
					{
						spiritScale += 0.005f;
						for (int k = 0; k < 180; k += 10)
						{
							Vector2 circularLocation = new Vector2(-40 * NPC.scale, 0).RotatedBy(MathHelper.ToRadians(k));
							circularLocation += 0.5f * new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
							int type = DustID.Electric;
							if (Main.rand.NextBool(30))
							{
								int num1 = Dust.NewDust(new Vector2(NPC.Center.X + circularLocation.X - 4, NPC.Center.Y + circularLocation.Y - 6), 4, 4, type);
								Main.dust[num1].noGravity = true;
								Main.dust[num1].scale *= 1f + 0.166f * spiritScale;
								Main.dust[num1].velocity = -circularLocation * 0.07f;
							}
						}
					}
				}
				else if(NPC.ai[2] < 100)
				{
					NPC.ai[2] += 1f;
				}
				else
                {
					NPC.ai[3]++;
					float ai3 = NPC.ai[3] - 100;
					Vector2 circularVelo = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(ai3 + 0.5f));
					NPC.velocity = new Vector2(-circularVelo.X * 0.1f, 0).RotatedBy(MathHelper.ToRadians(-48));
					if(ai3 % 20 == 0 && ai3 < 125)
						SOTSUtils.PlaySound(SoundID.Item15, (int)NPC.Center.X, (int)NPC.Center.Y, 0.3f + ai3 * 0.05f);
					if(ai3 == 125)
						SOTSUtils.PlaySound(SoundID.Item121, (int)NPC.Center.X, (int)NPC.Center.Y, 1.3f);
					if (ai3 > 180 && runAway)
                    {
						for (int j = 0; j < 300; j++)
                        {
							NPC.position += NPC.velocity * 4f;
							for (int i = 0; i < 2; i++)
							{
								int direction = i * 2 - 1;
								Vector2 rotationOrigin = new Vector2(-2.75f * -direction, 6f) - NPC.velocity * 10f;
								float overrideRotation = rotationOrigin.ToRotation();
								Vector2 dustVelo = new Vector2(7.2f, 0).RotatedBy(overrideRotation);
								Vector2 fromBody = NPC.Center + new Vector2(direction * (NPC.width / 2 - 4), -6).RotatedBy(NPC.rotation);
								int index = Dust.NewDust(fromBody + dustVelo * NPC.scale + new Vector2(-4, -4), 0, 0, ModContent.DustType<Dusts.CopyDust3>(), 0, 0, 0, Color.White);
								Dust dust = Main.dust[index];
								dust.noGravity = true;
								dust.fadeIn = 0.1f;
								dust.velocity = dustVelo;
								dust.scale += 0.3f;
								dust.scale *= NPC.scale;
								dust.alpha = NPC.alpha;
							}
						}
						runAway = false;
					}
					if(ai3 > 240)
                    {
						NPC.active = false;
                    }
				}
            }
			return true;
		}
	}
}