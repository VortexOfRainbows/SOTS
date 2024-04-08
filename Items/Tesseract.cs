using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.FakePlayer;
using SOTS.Items.OreItems;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Items.SpiritStaves;
using SOTS.Projectiles.Minions;
using SOTS.Void;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SOTS.Items
{
	public class Tesseract : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            this.SetResearchCost(1);
		}
		public override void SafeSetDefaults() 
		{
			Item.damage = 100;
			Item.knockBack = 5f;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(1, 0, 0, 0);
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<TesseractBuff>();
			Item.shoot = ModContent.ProjectileType<TesseractServant>();
		}
        public override bool BeforeUseItem(Player player)
        {
			return true;
			//Might want to implement the bottom line of code to limit the total amount of minions that can be spawned for performance and balance related reasons.
			//return player.ownedProjectileCounts[Item.shoot] < 10;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
			return false;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "ItemName")
            {
                Color outer = ColorHelpers.TesseractColor(0, 0.5f);
                Color inner = Color.Black;
                TextSnippet[] snippets = ChatManager.ParseMessage(line.Text, inner).ToArray();
                ChatManager.ConvertNormalSnippets(snippets);
                ChatManager.DrawColorCodedStringShadow(Main.spriteBatch, line.Font, line.Text, new Vector2(line.X, line.Y), outer, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);
                int outSnip;
                ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, snippets, new Vector2(line.X, line.Y), inner, line.Rotation, line.Origin, line.BaseScale, out outSnip, line.MaxWidth);
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient<ChaosSpiritStaff>(1)
                .AddIngredient<EvilSpiritStaff>(1)
                .AddIngredient<InfernoSpiritStaff>(1)
                .AddIngredient<TidalSpiritStaff>(1)
                .AddIngredient<OtherworldlySpiritStaff>(1)
                .AddIngredient<PermafrostSpiritStaff>(1)
                .AddIngredient<EarthenSpiritStaff>(1)
                .AddIngredient<NatureSpiritStaff>(1)
				.AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}