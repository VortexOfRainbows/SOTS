using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent; 

namespace SOTS.NPCs
{
	public class LostSoul : ModNPC
	{	float ai1 = 0;
		float ai2 = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Lost Soul");
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =44;
            NPC.lifeMax = 55;
            NPC.damage = 40; 
            NPC.defense = 3;	
            NPC.knockBackResist = 0.8f;
            NPC.width = 28;
            NPC.height = 40;
			Main.npcFrameCount[NPC.type] = 6;  
            NPC.value = 550;
            NPC.boss = false;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.DeathSound = null;
			Banner = NPC.type;
			BannerItem = ItemType<LostSoulBanner>();
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), 28, 24, ModContent.DustType<LostSoulDust>());
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity.X = NPC.velocity.X;
			Main.dust[num1].velocity.Y = -3;
			Main.dust[num1].alpha = NPC.alpha;
			NPC.dontTakeDamage = false;
			
			if(ai2 == 0)
			{
				NPC.alpha++;
				if(NPC.alpha >= 235)
				{
					ai2 = -1;
				}
			}
			if(ai2 == -1)
			{
				NPC.alpha--;
				if(NPC.alpha <= 20)
				{
					ai2 = 0;
				}
			}
			ai1++;
			NPC.rotation = 0;
		}
		int frame = 0;
		public override void FindFrame(int frameHeight) 
		{
			frame = frameHeight;
			if (ai1 >= 5f) 
			{
				ai1 -= 5f;
				NPC.frame.Y += frame;
				if(NPC.frame.Y >= 6 * frame)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<SoulResidue>(), 1, 1, 2));
		}
	}
}