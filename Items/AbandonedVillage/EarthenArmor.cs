using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.Localization;

namespace SOTS.Items.AbandonedVillage
{
    [AutoloadEquip(EquipType.Head)]
    public class EarthenHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
            SetupDrawing();
        }
        private void SetupDrawing()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHatHair[equipSlotHead] = true;
            ArmorIDs.Head.Sets.DrawFullHair[equipSlotHead] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 16;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            SetupDrawing();
            return body.type == ModContent.ItemType<EarthenChestplate>() && legs.type == ModContent.ItemType<EarthenLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.Earthen");
            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            voidPlayer.bonusVoidGain += 2f;
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            modPlayer.VibrantArmor = true;
        }
        public override void UpdateEquip(Player player)
        {
            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            voidPlayer.voidMeterMax2 += 50;
            player.GetCritChance(DamageClass.Ranged) += 5;
        }
    }
    [AutoloadEquip(EquipType.Body)]
	public class EarthenChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
            SetupDrawing();
        }
        private void SetupDrawing()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[equipSlotBody] = false;
            ArmorIDs.Body.Sets.showsShouldersWhileJumping[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = false;
        }
        //public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/VibrantChestplateGlow").Value;
        //public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        //{
        //	Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
        //	Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        //}
        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 5;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<EarthenHelmet>() && legs.type == ModContent.ItemType<EarthenChestplate>();
        }
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			player.GetDamage<VoidGeneric>() += 0.10f;
			player.GetDamage(DamageClass.Ranged) += 0.05f;
		}
	}
    [AutoloadEquip(EquipType.Legs)]
    public class EarthenLeggings : ModItem
    {
        //public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/VibrantLeggingsGlow").Value;
        //public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        //{
        //    Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
        //    Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        //}
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EarthenChestplate>() && head.type == ModContent.ItemType<EarthenHelmet>();
        }
        public override void UpdateEquip(Player player)
        {
            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            voidPlayer.voidCost -= 0.10f;
        }
    }
}