using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Chat;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class SoulOfPlight : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
            lightColor.A = 0;
            return Color.Lerp(lightColor, Color.White, 0.8f);
        }
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 9999;
        }
        public override void PostUpdate()
        {
            Color c = Color.Lerp(new Color(100, 100, 250, 200), new Color(250, 100, 100, 200), 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter)));
            Lighting.AddLight(Item.Center, c.ToVector3() * 0.55f * Main.essScale); 
        }
    }
}