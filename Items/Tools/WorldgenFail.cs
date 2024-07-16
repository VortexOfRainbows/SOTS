using SOTS.WorldgenHelpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class WorldgenFail: ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 0;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
		}
		public override bool? UseItem(Player player)
		{
			AbandonedVillageWorldgenHelper.FlattenEvilBiome(1);

			//for(int j = 0; j < 50; j++)
			//	for(int i = 0; i < Main.npcFrameCount.Length; i++)
			//	{
			//		NPC npc = new NPC();
			//		npc.SetDefaults(i);
			//		Main.BestiaryTracker.Kills.RegisterKill(npc);
			//	}
			//SOTSWorldgenHelper.CleanUpFloatingTrees();
			return true;
		}
	}
}