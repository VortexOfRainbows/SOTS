using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.FakePlayer;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Celestial;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
    public class SubspaceLocket : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 50;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }
        bool accessory = true;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FakeModPlayer modPlayer = FakeModPlayer.ModPlayer(player);
            modPlayer.servantActive = true;
            player.GetDamage(DamageClass.Generic) *= 0.75f;

            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            voidPlayer.voidMeterMax2 += 50;
            SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
            sPlayer.additionalHeal -= 40;
        }
        public override void UpdateVanity(Player player)
        {
            FakeModPlayer modPlayer = FakeModPlayer.ModPlayer(player);
            modPlayer.servantActive = true;
            modPlayer.servantIsVanity = true;
        }
        public override void UpdateInventory(Player player)
        {
            accessory = false;
        }
        public override string Texture => "SOTS/Items/Celestial/SubspaceLocket";
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            FakeModPlayer modPlayer = FakeModPlayer.ModPlayer(player);
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/Items/Celestial/SubspaceLocket").Value;
            Texture2D itemTextureOutline = ModContent.Request<Texture2D>("SOTS/Items/Celestial/GreenSubspaceLocket").Value;
            if (modPlayer.foundItem && accessory)
                for (int i = 0; i < 4; i++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * i));
                    spriteBatch.Draw(itemTextureOutline, position + circular, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
                }
            spriteBatch.Draw(texture, position, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<TerminalCluster>(1).AddIngredient<PrecariousCluster>(1).AddIngredient<Invidia.VoidTablet>(1).AddIngredient<SanguiteBar>(15).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}