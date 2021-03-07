using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class HealPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heal Pack");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 14;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 1;
		}
		public override bool OnPickup(Player player)
		{
			player.statLife += 5;
			player.HealEffect(5);
			return false;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool CanPickup(Player player)
        {
            return true;
        }
	}
	public class ManaPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Pack");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 18;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 1;
		}
		public override bool OnPickup(Player player)
		{
			player.statMana += 5;
			player.ManaEffect(5);
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override bool CanPickup(Player player)
		{
			return true;
		}
	}
}