using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SOTS.Void;
using Terraria.Localization;
using static System.Net.Mime.MediaTypeNames;
using Terraria.Chat;
using System.IO;

namespace SOTS.Items.ChestItems
{
	public class WishingStar : ModItem
    {
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(MyPlayer);
        }
        public override void NetReceive(BinaryReader reader)
        {
            MyPlayer = reader.ReadInt32();
        }
        public override ModItem Clone(Item newEntity)
        {
            var clone = base.Clone(newEntity);
            if (clone is WishingStar star)
                star.MyPlayer = MyPlayer;
            return clone;
        }
        public int MyPlayer = -1;
        public static bool IsAlternate => Main.LocalPlayer.SOTSPlayer().UniqueVisionNumber % 8 == 7; //Basically checking for the nameless vision
        public string appropriateNameRightNow => IsAlternate ? this.GetLocalizedValue("AltDisplayName") : this.GetLocalizedValue("DisplayName");
        public override void UpdateInventory(Player player)
        {
            MyPlayer = player.whoAmI;
            //if (Main.netMode != NetmodeID.Server)
            //    Main.NewText(MyPlayer, Color.Gray);
            //else
            //    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(MyPlayer.ToString()), Color.Orange);
            SetOverridenName();
        }
        public override void PostUpdate()
        {
            //if(Main.netMode != NetmodeID.Server)
            //    Main.NewText(MyPlayer);
            //else
            //    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(MyPlayer.ToString()), Color.Red);
            SetOverridenName();
        }
        public void SetOverridenName()
        {
            if (Item.type == ModContent.ItemType<WishingStar>())
                Item.SetNameOverride(appropriateNameRightNow);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria")
                {
                    if(line.Name == "Tooltip0")
                    {
                        if (!IsAlternate)
                            line.Text = Language.GetTextValue("Mods.SOTS.Items.WishingStar.DefaultTooltip");
                        else
                            line.Text = Language.GetTextValue("Mods.SOTS.Items.WishingStar.AltTooltip");
                    }
                    else if(line.Name == "Tooltip1")
                    {
                        int num = Main.LocalPlayer.SOTSPlayer().UniqueVisionNumber % 8;
                        line.Text = Language.GetTextValue($"Mods.SOTS.Items.WishingStar.Flavor{num}");
                        line.OverrideColor = SOTSPlayer.VisionColor(Main.LocalPlayer);
                    }
                    else if(line.Name == "ItemName")
                    {
                        if (!IsAlternate)
                            line.Text = Language.GetTextValue("Mods.SOTS.Items.WishingStar.DisplayName");
                        else
                            line.Text = Language.GetTextValue("Mods.SOTS.Items.WishingStar.AltDisplayName");
                    }
                }
            }
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Item[Type].Value;
            Texture2D textureG = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/ChestItems/WishingStarGlow");
            if(IsAlternate)
            {
                texture = ModContent.Request<Texture2D>("SOTS/Items/ChestItems/ShatteredDreams").Value;
                textureG = ModContent.Request<Texture2D>("SOTS/Items/ChestItems/ShatteredDreamsGlow").Value;
            }
            for (int i = 0; i < 8; i++)
            {
                float sinusoid = 4f + 2f * (float)Math.Sin(SOTSWorld.GlobalCounter * MathHelper.Pi / 90f);
                Vector2 circular = new Vector2(sinusoid * scale, 0).RotatedBy(i * MathHelper.TwoPi / 8f);
                spriteBatch.Draw(textureG, position + circular, frame, new Color(120, 110, 130, 0), 0f, origin, scale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Item[Type].Value;
            Texture2D textureG = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/ChestItems/WishingStarGlow");
            if (IsAlternate)
            {
                texture = ModContent.Request<Texture2D>("SOTS/Items/ChestItems/ShatteredDreams").Value;
                textureG = ModContent.Request<Texture2D>("SOTS/Items/ChestItems/ShatteredDreamsGlow").Value;
            }
            Vector2 origin = Item.Size / 2;
            for (int i = 0; i < 8; i++)
            {
                float sinusoid = 4f + 2f * (float)Math.Sin(SOTSWorld.GlobalCounter * MathHelper.Pi / 90f);
                Vector2 circular = new Vector2(sinusoid * scale, 0).RotatedBy(i * MathHelper.TwoPi / 8f);
                spriteBatch.Draw(textureG, Item.Center - Main.screenPosition + circular, null, new Color(120, 110, 130, 0), rotation, origin, scale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 52;     
            Item.height = 52;   
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.WishingStar = true;
        }
    }
}