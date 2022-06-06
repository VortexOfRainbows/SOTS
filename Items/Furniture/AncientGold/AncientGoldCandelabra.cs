using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.AncientGold
{
    public class AncientGoldCandelabra : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Gold Lamp");
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
            Item.consumable = true;
            Item.value = 0;
            Item.createTile = ModContent.TileType<AncientGoldCandelabraTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<RoyalGoldBrick>(5).AddIngredient<AncientGoldTorch>(3).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class AncientGoldCandelabraTile : Candelabra<AncientGoldCandelabra>
    {
        public override Vector3 LightClr => new Vector3(1.1f, 0.9f, 0.9f);
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("_Flame"));
            for (int k = 0; k < 5; k++)
            {
                SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(100, 100, 100, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.25f));
            }
        }
    }
}