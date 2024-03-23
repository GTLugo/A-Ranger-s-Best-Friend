using BestFriend.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Items.Accessories;

public class GlassEye : ModItem {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Accessories/GlassEye";

  public override void SetStaticDefaults() {
    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
  }

  public override void SetDefaults() {
    Item.width = 16;
    Item.height = 16;
    Item.accessory = true;
    Item.rare = ItemRarityID.Pink;
  }

  public override void UpdateAccessory(Player player, bool hide_visual) {
    var abilities = player.GetModPlayer<BestFriendAbilitiesPlayer>();
    // player.maxMinions += 1;
    player.GetDamage(DamageClass.Summon) += 0.10f;
    player.GetCritChance(DamageClass.Ranged) += 10;
  }
}