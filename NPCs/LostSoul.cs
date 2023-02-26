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
		public override void SetDefaults()
		{
            NPC.aiStyle = 44;
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
            #region generating dust with existing methods
            int num1 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), 28, 24, ModContent.DustType<LostSoulDust>());
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity.X = NPC.velocity.X;
			Main.dust[num1].velocity.Y = -3;
			Main.dust[num1].alpha = NPC.alpha;
			NPC.dontTakeDamage = false;
			#endregion

			//The following two if statements control the method by which the Non-Player-Character's (NPC) transparency changes
			if (ai2 == 0)
			{
				NPC.alpha++; //Alpha is a measurement of transparency in the RGBA color/drawing system
				if (NPC.alpha >= 235) //When transparency is above 234/255, swap mode the ai2 variable, which begins the process of turning more opaque
				{
					ai2 = -1;
				}
			}
			if(ai2 == -1)
			{
				NPC.alpha--;
				if(NPC.alpha <= 20) //When transparancy is below 21/255, swap back to increasing transparency
				{
					ai2 = 0;
				}
			}
			ai1++;
			NPC.rotation = 0; //Prevents the NPC from changing its rotation, which is necessary as the AI it inherits from changes in a way innappropriate for this NPC
		}
		int frame = 0;
		public override void FindFrame(int frameHeight) 
		{
			frame = frameHeight;
			if (ai1 >= 5f) //ai1 is a variable that functions as a timer for the NPC's animations
			{
				ai1 -= 5f;
				NPC.frame.Y += frame;
				if(NPC.frame.Y >= 6 * frame) //The NPC has 6 total animation frames, which is demonstrated by the animation cycling the moment the nonexistant 7th frame is met
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