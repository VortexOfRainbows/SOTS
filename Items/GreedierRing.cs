using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Helpers;
using SOTS.Items.Conduit;
using SOTS.Items.Fragments;
using SOTS.Items.Gems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class GreedierRing : ModItem
    {
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/GreedierRingGlow").Value;
            Color color = new Color(110, 90, 90, 0);
            for (int k = 0; k < 4; k++)
            {
                Vector2 circular = new Vector2(scale, 0).RotatedBy(MathHelper.ToRadians(k * 90 + Main.GameUpdateCount * 2));
                Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + circular, null, color, rotation, texture.Size() / 2, scale, SpriteEffects.None, 0f);
            }
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/GreedierRingGlow").Value;
            Color color = new Color(110, 90, 90, 0);
            for (int k = 0; k < 4; k++)
            {
                Vector2 circular = new Vector2(scale, 0).RotatedBy(MathHelper.ToRadians(k * 90 + Main.GameUpdateCount * 2));
                Main.spriteBatch.Draw(texture, position + circular, null, color, 0, origin, scale, SpriteEffects.None, 0f);
            }
        }
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 40;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.defense = 2;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            //Soul of the keeper
            modPlayer.GoldenTrowel = true;
            modPlayer.KeepersBox = true;
            modPlayer.DamageGenerateMoney += 1;
            player.GetCritChance(DamageClass.Generic) += 5;
            player.tileSpeed -= 0.05f;

            //Greedy ring
            player.hasLuckyCoin = true;
            player.luck += 0.05f;
            player.goldRing = true;
            player.discountEquipped = true;

            player.VoidPlayer().VoidGenerateMoney += 2f;
        }
        public override void AddRecipes() => CreateRecipe(1).AddIngredient<SoulOfTheKeeper>(1).AddIngredient(ItemID.GreedyRing).AddIngredient<SapphireRing>(1).AddIngredient<DissolvingAether>(1).AddTile(TileID.TinkerersWorkbench).Register();
    }
}