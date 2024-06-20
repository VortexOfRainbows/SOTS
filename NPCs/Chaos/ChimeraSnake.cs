using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Chaos
{
	public class ChimeraSnake : ModNPC
	{
		private float ownerID
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float aiCounter
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float aiCounter2
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float UniqueMultiplier
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            Main.npcFrameCount[NPC.type] = 3;
        }
		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
            NPC.lifeMax = 200;  
            NPC.damage = 60; 
            NPC.defense = 30;  
            NPC.knockBackResist = 0.1f;
            NPC.width = 26;
            NPC.height = 26;
            NPC.value = 0;
            NPC.npcSlots = 0f;
			NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.dontTakeDamage = true;
		}
		float counter2 = 0;
		float randMult = 1f;
		bool runOnce = true;
		float[] counterArr = new float[6];
		float[] randSeed1 = new float[6];
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            bool flip = false;
            if (Math.Abs(MathHelper.WrapAngle(NPC.rotation)) <= MathHelper.ToRadians(90))
            {
                flip = true;
            }
            float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Chaos/ChimeraSnakeBody");
			Vector2 drawOrigin = new Vector2(0, texture.Height / 2); 
			NPC owner = Main.npc[(int)ownerID];
			Vector2 myCenter = NPC.Center - new Vector2(12, 2 * (flip ? -1 : 1)).RotatedBy(NPC.rotation);
			if (owner.type == ModContent.NPCType<Chimera>() && owner.active)
			{
				Vector2 p0 = ownerCenter;
				Vector2 p1 = ownerCenter - new Vector2(120 * OwnerDir, 16); //Add some variety to the movement of the curve using a sinusoidal function
				Vector2 p2 = myCenter - new Vector2(140, 32 * (flip ? -1 : 1)).RotatedBy(NPC.rotation);
				Vector2 p3 = myCenter;
				int segments = 36;
				for (int i = 0; i < segments; i++)
                {
                    float t = i / (float)segments;
					Vector2 drawPos2 = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3); 
					t = (i + 1) / (float)segments;
					Vector2 drawPosNext = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
					Vector2 toNext = (drawPosNext - drawPos2);
                    float rotation = toNext.ToRotation();
                    float distance = toNext.Length();
                    drawColor = Lighting.GetColor((int)drawPos2.X / 16, (int)(drawPos2.Y / 16));
                    spriteBatch.Draw(texture, drawPos2 - screenPos, null, NPC.GetAlpha(drawColor), rotation, drawOrigin, NPC.scale * new Vector2((distance + 4) / (float)texture.Width, 1), SpriteEffects.None, 0f); //Terraria's main drawing function
				}
			}
			Vector2 drawPos = NPC.Center - screenPos;
			texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 6);
            spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation - bonusDir, drawOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : 0, 0f); //This draws the actual ball
			return false; 
		}
		public override bool PreAI()
		{
			if (runOnce)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
					NPC.netUpdate = true;
				for (int i = 0; i < counterArr.Length; i++)
				{
					counterArr[i] = 0;
					randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
				}
				runOnce = false;
			}
			NPC.TargetClosest(false);
			return true;
		}
		Vector2 baseVelo = Vector2.Zero;
		Vector2 rotateVector = new Vector2(12, 0);
		Vector2 ownerCenter = Vector2.Zero;
		private float OwnerDir = 0;
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC owner = Main.npc[(int)ownerID];
			OwnerDir = MathHelper.Lerp(OwnerDir, owner.direction, 0.056f);
            ownerCenter = new Vector2(owner.Center.X - ((owner.width / 2 - 14) * (OwnerDir * 0.5f + owner.direction * 0.5f)), owner.Center.Y + 6 + owner.gfxOffY);
            if (owner.type != ModContent.NPCType<Chimera>() || !owner.active)
			{
				NPC.life = -100;
				HitEffect(new NPC.HitInfo() { Damage = NPC.life, HitDirection = NPC.direction });
				NPC.active = false;
			}
			Vector2 distanceToOwner = owner.Center - NPC.Center;
			Vector2 toTarget = player.Center - NPC.Center;
            float distanceToTarget2 = toTarget.Length();
			toTarget = toTarget.SafeNormalize(Vector2.Zero);

            rotateVector += toTarget;
			rotateVector = rotateVector.SafeNormalize(Vector2.Zero);
			float returnToOwnerSpeed = 0.0005f;
            if (aiCounter2 < 0)
            {
                NPC.velocity *= 0.985f;
                NPC.velocity += toTarget * 0.25f;
                returnToOwnerSpeed = 0.0002f;
            }
			else
			{
                NPC.velocity *= 0.98f;
            }
            NPC.velocity.X += distanceToOwner.X * returnToOwnerSpeed;
            NPC.velocity.Y += distanceToOwner.Y * returnToOwnerSpeed;
            NPC.velocity.Y += -0.02f;
			NPC.velocity += rotateVector * 0.02f;
			if(aiCounter2 < 0)
			{
				NPC.velocity += rotateVector * 0.03f * (90 - aiCounter2) / 90f;
			}
			baseVelo *= 0.95f;
			baseVelo += NPC.velocity.SafeNormalize(Vector2.Zero) * (float)Math.Sqrt(NPC.velocity.Length());

			float overlapVelocity = 0.5f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC other = Main.npc[i];
				if (i != NPC.whoAmI && other.active && (int)other.ai[0] == (int)ownerID && Math.Abs(NPC.position.X - other.position.X) + Math.Abs(NPC.position.Y - other.position.Y) < NPC.width && other.type == NPC.type)
				{
					if (NPC.position.X < other.position.X) 
						NPC.velocity.X -= overlapVelocity;
					else 
						NPC.velocity.X += overlapVelocity;
					if (NPC.position.Y < other.position.Y) 
						NPC.velocity.Y -= overlapVelocity;
					else 
						NPC.velocity.Y += overlapVelocity;
				}
			}

			if(aiCounter2 >= 0)
            {
                NPC.rotation = toTarget.ToRotation();
            }
			else
			{
				NPC.rotation = MathHelper.Lerp(NPC.rotation, toTarget.ToRotation(), 0.02f);
			}
			aiCounter += 1 * NPC.direction;

			aiCounter2++;
			if (aiCounter2 >= 210 && distanceToTarget2 < 480f)
			{
				NPC.velocity += rotateVector * 14f;
				NPC.netUpdate = true;
				aiCounter2 = -30;
				SOTSUtils.PlaySound(SoundID.Item96, (int)NPC.Center.X, (int)NPC.Center.Y, 0.6f, -0.3f);
			}
		}
        public override void FindFrame(int frameHeight)
        {
            if (aiCounter2 < 0)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 4)
                {
					NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0;
                }
				if(NPC.frame.Y > frameHeight * 2)
				{
					NPC.frame.Y = frameHeight * 2;
					NPC.frameCounter = 0;

                }
            }
			else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 8)
                {
                    NPC.frame.Y -= frameHeight;
                    NPC.frameCounter = 0;
                }
                if (NPC.frame.Y < 0)
                {
                    NPC.frameCounter = NPC.frame.Y = 0;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Venom, 120, true);
        }
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
				return;
            for (int k = 0; k < 12; k++)
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.t_Cactus, 1.5f * hit.HitDirection, -2f, NPC.alpha, default, 1.3f);
                dust.velocity *= 0.5f;
            }
            bool flip = false;
            if (Math.Abs(MathHelper.WrapAngle(NPC.rotation)) <= MathHelper.ToRadians(90))
            {
                flip = true;
            }
            Vector2 myCenter = NPC.Center - new Vector2(12, 2 * (flip ? -1 : 1)).RotatedBy(NPC.rotation);
            Vector2 p0 = ownerCenter;
            Vector2 p1 = ownerCenter - new Vector2(120 * OwnerDir, 16); //Add some variety to the movement of the curve using a sinusoidal function
            Vector2 p2 = myCenter - new Vector2(140, 32 * (flip ? -1 : 1)).RotatedBy(NPC.rotation);
            Vector2 p3 = myCenter;
            int segments = 36;
            for (int i = 0; i < segments; i++)
            {
                float t = i / (float)segments;
                Vector2 drawPos2 = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
                for (int k = 0; k < Main.rand.Next(1, 3); k++)
                {
					short d = DustID.t_Cactus;
                    if (Main.rand.NextBool(2))
                        d = DustID.Blood;
                    Dust dust = Dust.NewDustDirect(drawPos2 - new Vector2(5), 0, 0, d, 1.5f * hit.HitDirection, -2f, NPC.alpha, default, 1.3f);
					dust.velocity *= 0.5f;
                }
                Gore.NewGore(NPC.GetSource_Death(), drawPos2 + Vector2.UnitY * -2f, NPC.velocity, ModGores.GoreType("Gores/Chimera/ChimeraSnakeBodyGore"), NPC.scale);
            }
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Chimera/ChimeraSnakeHeadGore"), NPC.scale);
        }
        public override bool PreKill()
		{
			return false;
		}
	}
}