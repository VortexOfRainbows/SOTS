using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Inferno;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Inferno
{
	public class LesserWisp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lesser Wisp");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 25;   
            npc.damage = 33; 
            npc.defense = 10;  
            npc.knockBackResist = 0.5f;
            npc.width = 26;
            npc.height = 32;
			Main.npcFrameCount[npc.type] = 1;  
            npc.value = 50;
            npc.npcSlots = 0.25f;
            npc.HitSound = SoundID.NPCHit30;
            npc.DeathSound = SoundID.NPCDeath6;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.buffImmune[BuffID.Ichor] = true;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.ai[0] = 0;
			npc.ai[1] = -360;
			//banner = npc.type;
			//bannerItem = ItemType<HoloSwordBanner>();
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(255, 69, 0, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = npc.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
			texture = ModContent.GetTexture("SOTS/NPCs/Inferno/LesserWispOutline");
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			color = new Color(80, 80, 80, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 drawPos = npc.Center - Main.screenPosition;
				Vector2 circular = new Vector2(Main.rand.NextFloat(0, 3), 0).RotatedBy(Math.PI / 6 * k);
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * 0.9f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			texture = Main.npcTexture[npc.type];
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, new Color(180, 180, 180), npc.rotation, drawOrigin, npc.scale * 0.9f, SpriteEffects.None, 0f);
			return false;
		}
		Vector2 savePlayerPos = Vector2.Zero;
        public override bool PreAI()
        {
			if(npc.ai[0] == 0)
            {
				if(Main.netMode != 1)
					for(int i = 0; i < Main.rand.Next(2, 5); i++)
						NPC.NewNPC((int)npc.Center.X, (int)npc.position.Y + npc.height, this.npc.type, 0, -1, -Main.rand.Next(300) - 360);
				npc.ai[0] = -1;
			}
			for(int i = 0; i < 1 + Main.rand.Next(2); i++)
			{
				Vector2 rotational = new Vector2(0, -4.4f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-20f, 20f)));
				rotational.X *= 0.5f;
				rotational.Y *= 1f;
				particleList.Add(new FireParticle(npc.Center, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.2f, 1.6f)));
			}
			cataloguePos();
			if (Main.rand.NextBool(20))
			{
				int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - new Vector2(5), npc.width, npc.height, 267);
				Dust dust = Main.dust[dust2];
				dust.color = new Color(155, 69, 0, 0);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
			return base.PreAI();
		}
		List<FireParticle> particleList = new List<FireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
				}
			}
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int orbital = modPlayer.orbitalCounter;
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC other = Main.npc[i];
				if (npc.type == other.type && other.active && npc.active && npc.target == other.target)
				{
					if (npc == other)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			npc.localAI[1]++;
			npc.rotation = npc.velocity.X * 0.03f;

			float dynamicAddition = (float)Math.Sin(MathHelper.ToRadians(npc.localAI[1])) * 4;
			Vector2 circular = player.Center + new Vector2(0, dynamicAddition) + new Vector2(156 + 4 * total, 0).RotatedBy(MathHelper.ToRadians(orbital * 0.75f + (360f * ofTotal / total)));
			Vector2 toPlayer = circular - npc.Center;
			float speed = 7.8f;
			float length = toPlayer.Length();
			if(length < speed)
            {
				speed = length;
            }
			npc.velocity *= 0.5f;
			npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed * 0.5f;
			npc.ai[1]++;
			if (npc.ai[1] > 0)
            {
				float num = npc.ai[1];
				float chargeMult = (40f - Math.Abs(num)) / 40f;
				if (chargeMult < 0)
					chargeMult = 0;
				npc.velocity *= chargeMult;
				if (npc.ai[1] == 40)
                {
					if(Main.netMode != 1)
					{
						toPlayer = player.Center - npc.Center;
						toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
						int Damage = npc.damage / 2;
						if (Main.expertMode)
						{
							Damage = (int)(Damage / Main.expertDamage);
						}
						Projectile.NewProjectile(npc.Center + toPlayer * 40, toPlayer * 5, ProjectileType<LesserWispLaser>(), Damage, 1f, Main.myPlayer);
					}
                }
				if (npc.ai[1] >= 80)
                {
					npc.ai[1] = -(Main.rand.Next(35, 80 + 20 * total) + total * 10);
					if (Main.netMode != 1)
						npc.netUpdate = true;
				}
            }
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{ 
			if (Main.rand.NextBool(15))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LivingFireBlock, Main.rand.Next(10, 21));
			else if (Main.rand.NextBool(25))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfInferno>(), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 20.0)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Fire, (float)(2 * hitDirection), -2f, 0, default, 0.5f);
					dust.scale *= 1.5f;
					num++;
				}
			}
			else
			{
				for (int i = 0; i < particleList.Count; i++)
				{
					FireParticle particle = particleList[i];
					Dust dust = Dust.NewDustDirect(new Vector2(particle.position.X - 4, particle.position.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 1.35f;
					dust.scale = particleList[i].scale * 1.25f + 0.25f;
					dust.fadeIn = 0.1f;
					dust.color = new Color(155, 69, 0, 0);
				}
				for (int k = 0; k < 30; k++)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Fire, (float)(2 * hitDirection), -2f, 0, default, 1f);
					dust.scale *= 2.5f;
				}
			}
		}
	}
}