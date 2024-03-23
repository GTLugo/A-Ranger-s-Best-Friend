using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BestFriend.Common;

public class BestFriendConfig : ModConfig {
  public override ConfigScope Mode => ConfigScope.ServerSide;

  [Header("Items")] [DefaultValue(false)]
  public bool disable_scope;
}