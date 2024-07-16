using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class BCatalogue : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 30;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 0;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
		}
		public override bool? UseItem(Player player)
		{
			for(int j = 0; j < 50; j++)
				for(int i = 0; i < Main.npcFrameCount.Length; i++)
				{
					NPC npc = new NPC();
					npc.SetDefaults(i);
					Main.BestiaryTracker.Kills.RegisterKill(npc);
				}
			return true;
		}
	}
}