using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Pyramid.GhostPepper;
using Terraria.ID;

namespace SOTS.Items.Invidia
{
	public class Sunbulb : ModItem
    {
        public static bool IsBroken => SOTSWorld.SunbulbFailed;
        public static bool IsPowered => !IsBroken && SOTSWorld.SunbulbSolved;
        public string AppropriateNameRightNow => IsBroken ? this.GetLocalizedValue("BrokenDisplayName") : IsPowered ? this.GetLocalizedValue("AltDisplayName") : this.GetLocalizedValue("DisplayName");
        public Texture2D altTexture => ModContent.Request<Texture2D>(Texture + "On").Value;
        public Texture2D deadTexture => ModContent.Request<Texture2D>(Texture + "Shattered").Value;
        public override string Texture => "SOTS/Items/Invidia/Sunbulb";
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D t;
            if (IsBroken)
                t = deadTexture;
            else if (IsPowered)
                t = altTexture;
            else
                t = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(t, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D t;
            if (IsBroken)
                t = deadTexture;
            else if (IsPowered)
                t = altTexture;
            else
                t = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(t, Item.Center - Main.screenPosition, null, lightColor, rotation, Item.Size / 2, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void UpdateInventory(Player player)
        {
            SetOverridenName();
        }
        public override void PostUpdate()
        {
            SetOverridenName();
        }
        public void SetOverridenName()
        {
            Item.SetNameOverride(AppropriateNameRightNow);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria")
                {
                    if (line.Name == "Tooltip0")
                    {
                        if (IsBroken)
                            line.Text = Language.GetTextValue("Mods.SOTS.Items.Sunbulb.BrokenTooltip");
                        else if(IsPowered)
                            line.Text = Language.GetTextValue("Mods.SOTS.Items.Sunbulb.AltTooltip");
                        else
                            line.Text = Language.GetTextValue("Mods.SOTS.Items.Sunbulb.DefaultTooltip");
                    }
                }
            }
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 52;
			Item.height = 52;
			Item.maxStack = 1;
			Item.rare = ModContent.RarityType<SunbulbRarity>();
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.accessory = true;
			Item.hasVanityEffects = true;
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            SetOverridenName();
            if(IsBroken)
            {
                player.endurance -= 1f;
                SOTSWorld.lightingChange -= 0.05f;
            }
            else if(IsPowered)
            {
                player.GetDamage(DamageClass.Generic) += 0.15f;
                player.GetCritChance(DamageClass.Generic) += 15;
                player.endurance += 0.15f;
                SOTSWorld.lightingChange += 0.05f;
            }
            else
            {
                player.GetDamage(DamageClass.Generic) += 0.01f;
                player.GetCritChance(DamageClass.Generic) += 1;
                player.endurance += 0.01f;
            }
        }
	}
}