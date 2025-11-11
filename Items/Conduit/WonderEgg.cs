using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.DataStructures;
using SOTS.Items.AbandonedVillage;
using Humanizer;
using SOTS.Items.Furniture.Functional;
using SOTS.Items.Void;
using SOTS.Dusts;

namespace SOTS.Items.Conduit
{
	public class WonderEgg : ModItem
	{
		public int MyUniqueID = 0;
        public override void SaveData(TagCompound tag)
        {
			tag["MyUniqueID"] = MyUniqueID;
        }
        public override void LoadData(TagCompound tag)
        {
			MyUniqueID = tag.GetInt("MyUniqueID");
        }
        public override void NetSend(BinaryWriter writer)
        {
			writer.Write(MyUniqueID);
        }
        public override void NetReceive(BinaryReader reader)
        {
			MyUniqueID = reader.ReadInt32();
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
            drawColor = Color.White;
			Player player = Main.LocalPlayer;
			int unique = MyUniqueID;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Conduit/WonderEggSheet");
			frame = new (38 * VisionAmulet.GetGem(unique), 42 * VisionAmulet.GetFrame(unique) + 2, 36, 40);
			Color c = new(60, 60, 60, 0);
			for(int i = 0; i < 6; ++i)
			{
				Vector2 circular = new Vector2(2 * scale, 0).RotatedBy(i * MathHelper.Pi / 3f + SOTSWorld.GlobalCounter * 0.01f);
                spriteBatch.Draw(texture, position + circular, frame, c, 0f, origin, scale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
            lightColor = Color.White;
            lightColor.A = alphaColor.A;
            Player player = Main.LocalPlayer;
			int unique = MyUniqueID;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Conduit/WonderEggSheet");
			Rectangle frame = new(38 * VisionAmulet.GetGem(unique), 42 * VisionAmulet.GetFrame(unique) + 2, 36, 40);
            Vector2 origin = Item.Size / 2;
			Color c = new Color(60, 60, 60, 0);
            c.A = 0;
            for (int i = 0; i < 6; ++i)
            {
                Vector2 circular = new Vector2(4 * scale, 0).RotatedBy(i * MathHelper.Pi / 3f + SOTSWorld.GlobalCounter * 0.01f);
                spriteBatch.Draw(texture, Item.Center + circular - Main.screenPosition, frame, c, rotation, origin, scale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(texture, Item.Center - Main.screenPosition, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<JumboSurpriseEgg>();
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<JumboSurpriseEgg>();
            Item.width = 36;
            Item.height = 40;   
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Orange;
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
			Item.maxStack = Item.CommonMaxStack;
        }
        public override void OnSpawn(IEntitySource source)
        {
			MyUniqueID = Main.rand.Next(SOTSPlayer.TotalVisionNumber);

			//Item syncing is sent after this method is called, so this value should sync between server and client
        }
        public override bool CanStack(Item source)
        {
			if (source.ModItem is WonderEgg w && w.MyUniqueID == MyUniqueID)
				return true;
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			int unique = player.SOTSPlayer().UniqueVisionNumber;
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
            {
				if(line.Mod == "Terraria")
                {
                    if (line.Name == "ItemName" && line.Text == Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.EGG")) //checks the name of the tootip line
                    {
						line.OverrideColor = SOTSPlayer.VisionColorFromNumber(MyUniqueID);
                        line.Text = GetName(MyUniqueID);
                    }
                    else if (line.Name == "Tooltip1") //checks the name of the tootip line
                    {
                        if (MyUniqueID % 8 == unique % 8)
                            line.OverrideColor = SOTSPlayer.VisionColor(player);
						else
							line.OverrideColor = SOTSPlayer.VisionColorFromNumber(MyUniqueID);
                        line.Text = GetTooltip(MyUniqueID);
                        return;
                    }
                }
			}
		}
        public static string GetName(int ID)
        {
            string text = Language.GetTextValue($"Mods.SOTS.EggName.{ID}");
            return text;
        }
        public static string GetTooltip(int ID)
        {
			int unique = Main.LocalPlayer.SOTSPlayer().UniqueVisionNumber;
            if (ID == unique)
				return Language.GetTextValue("Mods.SOTS.EggDescription.x8");
            if (ID % 8 == unique % 8)
				return Language.GetTextValue($"Mods.SOTS.EggDescription.x{ID % 8}");
			return Language.GetTextValue($"Mods.SOTS.EggDescription.{unique % 8}");
		}
        private int UpdateCounter = 0;
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            UpdateCounter++;
            if(UpdateCounter % 3 == 0)
            {
                HydraulicPressTile.CheckIfInsideHydraulic(Item);
            }
            if(UpdateCounter % 60 == 0)
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, Item.whoAmI);
            }
        }
        public void Crush()
        {
            SOTSUtils.PlaySound(SoundID.Item107, Item.Center, 1.0f, -0.5f);
            if(Item.stack <= 1)
            {
                Item.active = false;
            }
            else
            {
                Item.stack--;
            }
            Color c = SOTSPlayer.VisionColorFromNumber(MyUniqueID);
            c.A = 0;
            for(int i = 0; i < 120; ++i)
            {
                float scale = Main.rand.NextFloat(1.5f, 2.5f);
                Dust d = PixelDust.Spawn(Item.Center, 0, 0, Main.rand.NextVector2Circular(10, 10) / scale, c, 3);
                d.scale = scale;

                if(i % 5 == 0)
                {
                    d = Dust.NewDustDirect(Item.position, Item.width, Item.height, DustID.t_Golden, 0, 0, 50);
                    d.scale *= 1.2f;
                }
            }
            //if(Main.netMode != NetmodeID.MultiplayerClient)
            //{
            //    Item.NewItem(Item.GetSource_Misc("SOTS:EggCrushed"), Item.Hitbox, ModContent.ItemType<JumboSurpriseEgg>(), 1, false);
            //}

            int closest = -1;
            float dist = float.MaxValue;
            for(int i =0; i < Main.player.Length; i++)
            {
                Player p = Main.player[i];
                float d = p.Distance(Item.Center);
                if (p.active && d < dist)
                {
                    closest = i;
                    dist = d;
                }
            }
            if(closest != -1)
            {
                Player p = Main.player[closest];
                p.SOTSPlayer().ResetVisionID(MyUniqueID, true, true);
            }
        }
    }
}

