using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Celestial;
using SOTS.Projectiles.Celestial;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.FakePlayer
{
    public class SubspacePlayer : ModPlayer
    {
        public FakePlayer fPlayer;
        public override void SetStaticDefaults()
        {
        }
        public bool foundItem = false;
        public bool servantActive = false;
        public bool servantIsVanity = false;
        public static SubspacePlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<SubspacePlayer>();
        }
        public override void PostUpdate()
        {
            if(servantActive)
            {
                if(fPlayer == null)
                    fPlayer = new FakePlayer(0);
                fPlayer.ItemCheckHack(Player);
            }
        }
        /*public static readonly PlayerLayer DrawServant = new PlayerLayer("SOTS", "DrawServant", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
        {
            Mod mod = ModLoader.GetMod("SOTS");
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.active)
            {
                SubspacePlayer modPlayer = SubspacePlayer.ModPlayer(drawPlayer);
                int Probe = modPlayer.Probe;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    Projectile proj = Main.projectile[i];
                    SubspaceServant subServ = proj.ModProjectile as SubspaceServant;
                    if (subServ != null && proj.owner == drawInfo.drawPlayer.whoAmI && proj.active)
                    {
                        float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
                        float drawY = (int)drawInfo.position.Y + drawPlayer.height / 2;
                        Color color = Color.White.MultiplyRGBA(Lighting.GetColor((int)drawX / 16, (int)drawY / 16));
                        subServ.PreDraw(Main.spriteBatch, color);
                        subServ.PostDraw(Main.spriteBatch, color);
                    }
                }
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            DrawServant.visible = true;
            layers.Insert(0, DrawServant);
        }*/
        public int subspaceServantShader = 0;
        public override void ResetEffects()
        {
            subspaceServantShader = 0;
            servantIsVanity = false;
            for (int i = 9 + Player.extraAccessorySlots; i < Player.armor.Length; i++) //checking vanity slots
            {
                Item item = Player.armor[i];
                if (item.type == ModContent.ItemType<SubspaceLocket>())
                {
                    servantActive = true;
                    servantIsVanity = true;
                }
                //if (Item.type == ModContent.ItemType<SubspaceLocket>())
                //{
                //    SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type);
                //}
            }
            //for (int i = 0; i < 10; i++) //iterating through armor + accessories
            //{
            //    Item item = player.armor[i];
            //    if (Item.type == ModContent.ItemType<SubspaceLocket>())
            //    {
            //        SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type);
            //    }
            //}
            /*if (servantActive)
                Summon();*/
            servantActive = false;
            foundItem = false;
        }
        public int Probe = -1;
        /*public void Summon()
        {
            int type = ModContent.ProjectileType<SubspaceServant>();
            if (Main.myPlayer == Player.whoAmI)
            {
                if (Probe == -1)
                {
                    Probe = Projectile.NewProjectile(Player.Center, Vector2.Zero, type, 0, 0, Player.whoAmI, 0);
                }
                Projectile temp = Main.projectile[Probe];
                if (!temp.active || temp.type != type || temp.owner != Player.whoAmI)
                {
                    Probe = Projectile.NewProjectile(Player.Center, Vector2.Zero, type, 0, 0, Player.whoAmI, 0);
                }
                Main.projectile[Probe].timeLeft = 6;
            }
        }*/
    }
}