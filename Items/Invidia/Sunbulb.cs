using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Pyramid.GhostPepper;
using Terraria.ID;
using System.Formats.Asn1;
using SOTS.Void;

namespace SOTS.Items.Invidia
{
	public class Sunbulb : ModItem
    {
        public static bool IsBroken => SOTSWorld.SunbulbFailed;
        public static bool IsPowered => !IsBroken && SOTSWorld.SunbulbSolved;
        public string AppropriateNameRightNow => IsBroken ? this.GetLocalizedValue("BrokenDisplayName") : IsPowered ? this.GetLocalizedValue("AltDisplayName") : this.GetLocalizedValue("DisplayName");
        public Texture2D altTexture => ModContent.Request<Texture2D>(Texture + "On").Value;
        public Texture2D deadTexture => ModContent.Request<Texture2D>(Texture + "Shattered").Value;
        public Texture2D glassTexture => ModContent.Request<Texture2D>(Texture + "Glass").Value;
        public Texture2D altGlassTexture => ModContent.Request<Texture2D>(Texture + "OnGlass").Value;
        public Texture2D deadGlassTexture => ModContent.Request<Texture2D>(Texture + "ShatteredGlass").Value;
        public Texture2D glowTexture1 => ModContent.Request<Texture2D>(Texture + "OnGlow1").Value;
        public Texture2D glowTexture2 => ModContent.Request<Texture2D>(Texture + "OnGlow2").Value;
        public override string Texture => "SOTS/Items/Invidia/Sunbulb";
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D t;
            Texture2D tGlass;
            if (IsBroken)
            {
                t = deadTexture;
                tGlass = deadGlassTexture;
            }
            else if (IsPowered)
            {
                t = altTexture;
                tGlass = altGlassTexture;
            }
            else
            {
                t = ModContent.Request<Texture2D>(Texture).Value;
                tGlass = glassTexture;
            }
            if (IsPowered)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 circular = new Vector2(scale, 0).RotatedBy(MathHelper.ToRadians(i * 30 + SOTSWorld.GlobalCounter));
                    spriteBatch.Draw(tGlass, position + circular * 17, null, new Color(22, 22, 22, 0), 0f, origin, scale, SpriteEffects.None, 0f);
                }
            }
            spriteBatch.Draw(t, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(tGlass, position, frame, drawColor * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);
            if (IsPowered)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(scale, 0).RotatedBy(MathHelper.ToRadians(i * 60 + SOTSWorld.GlobalCounter));
                    spriteBatch.Draw(glowTexture2, position + circular * 2, null, new Color(18, 18, 18, 0), 0f, origin, scale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(glowTexture1, position + circular, null, new Color(20, 20, 20, 0), 0f, origin, scale, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Vector2 origin = Item.Size / 2;
            Texture2D t;
            Texture2D tGlass;
            if (IsBroken)
            {
                t = deadTexture;
                tGlass = deadGlassTexture;
            }
            else if (IsPowered)
            {
                t = altTexture;
                tGlass = altGlassTexture;
            }
            else
            {
                t = ModContent.Request<Texture2D>(Texture).Value;
                tGlass = glassTexture;
            }
            if (IsPowered)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(i * 30 + SOTSWorld.GlobalCounter));
                    spriteBatch.Draw(tGlass, Item.Center - Main.screenPosition + circular * 17, null, new Color(22, 22, 22, 0), rotation, origin, scale, SpriteEffects.None, 0f);
                }
            }
            spriteBatch.Draw(t, Item.Center - Main.screenPosition, null, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(tGlass, Item.Center - Main.screenPosition, null, lightColor * 0.5f, rotation, origin, scale, SpriteEffects.None, 0f);
            if (IsPowered)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(i * 60 + SOTSWorld.GlobalCounter));
                    spriteBatch.Draw(glowTexture2, Item.Center - Main.screenPosition + circular * 2, null, new Color(18, 18, 18, 0), rotation, origin, scale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(glowTexture1, Item.Center - Main.screenPosition + circular, null, new Color(20, 20, 20, 0), rotation, origin, scale, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
        public override void UpdateInventory(Player player)
        {
            SetOverridenName();
        }
        public override void PostUpdate()
        {
            if(IsPowered)
                Lighting.AddLight(Item.Center, 1.5f, 1.5f, 1.5f);
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
                player.GetDamage<VoidGeneric>() += 0.2f;
                player.endurance -= 1f;
                SOTSWorld.lightingChange -= 0.05f;
            }
            else if(IsPowered)
            {
                player.GetDamage(DamageClass.Generic) += 0.15f;
                player.GetCritChance(DamageClass.Generic) += 15;
                player.endurance += 0.15f;
                SOTSWorld.lightingChange += 0.05f;
                player.SOTSPlayer().Sunbulb = true;
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