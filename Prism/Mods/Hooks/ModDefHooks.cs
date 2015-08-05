﻿using System;
using System.Collections.Generic;
using System.Linq;
using Prism.API;
using Terraria;

namespace Prism.Mods.Hooks
{
    class ModDefHooks : IHookManager
    {
        IEnumerable<Action>
            onAllModsLoaded,
            onUnload       ,
            preUpdate      ,
            postUpdate     ,
            updateMusic    ;

#if DEV_BUILD
        IEnumerable<Action> updateDebug;
#endif

        public void Create()
        {
            onAllModsLoaded = HookManager.CreateHooks<ModDef, Action>(ModData.mods.Values, "OnAllModsLoaded");
            onUnload        = HookManager.CreateHooks<ModDef, Action>(ModData.mods.Values, "OnUnload"       );
            preUpdate       = HookManager.CreateHooks<ModDef, Action>(ModData.mods.Values, "PreUpdate"      );
            postUpdate      = HookManager.CreateHooks<ModDef, Action>(ModData.mods.Values, "PostUpdate"     );
            updateMusic     = HookManager.CreateHooks<ModDef, Action>(ModData.mods.Values, "UpdateMusic"    );

#if DEV_BUILD
            updateDebug     = HookManager.CreateHooks<ModDef, Action>(ModData.mods.Values, "UpdateDebug"    );
#endif
        }
        public void Clear ()
        {
            onAllModsLoaded = null;
            onUnload        = null;
            preUpdate       = null;
            postUpdate      = null;
            updateMusic     = null;

#if DEV_BUILD
            updateDebug     = null;
#endif
        }

        public void OnAllModsLoaded()
        {
            HookManager.Call(onAllModsLoaded);
        }
        public void OnUnload() //Fuckyou
        {
            HookManager.Call(onUnload);
        }
        public void PreUpdate() //Poro
        {
            HookManager.Call(preUpdate);
        }
        public void PostUpdate() //Quit
        {
            HookManager.Call(postUpdate);
        }
        public void UpdateMusic() //Doingthat
        {
            HookManager.Call(updateMusic);
        }

#if DEV_BUILD
        public void UpdateDebug()
        {
            HookManager.Call(updateDebug);
        }
#endif
    }
}
