using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Conduit
{
	public class SkipSoul : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(200);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 24;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 0, 50, 0);
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		/*public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Vector3 vColor = ColorHelpers.VibrantColor.ToVector3() * 0.34f;
			Lighting.AddLight(Item.position, vColor);
		}*/
	}
	public class SkipShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(200);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 30;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 0, 50, 0);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		/*public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Vector3 vColor = ColorHelpers.VibrantColor.ToVector3() * 0.34f;
			Lighting.AddLight(Item.position, vColor);
		}*/
	}
}